using System.Windows.Media;

namespace WpfApplication2.Elements
{
    class OxTriangle : OxLine
    {
        protected int _x3;
        protected int _y3;

        public int X3
        {
            get
            {
                return _x3;
            }
            set
            {
                _x3 = value;
            }
        }

        public int Y3
        {
            get
            {
                return _y3;
            }
            set
            {
                _y3 = value;
            }
        }

        public OxTriangle() { }
        public OxTriangle(int x1, int y1, int x2, int y2, int x3, int y3)
            : base(x1, y1, x2, y2)
        {
            _x3 = x3;
            _y3 = y3;
        }


        public Color Color { get; set; }
    }
}
