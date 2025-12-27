using System.Windows;
using KuroDockLauncher.Index;

namespace KuroDockLauncher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 画面の「作業領域（タスクバーを除いた領域）」を取得します
            var workArea = SystemParameters.WorkArea;
            // ウィンドウの高さを画面の高さに合わせます
            this.Height = workArea.Height;
            // ウィンドウの左位置 = 画面右端 - ウィンドウの幅
            this.Left = workArea.Right - this.Width;
            // ウィンドウの上位置 = 作業領域の上端
            this.Top = workArea.Top;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var indexdoc = new IndexControl();
            indexdoc.ShowDialog();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}