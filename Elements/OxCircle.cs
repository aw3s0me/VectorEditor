﻿using System.Windows.Media;

namespace WpfApplication2.Elements
{
    public class OxCircle : OxPoint
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
        public OxCircle(int x1,int y1,int rad):base(x1,y1)
        {
            _radius = rad;
        }
    }
}