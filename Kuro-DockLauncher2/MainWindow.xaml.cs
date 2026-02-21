using KuroDockLauncher.Index;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation; // アニメーションのために追加

namespace KuroDockLauncher
{
    public class IndexData
    {
        public string Name { get; set; }
        public List<string> Paths { get; set; }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var workArea = SystemParameters.WorkArea;
            this.Height = workArea.Height;
            this.Top = workArea.Top;

            // 普段は画面右端に 10ピクセル だけ残して隠します
            this.Left = workArea.Right - 211;
            LoadSettings();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            var workArea = SystemParameters.WorkArea;

            // 右端からウィンドウ全体の幅を引き、全画面をスライド表示させます
            DoubleAnimation animation = new DoubleAnimation
            {
                To = workArea.Right - this.Width,
                Duration = TimeSpan.FromSeconds(0.2) // 0.2秒でスライド
            };
            this.BeginAnimation(Window.LeftProperty, animation);
        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            var workArea = SystemParameters.WorkArea;

            // MouseEnter時と同様に、全画面をスライド表示させます
            DoubleAnimation animation = new DoubleAnimation
            {
                To = workArea.Right - this.Width,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            this.BeginAnimation(Window.LeftProperty, animation);
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            var workArea = SystemParameters.WorkArea;

            // 再び右端へ 10ピクセル だけ残してスライドして隠します
            DoubleAnimation animation = new DoubleAnimation
            {
                To = workArea.Right - 211,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            this.BeginAnimation(Window.LeftProperty, animation);

            // ランチャーからマウスが外れた際、展開されたボタン群を非表示（クリア）にします
            MiddlePanel.Children.Clear();
            FolderPulldown?.Children.Clear();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
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
        private readonly string settingsFilePath = "settings.json";

        private void SaveSettings()
        {
            var dataList = new List<IndexData>();

            // IndexPanel内のボタンから名前とパスのリストを抽出しますわ
            foreach (var child in IndexPanel.Children)
            {
                if (child is Button btn && btn.Content is string name)
                {
                    var paths = btn.Tag as List<string> ?? new List<string>();
                    dataList.Add(new IndexData { Name = name, Paths = paths });
                }
            }

            // 日本語の文字化け（Unicodeエスケープ）を防ぐ設定です
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // JSONを整形して見やすくします
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            string jsonString = JsonSerializer.Serialize(dataList, options);
            // 明示的にUTF-8を指定して書き出しますわ
            File.WriteAllText(settingsFilePath, jsonString, System.Text.Encoding.UTF8);
        }

        private void LoadSettings()
        {
            if (!File.Exists(settingsFilePath)) return;

            string jsonString = File.ReadAllText(settingsFilePath, System.Text.Encoding.UTF8);
            var dataList = JsonSerializer.Deserialize<List<IndexData>>(jsonString);

            if (dataList == null) return;

            foreach (var data in dataList)
            {
                Button indexButton = new Button
                {
                    Width = 90,
                    Height = 30,
                    Margin = new Thickness(0, 1, 0, 0),
                    Content = data.Name,
                    Tag = data.Paths,
                    AllowDrop = true
                };

                // 復元したボタンにも、IndexControl側で定義したイベントを付与しますわ
                indexButton.Drop += IndexControl.Bookmark_Drop;
                indexButton.MouseEnter += IndexControl.Bookmark_View;

                IndexPanel.Children.Add(indexButton);
            }
        }
    }
}