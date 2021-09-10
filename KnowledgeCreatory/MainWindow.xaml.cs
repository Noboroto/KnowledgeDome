using System.Windows;

namespace KnowledgeCreatory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            #if DEBUG
                KDLib.Utilities.ReadKeyFromConfig();
                KDLib.Utilities.DemoEncryption();
                KDLib.Utilities.DemoDumpVideoToBytes();
                KDLib.Utilities.DemoVideoDecryption();
            #endif
        }
    }
}
