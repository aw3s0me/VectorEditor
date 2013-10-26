using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace WpfApplication2.Layers
{
    public enum Tools {Hand, Point, Line, Ellipse, Rectangle, Triangle, Square, Erase, Fill};
    public enum Size { Small, Medium, Large };
    public sealed class SelectionLayer
    {
        private static readonly SelectionLayer Instance = new SelectionLayer();
        SelectionLayer()
        {
            CurrentTool = Tools.Hand;
            CurrentColor = Colors.Black;
            CurrentSize = Size.Medium;
        }
        public Tools CurrentTool;
        public Color CurrentColor;
        public Size CurrentSize;
        public static SelectionLayer GetInstance
        {
            get
            {
                return Instance;
            }
        }
    }
}
