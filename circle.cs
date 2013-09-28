using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace WpfApplication2
{
    public class circle: point
    {
        private int _radius;
        public int radius
        {
            get
            {
                return _radius;
            }
            set
            {
                _radius = value;
            }
        }
        private Color _color;
        public Color color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }
        public circle(int x1,int y1,int rad):base(x1,y1)
        {
            _radius = rad;
        }
    }
}
