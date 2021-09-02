/*
  In App.xaml:
  <Application.Resources>
	  <vm:ViewModelLocator xmlns:vm="clr-namespace:KDCtrlLib"
						   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;

using GalaSoft.MvvmLight.Ioc;

using KDCtrlLib.ViewModels.ServerViewModel;

namespace KDCtrlLib.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<ConnectViewModel>();
            SimpleIoc.Default.Register<RoleViewModel>();
            SimpleIoc.Default.Register<MainClientPageViewModel>();
            SimpleIoc.Default.Register<MainServerPageViewModel>();
            SimpleIoc.Default.Register<ConfigurationSettings>();
            SimpleIoc.Default.Register<MainServerFramePageViewModel>();
            SimpleIoc.Default.Register<StartRoundViewModel>();
        }

        public static void ClearData<TClass> () where TClass : class
		{
            SimpleIoc.Default.Unregister<TClass>();
        }

        public static void Reload<TClass>() where TClass : class
        {
            SimpleIoc.Default.Unregister<TClass>();
            SimpleIoc.Default.Register<TClass>();
        }

        public static MainWindowViewModel MainWindow => ServiceLocator.Current.GetInstance<MainWindowViewModel>();

        public static RoleViewModel Role => ServiceLocator.Current.GetInstance<RoleViewModel>();

        public static ConnectViewModel Connect => ServiceLocator.Current.GetInstance<ConnectViewModel>();

        public static ConfigurationSettings AppConfig => ServiceLocator.Current.GetInstance<ConfigurationSettings>();

        public static MainClientPageViewModel MainClient => ServiceLocator.Current.GetInstance<MainClientPageViewModel>();

        public static MainServerPageViewModel MainServer => ServiceLocator.Current.GetInstance<MainServerPageViewModel>();

        public static MainServerFramePageViewModel MainServerFrame => ServiceLocator.Current.GetInstance<MainServerFramePageViewModel>();

        public static StartRoundViewModel StartRound => ServiceLocator.Current.GetInstance<StartRoundViewModel>();
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}