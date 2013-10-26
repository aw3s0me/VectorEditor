using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;
using WpfApplication2.Elements;

namespace WpfApplication2.Layers
{
    [XmlRoot("Canvas")]
    public class CanvasLayer : Canvas
    {
        [XmlElement("Element")]
        private readonly List<Visual> _visuals = new List<Visual>();
        //private readonly List< 

        public List<Visual> Visuals
        {
            get
            {
                return _visuals;
            }
        } 

        protected override int VisualChildrenCount
        {
            get
            {
                return _visuals.Count;
            }
        }
        public int ChildrenCount()
        {
            return _visuals.Count;
        }
        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }
        public void AddVisual(Visual visual)
        {
            _visuals.Add(visual);
            AddVisualChild(visual);
            AddLogicalChild(visual);
        }
        public void DeleteVisual(Visual visual)
        {
            _visuals.Remove(visual);
            RemoveLogicalChild(visual);
            RemoveVisualChild(visual);
        }

        public OxFigure GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as OxFigure;
        }
        public void DelAll()
        {
            var size = VisualChildrenCount;
            for (var i = size - 1; i > -1; i--)
            {
                DeleteVisual(_visuals[i]);
            }

            var brushConverter = new BrushConverter();
            var drawingBrush = (Brush)brushConverter.ConvertFromString(Colors.White.ToString());
            Background = drawingBrush;
        }
        public void DelLast()
        {
            DeleteVisual(GetVisualChild(VisualChildrenCount - 1));
        }
        public void CopyLast()
        {
            AddVisual(GetVisualChild(VisualChildrenCount - 1));
        }
    }
}