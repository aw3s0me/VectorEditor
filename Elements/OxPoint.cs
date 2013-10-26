using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WpfApplication2.Elements
{

    public class OxPoint : OxFigure
    {
        [XmlElement("X1")]
        protected int _x;
        [XmlElement("Y1")]
        protected int _y;
        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
        public OxPoint() { }
        public OxPoint(int x1, int y1)
        {
            _x = x1;
            _y = y1;
        }

        public OxPoint(OxFigure figure) : base()
        {
            
        }


        public void isSelected()
        {
            
        }
   /*     public void Print(Canvas pole)
        {
            var ellipse = new Ellipse
            {
                Width = 4,
                Height = 4,
                StrokeThickness = 2,
                Fill = new SolidColorBrush(Colors.Black)
            };
            //x = Convert.ToInt32(Mouse.GetPosition(pole).X);
            //y = Convert.ToInt32(Mouse.GetPosition(pole).Y);
            Canvas.SetLeft(ellipse, X);
            Canvas.SetTop(ellipse, Y);
            pole.Children.Add(ellipse);
           
        } */
    }
}
