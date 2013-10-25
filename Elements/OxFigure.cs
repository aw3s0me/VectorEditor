using System.Windows;
using System.Windows.Media;
using WpfApplication2.Layers;
using Size = System.Windows.Size;

namespace WpfApplication2.Elements
{
    public class OxFigure : DrawingVisual
    {
        public enum Shape { Point, Ellispe, Square, Rectangle, Line };
        public Shape Name;
        public Color Color;
        public Size size;
        public Vector Vect1;
        public Vector Vect2;

        public OxFigure() : base()
        {
            Color = ServiceLayer.GetInstance.CurrentColor;
            size = new Size(0, 0);
        }
        public OxFigure(Shape name) : base() {
            Color = ServiceLayer.GetInstance.CurrentColor;
            this.Name = name;
        }
    }
}


