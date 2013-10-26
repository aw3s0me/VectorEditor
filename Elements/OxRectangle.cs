using System.Windows.Media;
using System.Xml.Serialization;

namespace WpfApplication2.Elements
{
    class OxRectangle : OxLine
    {
        public OxRectangle() { }
        public OxRectangle(int x1, int y1, int x2, int y2)
            : base(x1, y1, x2, y2)
        {
        }
    }
}
