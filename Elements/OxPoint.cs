using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication2.Elements
{
    public class OxPoint
    {
        protected int _x;
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
        public void isSelected()
        {
            
        }
        public void Print(Canvas pole)
        {
            var ellipse = new Ellipse();
            ellipse.Width = 4;
            ellipse.Height = 4;
            ellipse.StrokeThickness = 2;
            //x = Convert.ToInt32(Mouse.GetPosition(pole).X);
            //y = Convert.ToInt32(Mouse.GetPosition(pole).Y);
            ellipse.Fill = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(ellipse, X);
            Canvas.SetTop(ellipse, Y);
            pole.Children.Add(ellipse);
           
        }
    }
}
