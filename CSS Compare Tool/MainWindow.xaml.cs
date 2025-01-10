using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace CSS_Compare_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        CompareViewModel compareViewModel = new CompareViewModel();
        public MainWindow()
        {
            this.DataContext = compareViewModel;
            
            InitializeComponent();
        }

        private void FluentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadThemeEditor();
        }

        public void LoadThemeEditor()
        {
            using (var reader = XmlReader.Create("CSS.xshd"))
            {
                var highlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                MainCssEditor.SyntaxHighlighting = highlighting;
                SecondCssEditor.SyntaxHighlighting = highlighting;
            }
        }

        private void DragMousePanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MouseButton.Left == e.ChangedButton)
            {
                DragMove();
            }
        }

        private void Button_Minimize(object sender, EventArgs e)
        {
            
            this.WindowState = WindowState.Minimized;

        }

        private void Button_Maximize(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void Button_Exit(object sender, EventArgs e)
        {
            Close();
        }

        private void Button_Compare(object sender, EventArgs e)
        {
            compareViewModel.MainCss = MainCssEditor.Text;
            compareViewModel.SecondCss = SecondCssEditor.Text;

            compareViewModel.Compare(this);
        }


    }
}
