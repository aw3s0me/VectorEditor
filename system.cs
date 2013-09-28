using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using PhoenixControlLib;

namespace WpfApplication2
{
    public enum tools {hand, point, line, ellipse, rectangle, square,erase, fill};
    public enum size { Small, Medium, Large };
    public sealed class system
    {
        private static readonly system instance = new system();
        system()
        {
            currentTool = tools.hand;
            currentColor = Colors.Black;
            currentSize = size.Medium;
        }
        public tools currentTool;
        public Color currentColor;
        public size currentSize;
        public static system getInstance
        {
            get
            {
                return instance;
            }
        }
        
    }
}
