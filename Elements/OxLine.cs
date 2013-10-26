using System.Xml.Serialization;

namespace WpfApplication2.Elements
{
    public class OxLine:OxPoint
    {
        [XmlElement("X2")]
        protected int _x2;
        [XmlElement("Y2")]
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
        public OxLine() { }
        public OxLine(int x1, int y1, int x2, int y2)
            : base(x1, y1)
        {
            _x2 = x2;
            _y2 = y2;
        }

    }
}
