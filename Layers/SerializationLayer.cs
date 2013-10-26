using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;
using WpfApplication2.Elements;

namespace WpfApplication2.Layers
{
    public static class SerializationLayer
    {
        public static void SerializeToXML(CanvasLayer canvas, string filename)
        {
            try
            {
                //var fileStream = File.Create(filename);
                var elements = canvas.Visuals;
        //        using (var xmlWriter = XmlWriter.Create(filename))
        //        {
                    var xmlDoc = new XmlDocument();
                    XmlElement root = xmlDoc.CreateElement("Canvas");
                    xmlDoc.AppendChild(root);

                    var mySerializer = new XmlSerializer(typeof(Visual));
                    foreach (var element in elements)
                    {
                        XmlElement child = null;

                        var elemToSer = element as OxFigure;
                        if (elemToSer == null)
                            continue;
                        switch (elemToSer.Name)
                        {
                            case OxFigure.Shape.Point:
                                {
                                    var elemAsPoint = elemToSer as OxPoint;
                                    if (elemAsPoint == null)
                                        continue;
                                    child = xmlDoc.CreateElement("Point");
                                    child.SetAttribute("X", value: elemAsPoint.X.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("Y", value: elemAsPoint.Y.ToString(CultureInfo.InvariantCulture));
                                    child.InnerText = "Point";
                                    break;
                                }
                            case OxFigure.Shape.Square:
                                {
                                    var elemAsSquare = elemToSer as OxRectangle;
                                    if (elemAsSquare == null)
                                        continue;

                                    child = xmlDoc.CreateElement("Square");
                                    child.SetAttribute("SizeW", elemAsSquare.size.Width.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("SizeH", elemAsSquare.size.Height.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("X", value: elemAsSquare.X.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("Y", value: elemAsSquare.Y.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("X2", value: elemAsSquare.X2.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("Y2", value: elemAsSquare.Y2.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("Color", value: elemAsSquare.XmlColor);
                                    child.InnerText = "Square";
                                    break;
                                }
                            case OxFigure.Shape.Ellispe:
                                {
                                    var elemAsCircle = elemToSer as OxCircle;
                                    if (elemAsCircle == null)
                                        continue;

                                    child = xmlDoc.CreateElement("Circle");
                                    child.SetAttribute("SizeW", elemAsCircle.size.Width.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("SizeH", elemAsCircle.size.Height.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("X", value: elemAsCircle.X.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("Y", value: elemAsCircle.Y.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("Color", value: elemAsCircle.XmlColor);
                                    child.InnerText = "Circle";
                                    break;
                                }
                            case OxFigure.Shape.Rectangle:
                                {
                                    var elemAsRect = elemToSer as OxRectangle;
                                    if (elemAsRect == null)
                                        continue;

                                    child = xmlDoc.CreateElement("Rectangle");
                                    child.SetAttribute("SizeW", elemAsRect.size.Width.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("SizeH", elemAsRect.size.Height.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("X", value: elemAsRect.X.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("Y", value: elemAsRect.Y.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("X2", value: elemAsRect.X2.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("Y2", value: elemAsRect.Y2.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("Color", value: elemAsRect.XmlColor);
                                    child.InnerText = "Rectangle";
                                    break;
                                }
                            case OxFigure.Shape.Line:
                                {
                                    var elemAsLine = elemToSer as OxLine;
                                    if (elemAsLine == null)
                                        continue;

                                    child = xmlDoc.CreateElement("Line");
                                    child.SetAttribute("X", value: elemAsLine.X.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("Y", value: elemAsLine.Y.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("X2", value: elemAsLine.X2.ToString(CultureInfo.InvariantCulture));
                                    child.SetAttribute("Y2", value: elemAsLine.Y2.ToString(CultureInfo.InvariantCulture));
                                    child.InnerText = "Line";
                                    break;
                                }

                        }
                        if (child != null)
                        {
                            root.AppendChild(child);
                        }
                        xmlDoc.Save(filename);
                    }
       //         }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

        public static void DeserializeFromXML(string filename, ref CanvasLayer canvas)
        {

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                var root = xmlDoc.FirstChild;

                if (xmlDoc.DocumentElement != null)
                    //foreach (var node in xmlDoc.DocumentElement.ChildNodes)
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        if (node.Attributes != null)
                        {
                            XmlAttributeCollection attributes;
                            switch (node.Name)
                            {
                                case "Point":
                                {
                                    Debug.WriteLine("1");
                                    attributes = node.Attributes;
                                    OxFigure figure = new OxPoint();
                                    figure.Name = OxFigure.Shape.Point;
                                    var point = new Point(Int32.Parse(attributes["X"].Value), Int32.Parse(attributes["Y"].Value));
                                    DrawingLayer.GetInstance.DrawFigure(ref figure, point, true);
                                    canvas.AddVisual(figure);

                                    break;
                                }
                                case "Square":
                                {
                                    attributes = node.Attributes;
                                    OxFigure figure = new OxRectangle();
                                    figure.Name = OxFigure.Shape.Square;
                                    var point1 = new Point(Int32.Parse(attributes["X"].Value), Int32.Parse(attributes["Y"].Value));
                                    figure.size = new System.Windows.Size(Double.Parse(attributes["SizeW"].Value), Double.Parse(attributes["SizeH"].Value));
                                    figure.Color = Colors.Black;
                                    DrawingLayer.GetInstance.DrawFigure(ref figure, point1, false);
                                    canvas.AddVisual(figure);
                                    break;
                                }
                                case "Circle":
                                {
                                    attributes = node.Attributes;
                                    OxFigure figure = new OxCircle();
                                    figure.Name = OxFigure.Shape.Rectangle;
                                    var point1 = new Point(Int32.Parse(attributes["X"].Value), Int32.Parse(attributes["Y"].Value));
                                    figure.size = new System.Windows.Size(Double.Parse(attributes["SizeW"].Value), Double.Parse(attributes["SizeH"].Value));
                                    figure.Color = Colors.Black;
                                    DrawingLayer.GetInstance.DrawFigure(ref figure, point1, false);
                                    canvas.AddVisual(figure);
                                    break;
                                }
                                case "Rectangle":
                                {
                                    attributes = node.Attributes;
                                    OxFigure figure = new OxRectangle();
                                    figure.Name = OxFigure.Shape.Rectangle;
                                    var point1 = new Point(Int32.Parse(attributes["X"].Value), Int32.Parse(attributes["Y"].Value));
                                    figure.size = new System.Windows.Size(Double.Parse(attributes["SizeW"].Value), Double.Parse(attributes["SizeH"].Value));
                                    figure.Color = Colors.Black;
                                    DrawingLayer.GetInstance.DrawFigure(ref figure, point1, false);
                                    canvas.AddVisual(figure);
                                    break;
                                }
                                case "Line":
                                {
                                    attributes = node.Attributes;
                                    OxFigure figure = new OxLine();
                                    figure.Name = OxFigure.Shape.Line;
                                    var point1 = new Point(Int32.Parse(attributes["X"].Value), Int32.Parse(attributes["Y"].Value));
                                    var point2 = new Point(Int32.Parse(attributes["X2"].Value), Int32.Parse(attributes["Y2"].Value));
                                    DrawingLayer.GetInstance.DrawLine(ref figure, point1, point2);
                                    figure.Color = Colors.Black;
                                    DrawingLayer.GetInstance.DrawFigure(ref figure, point1, false);
                                    canvas.AddVisual(figure);
                                    break;
                                }


                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }



    }
}
