using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WpfApplication2
{
    class rectangle :line
    {
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
    }
}
