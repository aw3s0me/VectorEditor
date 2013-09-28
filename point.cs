using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;

namespace WpfApplication2
{
    public class point
    {
        protected int _x;
        protected int _y;
        public int x
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
        public int y
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
        public point() { }
        public point(int x1, int y1)
        {
            _x = x1;
            _y = y1;
        }
        public void isSelected()
        {
            
        }
        public void print(Canvas pole)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 4;
            ellipse.Height = 4;
            ellipse.StrokeThickness = 2;
            //x = Convert.ToInt32(Mouse.GetPosition(pole).X);
            //y = Convert.ToInt32(Mouse.GetPosition(pole).Y);
            ellipse.Fill = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);
            pole.Children.Add(ellipse);
           
        }
    }
}
