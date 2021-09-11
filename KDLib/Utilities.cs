using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;

namespace KDLib
{
    /// <summary>
    /// Class <c>Ultilities</c> provides some methods for data encryption. First, initialize the encryption key in the <c>app.config</c> file; then call <c>ReadKeyFromConfig</c> to load the key.
    /// Method <c>EncryptStringToBytes</c> is for data encryption.
    /// Method <c>DecryptBytesToString</c> is for data decryption.
    /// </summary>
    public static class Utilities
    {
        private static Aes aes = Aes.Create();
        private static readonly ImageConverter _imageConverter = new ImageConverter();

        /// <summary>
        /// Internal function for encryption
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        private static byte[] _EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private static string _DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        ///<summary>Method <c>EncryptStringToBytes</c> encode a String object to a byte[] array using the AES algorithm. To initialize an encryption key, modify the app.config file and call <c>ReadKeyFromConfig</c> to load it.</summary>
        public static byte[] EncryptStringToBytes(string plainText)
        {
            return _EncryptStringToBytes_Aes(plainText, aes.Key, aes.IV);
        }

        ///<summary>Method <c>DecryptBytesToString</c> decode a byte[] array using the AES algorithm. To initialize an encryption key, modify the app.config file and call <c>ReadKeyFromConfig</c> to load it.</summary>
        public static string DecryptBytesToString(byte[] cipherText)
        {
            return _DecryptStringFromBytes_Aes(cipherText, aes.Key, aes.IV);
        }

        public static void DemoEncryption()
        {
            /**
            string text = "Hello world 123";
            var encrypted = EncryptStringToBytes(text);
            string string_encrypted = ByteArrayToHexString(encrypted);
            File.WriteAllText("demo.txt", string_encrypted);

            string roundTrip = DecryptBytesToString(encrypted);

            MessageBox.Show(string.Format("Encrypted = \"{0}\"\nRound Trip = \"{1}\"", string_encrypted, roundTrip));
            */
            Image inputImage = Image.FromFile("test_input.jpg");
            var inputByte = CopyImageToByteArray(inputImage);
            string inputString = ByteArrayToHexString(inputByte);
            File.WriteAllText("test_input.txt", inputString);
            
            var outputByte = EncryptImageToByte("test_input.jpg");
            string outputString = ByteArrayToHexString(outputByte);
            File.WriteAllText("test_output.txt", outputString);

            DecrpytByteToImage(outputByte, true, null, null, "test_roundtrip.jpg");
        }

        public static void DemoDumpVideoToBytes()
        {
            byte[] data = File.ReadAllBytes("197.mp4");
            data = _EncryptBytesToBytes_Aes(data, aes.Key, aes.IV);
            string hexdata = ByteArrayToHexString(data);
            File.WriteAllText("197.txt", hexdata);
        }

        public static void DemoVideoDecryption()
        {
            string hex = File.ReadAllText("197.txt");
            byte[] data = HexStringToByteArray(hex);
            data = _DecryptBytesFromBytes_Aes(data, aes.Key, aes.IV);
            File.WriteAllBytes("197_roundtrip.mp4", data);
        }

        /// <summary>
        ///   
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        /// <exception cref="FormatException">when the string's length is odd</exception>
        public static byte[] HexStringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            if (NumberChars % 2 == 1)
            {
                throw new FormatException("hex string should not consist of an odd number of characters");
            }
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        /// <summary>
        /// Read Crypto key and IV from app.config
        /// </summary>
        /// <returns>True if read successfully, False otherwise</returns>
        /// <exception cref="ConfigurationErrorsException">when either not found app.confiig or invalid key/iv was found</exception>
        public static bool ReadKeyFromConfig()
        {
            string keyString = "", ivString = "";
            byte[] key = { }, iv = { };
            try
            {
                keyString = ConfigurationManager.AppSettings["aesKey"];
                if (keyString == null || keyString.Length <= 0)
                {
                    keyString = "";
                }
                key = HexStringToByteArray(keyString);

                ivString = ConfigurationManager.AppSettings["aesIV"];
                if (ivString == null || ivString.Length <= 0)
                {
                    ivString = "";
                }
                iv = HexStringToByteArray(ivString);
            }
            catch (ConfigurationErrorsException)
            {
#if DEBUG
                MessageBox.Show("Error reading app settings");
#endif
                throw new ConfigurationErrorsException();
            }
            
            if (aes.ValidKeySize(key.Length * 8) && (iv.Length *8 == aes.BlockSize))
            {
#if DEBUG
                    //MessageBox.Show("valid key and IV");
#endif  
                aes.Key = key;
                aes.IV = iv;
            }
            else
            {
#if DEBUG
                MessageBox.Show("Error reading app settings");
#endif
                throw new ConfigurationErrorsException();
            }
            return true;
        }

        static byte[] _EncryptBytesToBytes_Aes(byte[] plainData, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainData == null || plainData.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        /*
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            //swEncrypt.Write(plainData);
                        }*/
                        csEncrypt.Write(plainData, 0, plainData.Length);
                        csEncrypt.FlushFinalBlock();
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static byte[] _DecryptBytesFromBytes_Aes(byte[] cipherData, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherData == null || cipherData.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            byte[] plainData = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        csDecrypt.Write(cipherData, 0, cipherData.Length);
                        csDecrypt.Close();
                    }
                    plainData = msDecrypt.ToArray();
                }
            }

            return plainData;
        }

        /// <summary>
        /// Method to "convert" an Image object into a byte array, formatted in PNG file format, which 
        /// provides lossless compression. This can be used together with the GetImageFromByteArray() 
        /// method to provide a kind of serialization / deserialization. 
        /// </summary>
        /// <param name="theImage">Image object, must be convertable to PNG format</param>
        /// <returns>byte array image of a PNG file containing the image</returns>
        public static byte[] CopyImageToByteArray(Image theImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                theImage.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Method that uses the ImageConverter object in .Net Framework to convert a byte array, 
        /// presumably containing a JPEG or PNG file image, into a Bitmap object, which can also be 
        /// used as an Image object.
        /// </summary>
        /// <param name="byteArray">byte array containing JPEG or PNG file image or similar</param>
        /// <returns>Bitmap object if it works, else exception is thrown</returns>
        public static Bitmap GetImageFromByteArray(byte[] byteArray)
        {
            Bitmap bm = (Bitmap)_imageConverter.ConvertFrom(byteArray);

            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution ||
                               bm.VerticalResolution != (int)bm.VerticalResolution))
            {
                // Correct a strange glitch that has been observed in the test program when converting 
                //  from a PNG file image created by CopyImageToByteArray() - the dpi value "drifts" 
                //  slightly away from the nominal integer value
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f),
                                 (int)(bm.VerticalResolution + 0.5f));
            }

            return bm;
        }

        public static byte[] EncryptImageToByte(string InputImagePath, bool doesReadKeyFromConfig = true, byte[] key = null, byte[] iv = null) 
        {
            Image image = Image.FromFile(InputImagePath);
            var imageByte = CopyImageToByteArray(image);
            byte[] encryptedByte;
            if (doesReadKeyFromConfig)
            {
                ReadKeyFromConfig();
            }
            else
            {
                aes.Key = key;
                aes.IV = iv;
            }
            encryptedByte = _EncryptBytesToBytes_Aes(imageByte, aes.Key, aes.IV);
            return encryptedByte;
        }
        public static byte[] EncryptImageToByte(Image image, bool doesReadKeyFromConfig = true, byte[] key = null, byte[] iv = null)
        {
            var imageByte = CopyImageToByteArray(image);
            byte[] encryptedByte;
            if (doesReadKeyFromConfig)
            {
                ReadKeyFromConfig();
            }
            else
            {
                aes.Key = key;
                aes.IV = iv;
            }
            encryptedByte = _EncryptBytesToBytes_Aes(imageByte, aes.Key, aes.IV);
            return encryptedByte;
        }

        public static Image DecrpytByteToImage(byte[] cipherImage, bool doesReadKeyFromConfig = true, byte[] key = null, byte[] iv = null, string outputImagePath = "")
        {
            if (doesReadKeyFromConfig)
            {
                ReadKeyFromConfig();
            }
            else
            {
                aes.Key = key;
                aes.IV = iv;
            }
            byte[] decryptedByte = _DecryptBytesFromBytes_Aes(cipherImage, aes.Key, aes.IV);
            var ret = GetImageFromByteArray(decryptedByte);
            if (outputImagePath.Length > 0)
            {
                ret.Save(outputImagePath, ImageFormat.Png);
            }
            return ret;
        }
    }
}
