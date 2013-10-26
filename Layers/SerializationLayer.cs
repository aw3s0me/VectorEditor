using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Serialization;

namespace WpfApplication2.Layers
{
    public static class SerializationLayer
    {
        public static void SerializeToXML(CanvasLayer canvas, string filename)
        {
            try
            {
                //var myStrXaml = XamlWriter.Save(canvas);
                var fileStream = File.Create(filename);
                var streamWriter = new StreamWriter(fileStream);
                var elements = canvas.Visuals;
                var mySerializer = new XmlSerializer(typeof (Visual));
                //streamWriter.Write(myStrXaml);
                foreach (var element in elements)
                {
                    mySerializer.Serialize(streamWriter, element);
                }

                streamWriter.Close();
                fileStream.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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
