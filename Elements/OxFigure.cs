using System;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using WpfApplication2.Layers;
using Size = System.Windows.Size;
using ThicknessConverter = Xceed.Wpf.DataGrid.Converters.ThicknessConverter;

namespace WpfApplication2.Elements
{

    [Serializable]
    public class OxFigure : DrawingVisual
    {
        public enum Shape { Point, Ellispe, Square, Rectangle, Line };

        public Shape Name;

        public Color Color;
        public string XmlColor
        {
            get { return Color.ToString(); }
        }
 
        public Size size;

        public string XMLSize
        {
            get
            {
                
                return size.ToString();
            }

        }
        
        public Vector Vect1;
        public Vector Vect2;

        public OxFigure()
        {
            Color = SelectionLayer.GetInstance.CurrentColor;
            size = new Size(0, 0);
        }
        public OxFigure(Shape name)
        {
            Color = SelectionLayer.GetInstance.CurrentColor;
            Name = name;
        }
    }
}


