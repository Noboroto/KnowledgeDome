using KDLib;
using KDLib.KDException;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KDClients.ViewModels
{
    class ConnectViewModel : KDViewModelBase
    {
        #region Command
        public ICommand TryConnectCmd { get; set; }
        #endregion
        
        public ConnectViewModel()
        {
            TryConnectCmd = new KDUICommand<string>(
                (s) =>
                {
                    try
                    {
                        NetClient.Connect(s);
                    }
                    catch (AggregateException ae)
                    {
                        string e = "";
                        try
                        {
                            if (ae.InnerExceptions.Count > 1)
                            {
                                foreach (var ex in ae.InnerExceptions)
                                {
                                    e += ex.Message + "\n";
                                }
                                MessageBox.Show(e);
                            }
                            else throw ae.InnerException;
                        }
                        catch (FormatException)
                        {
                            MessageBox.Show("Sai định dạng IP", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        catch (IPNotFoundException)
                        {
                            MessageBox.Show("Không tìm thấy địa chỉ IP", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Sai định dạng IP", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("e " + e.Message);
                    }
                },
                (s) =>
                {
                    return !string.IsNullOrEmpty(s);
                }
                );
        }
    }
}
