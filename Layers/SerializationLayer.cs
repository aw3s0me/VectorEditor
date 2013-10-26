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

        public static CanvasLayer DeserializeFromXML(string filename)
        {
            CanvasLayer savedCanvas;
            using (var fs = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                savedCanvas = XamlReader.Load(fs) as CanvasLayer;
                fs.Close();
            }
            return savedCanvas;
            //WpfApplication2.MainWindow.La
        }



    }
}
