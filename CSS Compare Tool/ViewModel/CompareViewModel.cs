using CSS_Compare_Tool.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Css.Parser;
using System.Windows;

namespace CSS_Compare_Tool
{
    public class CompareViewModel : ViewModelBase
    {
        
        public CompareViewModel()
        {
            
        }

        private string mainCss;

        public string MainCss
        {
            get { return mainCss; }
            set { mainCss = value; OnPropertyChanged(); }
        }

        private string secondCss;

        public string SecondCss
        {
            get { return secondCss; }
            set { secondCss = value; OnPropertyChanged(); }
        }

        private string comparedCss;

        public string ComparedCss
        {
            get { return comparedCss; }
            set { comparedCss = value; OnPropertyChanged(); }
        }

        private string comparedCaption;

        public string ComparedCaption
        {
            get { return comparedCaption; }
            set { comparedCaption = value; }
        }

        public static readonly DependencyProperty ProductTitleProperty = DependencyProperty.Register(
            "ProductTitle",  // "ProductTitleText" ?
            typeof(string),
            typeof(CompareViewModel)
        );


        public void Compare(Window window)
        {
            // CompareCssFiles
            var Compared = CompareCssFiles();
            foreach (var item in Compared)
            {
                ComparedCss += item + "\n";
            }

            ComparedCaption = $"Founded: {Compared.Count()} items";

            ComparedWindow comparedWindow = new ComparedWindow();

            comparedWindow.Owner = window;
            comparedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            comparedWindow.Code = ComparedCss;
            comparedWindow.Caption = ComparedCaption;


            comparedWindow.ShowDialog();

        }

        public HashSet<string> ExtractClassesFromCss(string cssContent)
        {
            var cssClasses = new HashSet<string>();
            var parser = new CssParser();

            var stylesheet = parser.ParseStyleSheet(cssContent);

            foreach (var rule in stylesheet.Rules)
            {
                if (rule is AngleSharp.Css.Dom.ICssStyleRule styleRule && !string.IsNullOrEmpty(styleRule.SelectorText))
                {
                    var selectors = styleRule.SelectorText.Split(new[] { ' ', ',', '>', '+', '~' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var selector in selectors)
                    {
                        if (selector.StartsWith("."))
                        {
                            cssClasses.Add(selector.Trim('.'));
                        }
                    }
                }
            }
            return cssClasses;
        }

        public IEnumerable<string> CompareCssFiles()
        {
            var mainCssClasses = ExtractClassesFromCss(MainCss);
            var secondaryCssClasses = ExtractClassesFromCss(SecondCss);

            var missingInMain = secondaryCssClasses.Except(mainCssClasses);

            return missingInMain;
        }


    }
}
