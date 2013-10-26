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

        [XmlElement("Name")]
        public Shape Name;
        [NonSerialized]
        public Color Color;
        [XmlElement("Color")]
        public string XmlColor
        {
            get { return Color.ToString(); }
        }
        [NonSerialized]
        public Size size;
        [XmlElement("Size")]
        public string XMLSize
        {
            get
            {
                
                return size.ToString();
            }

        }
        

        [NonSerialized]
        public Vector Vect1;
        [NonSerialized]
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


