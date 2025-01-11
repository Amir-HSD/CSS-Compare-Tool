using CSS_Compare_Tool.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Css.Parser;
using System.Windows;
using AngleSharp.Css.Dom;

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
            ComparedCss = string.Empty;
            // CompareCssFiles
            var Compared = CompareCssFiles();

            foreach (var item in Compared)
            {
                ComparedCss += item + "\n";
            }
            //var SSSWW = Compared2.Count();
            ComparedCaption = $"Founded: {Compared.Count()} items";

            ComparedWindow comparedWindow = new ComparedWindow();

            comparedWindow.Owner = window;
            comparedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            comparedWindow.Code = ComparedCss;
            comparedWindow.Caption = ComparedCaption;


            comparedWindow.ShowDialog();

        }

        public ICssStyleSheet ParseStyleSheet(string cssContent)
        {
            var parser = new CssParser();

            var stylesheet = parser.ParseStyleSheet(cssContent);

            return stylesheet;
        }

        public HashSet<string> ExtractClassesFromCss(string cssContent)
        {
            var cssClasses = new HashSet<string>();

            var stylesheet = ParseStyleSheet(cssContent);

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

        public void Compare_v2(Window window)
        {
            ComparedCss = string.Empty;
            // CompareCssFiles
            var Compared = CompareCssFiles_v2();

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

        public HashSet<string> ExtractClassesFromCss_v2(string cssContent)
        {
            var cssClasses = new HashSet<string>();

            var stylesheet = ParseStyleSheet(cssContent);

            foreach (var rule in stylesheet.Rules)
            {
                if (rule is AngleSharp.Css.Dom.ICssStyleRule styleRule && !string.IsNullOrEmpty(styleRule.SelectorText))
                {
                    cssClasses.Add(styleRule.SelectorText);
                }
            }
            return cssClasses;
        }

        public List<string> ExtractStylesFromCss_v2(string cssContent)
        {
            

            var cssStyles = new List<string>();

            var stylesheet = ParseStyleSheet(cssContent);


            foreach (var rule in stylesheet.Rules)
            {
                if (rule is AngleSharp.Css.Dom.ICssStyleRule styleRule && !string.IsNullOrEmpty(styleRule.SelectorText))
                {
                    
                    cssStyles.Add(rule.CssText);
                }
            }

            return cssStyles;
        }

        public IEnumerable<string> CompareCssFiles_v2()
        {
            
            var mainCssClasses = ExtractClassesFromCss_v2(MainCss);
            var secondaryCssClasses = ExtractClassesFromCss_v2(SecondCss);

            var mainCssStyles = ExtractStylesFromCss_v2(MainCss);
            var secondaryCssStyles = ExtractStylesFromCss_v2(SecondCss);

            var missingInMain = secondaryCssClasses.Except(mainCssClasses);

            var missingStyles = new List<string>();

            foreach (var Styles in secondaryCssStyles)
            {
                var StyleIndexEnd = Styles.IndexOf('{');
                var Style = Styles.Substring(0, StyleIndexEnd);
                Style = Style.Trim();
                bool Check = missingInMain.Contains(Style);
                if (Check)
                {
                    missingStyles.Add(Styles);
                }
            }

            return missingStyles;
        }




    }
}
