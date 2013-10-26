using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace WpfApplication2.Elements
{
    
    public class OxCircle : OxPoint
    {
        [XmlElement("Radius")]
        private int _radius;
        
        public int Radius
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
        [XmlElement("Color")]
        public string XmlColor
        {
            get { return color.ToString(); }
        }

        public OxCircle(int x1,int y1,int rad):base(x1,y1)
        {
            _radius = rad;
        }
    }
}
