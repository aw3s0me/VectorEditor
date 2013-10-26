using System.Windows.Media;
using System.Xml.Serialization;

namespace WpfApplication2.Elements
{
    class OxRectangle : OxLine
    {
        public Color Color { get; set; }

        [XmlElement("Color")]
        public string XmlColor
        {
            get { return Color.ToString(); }
        }

    }
}
