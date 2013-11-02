using System.Windows;
using System.Windows.Media;
using WpfApplication2.Elements;
using WpfApplication2.Interfaces;
using WpfApplication2.Layers;

namespace WpfApplication2.Helpers
{
    public class FillCommand : ICommand
    {
        private OxFigure _value;
        //public CanvasLayer CurCanv;
        private Color _newColor;
        private Color _oldColor;
        private Point _coord;

        public OxFigure Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public FillCommand(OxFigure value, Point coord , Color newColor, Color oldColor)
        {
            _value = value;
            _coord = coord;
            _newColor = newColor;
            _oldColor = oldColor;
            _value.Color = _newColor;
        }

      public void Redo()
      {
          _value.Color = _newColor;
          DrawingLayer.GetInstance.IsFill = false;
          DrawingLayer.GetInstance.DrawFigure(ref _value, _coord , false);
      }
      public void Undo()
      {
          _value.Color = _oldColor;
          DrawingLayer.GetInstance.IsFill = false;
          DrawingLayer.GetInstance.DrawFigure(ref _value, _coord, false);
      }
    }
}
