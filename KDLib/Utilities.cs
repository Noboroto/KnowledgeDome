using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;


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
			string text = "Hello world 123";
			var encrypted = EncryptStringToBytes(text);
			string string_encrypted = ByteArrayToString(encrypted);
			File.WriteAllText("demo.txt", string_encrypted);

			string roundTrip = DecryptBytesToString(encrypted);

			MessageBox.Show(String.Format("Encrypted = \"{0}\"\nRound Trip = \"{1}\"", string_encrypted, roundTrip));
		}
		public static byte[] StringToByteArray(String hex)
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
		public static string ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
				hex.AppendFormat("{0:x2}", b);
			return hex.ToString();
		}

		static void ReadAllSettings()
		{
			/*
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    MessageBox.Show("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        MessageBox.Show(String.Format("Key: {0} Value: {1}", key, appSettings[key]));
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                MessageBox.Show("Error reading app settings");
            }*/
		}

		public static bool ReadKeyFromConfig()
		{
			string keyString = ConfigurationManager.AppSettings["aesKey"];
			if (keyString == null || keyString.Length <= 0)
			{
				keyString = "";
			}
			byte[] key = StringToByteArray(keyString);

			string ivString = ConfigurationManager.AppSettings["aesIV"];
			if (ivString == null || ivString.Length <= 0)
			{
				ivString = "";
			}
			byte[] iv = StringToByteArray(ivString);

			if (aes.ValidKeySize(key.Length * 8) && (iv.Length * 8 == aes.BlockSize))
			{
#if DEBUG
				MessageBox.Show("valid key and IV");
#endif
				aes.Key = key;
				aes.IV = iv;
				return true;
			}
			return false;
		}

	}
}
