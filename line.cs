using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2
{
    public class line:point
    {
        protected int _x2;
        protected int _y2;

        public int x2
        {
            get
            {
                return _x2;
            }
            set
            {
                _x2 = value;
            }
        }
        public int y2
        {
            get
            {
                return _y2;
            }
            set
            {
                _y2 = value;
            }
        }
        public line() { }
        public line(int x1, int y1, int x2, int y2)
            : base(x1, y1)
        {
            _x2 = x2;
            _y2 = y2;
        }

    }
}
