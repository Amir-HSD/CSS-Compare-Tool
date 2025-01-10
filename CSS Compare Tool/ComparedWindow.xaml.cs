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
using System.Windows.Shapes;
using System.Xml;
using Wpf.Ui.Controls;

namespace CSS_Compare_Tool
{
    /// <summary>
    /// Interaction logic for ComparedWindow.xaml
    /// </summary>
    public partial class ComparedWindow : FluentWindow
    {
        public string Code { get; set; }
        public string Caption { get; set; }

        public ComparedWindow()
        {
            
            InitializeComponent();
        }

        private void FluentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadThemeEditor();
            LoadCode();
        }

        public void LoadThemeEditor()
        {
            using (var reader = XmlReader.Create("CSS.xshd"))
            {
                var highlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                ComparedCssEditor.SyntaxHighlighting = highlighting;
            }
        }

        public void LoadCode()
        {
            ComparedCssEditor.Text = Code;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Code);
        }
    }
}
