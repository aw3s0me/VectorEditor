using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using WpfApplication2.Elements;
using WpfApplication2.Interfaces;
using WpfApplication2.Layers;

namespace WpfApplication2.Helpers
{
    internal class MoveCommand : ICommand
    {
        private readonly Point _newCoord;
        private readonly Point _oldCoord;
        private Visual _value;
        public CanvasLayer CurCanv;

        public Visual Value
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

        public MoveCommand(Visual value, CanvasLayer curCanv, Point newCoord, Point oldCoord)
        {
            _value = value;
            CurCanv = curCanv;
            _newCoord = newCoord;
            _oldCoord = oldCoord;
        }

        public void Redo()
        {
            if (CurCanv != null && _value != null)
            {
                //Point topLeftCorner = new Point();
                //_value = CurCanv.GetVisual(_newCoord);
                var fig = _value as OxFigure;
                if (fig != null)
                {
                    DrawingLayer.GetInstance.DrawFigure(ref fig, _newCoord, false);
                    /*if (fig.Name == OxFigure.Shape.Line)
                  {
                      //DrawingLayer.GetInstance.IsClicked = true;
                      DrawingLayer.GetInstance.DrawFigure(ref fig, topLeftCorner, true);
                      //DrawingLayer.GetInstance.ClickOffset = topLeftCorner - _newCoord;
                      //DrawingLayer.GetInstance.IsDragging = true;
                      //if (DrawingLayer.GetInstance.SelectedVisual != null && DrawingLayer.GetInstance.SelectedVisual != fig)
                        //  DrawingLayer.GetInstance.ClearSelection();
                      //DrawingLayer.GetInstance.SelectedVisual = fig;
                      //DrawingLayer.GetInstance.LastPoint = topLeftCorner;
                      //DrawingLayer.GetInstance.IsSel = true;
                  }
                  else
                  {
                      //DrawingLayer.GetInstance.IsClicked = true;
                      topLeftCorner.X = fig.ContentBounds.TopLeft.X + DrawingLayer.GetInstance.DrawingPen.Thickness / 2;
                      topLeftCorner.Y = fig.ContentBounds.TopLeft.Y + DrawingLayer.GetInstance.DrawingPen.Thickness / 2;
                      DrawingLayer.GetInstance.DrawFigure(ref fig, topLeftCorner, true);
                  } */
                }
            }
        }

        public void Undo()
        {
            if (CurCanv != null && _value != null)
            {
                var fig = _value as OxFigure;
                if (fig != null)
                {
                    DrawingLayer.GetInstance.DrawFigure(ref fig, _oldCoord, false);
                }
            }
        }
    }
}
