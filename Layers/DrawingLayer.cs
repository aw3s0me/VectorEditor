using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using WpfApplication2.Elements;


namespace WpfApplication2.Layers
{
    public class DrawingLayer
    {
        #region peremennie
        // for replace
        public bool IsClicked { get; set; }
        public bool IsSel { get; set; }
        public Point LastPoint { get; set; }
        public bool ColorChange { get; set; }
        public bool IsFill { get; set; }
        public Color LastColor { get; set; }
        public bool IsPoint { get; set; }
        public Point Point2 { get; set; }
        public Point Point1 { get; set; }
        public bool IsSave { get; set; }
        public string FileName { get; set; }
        public OxFigure SelectedVisual { get; set; }
        public Vector ClickOffset { get; set; }
        public Brush DrawingBrush { get; set; }
        public BrushConverter BrushConverter { get; set; }
        public Brush SelectedDrawingBrush { get; set; }
        public Pen DrawingPen { get; set; }
        public bool IsChanged { get; set; }
        public bool IsDragging { get; set; }

        private static readonly DrawingLayer Instance = new DrawingLayer();


        DrawingLayer()
        {
            IsClicked = false;
            IsSel = false;
            ColorChange = false;
            IsFill = false;
            LastColor = SelectionLayer.GetInstance.CurrentColor;
            IsPoint = false;
            IsSave = false;
            FileName = String.Empty;
            IsDragging = false;
            BrushConverter   = new BrushConverter();
            SelectedDrawingBrush = Brushes.LightGoldenrodYellow;
            DrawingPen = new Pen(Brushes.Black, 2);
            IsChanged = false;
        }

        public static DrawingLayer GetInstance
        {
            get
            {
                return Instance;
            }
        }

        #endregion

        public void CheakClearSelection()
        {
            if (SelectedVisual != null)
                ClearSelection();
            if (SelectionLayer.GetInstance.CurrentTool != Tools.Line)
                IsPoint = false;
            if (SelectionLayer.GetInstance.CurrentTool != Tools.Fill)
                IsFill = false;
        }

        public void ClearSelection()
        {
            if (SelectedVisual.Name != OxFigure.Shape.Line)
            {
                var topLeftCorner = new Point(SelectedVisual.ContentBounds.TopLeft.X + DrawingPen.Thickness/2,
                    SelectedVisual.ContentBounds.TopLeft.Y + DrawingPen.Thickness/2);
                var flag = IsClicked;
                IsClicked = true;
                DrawFigure(SelectedVisual, topLeftCorner, false);
                IsClicked = flag;
            }
            SelectedVisual = null;
        }

        public void DrawFigure(OxFigure visual, Point topLeftCorner, bool isSelected)
        {
            var curSize = new System.Windows.Size(1, 1);
            Debug.WriteLine(topLeftCorner.ToString());
            if (ColorChange && IsClicked)
                ColorChange = false;

            DrawingBrush = (Brush) BrushConverter.ConvertFromString(visual.Color.ToString());

            if (IsFill)
                DrawingBrush = (Brush) BrushConverter.ConvertFromString(SelectionLayer.GetInstance.CurrentColor.ToString());

            using (var dc = visual.RenderOpen())
            {
                if (isSelected) DrawingBrush = SelectedDrawingBrush;
                switch (visual.Name)
                {
                    case OxFigure.Shape.Point:
                        DrawingBrush = (Brush) BrushConverter.ConvertFromString(Colors.Black.ToString());
                        dc.DrawRoundedRectangle(DrawingBrush, DrawingPen,
                            new Rect(topLeftCorner, new System.Windows.Size(7, 7)), 5, 5);
                        break;

                    case OxFigure.Shape.Square:
                        if (IsClicked)
                            curSize = visual.size;
                        else if (SelectionLayer.GetInstance.CurrentSize == Size.Small)
                            curSize = new System.Windows.Size(40, 40);
                        else if (SelectionLayer.GetInstance.CurrentSize == Size.Medium)
                            curSize = new System.Windows.Size(70, 70);
                        else curSize = new System.Windows.Size(110, 110);
                        dc.DrawRectangle(DrawingBrush, DrawingPen, new Rect(topLeftCorner, curSize));
                        break;

                    case OxFigure.Shape.Ellispe:
                        if (IsClicked)
                            curSize = visual.size;
                        else switch (SelectionLayer.GetInstance.CurrentSize)
                        {
                            case Size.Small:
                                curSize = new System.Windows.Size(40, 40);
                                break;
                            case Size.Medium:
                                curSize = new System.Windows.Size(70, 70);
                                break;
                            case Size.Large:
                                curSize = new System.Windows.Size(110, 110);
                                break;
                            default:
                                curSize = new System.Windows.Size(110, 110);
                                break;
                        }
                        dc.DrawRoundedRectangle(DrawingBrush, DrawingPen, new Rect(topLeftCorner, curSize), 80, 80);
                        break;

                    case OxFigure.Shape.Rectangle:
                        if (IsClicked)
                            curSize = visual.size;
                        else switch (SelectionLayer.GetInstance.CurrentSize)
                        {
                            case Size.Small:
                                curSize = new System.Windows.Size(40, 23);
                                break;
                            case Size.Medium:
                                curSize = new System.Windows.Size(80, 45);
                                break;
                            case Size.Large:
                                curSize = new System.Windows.Size(120, 70);
                                break;
                            default:
                                curSize = new System.Windows.Size(80, 45);
                                break;
                        }
                        dc.DrawRectangle(DrawingBrush, DrawingPen, new Rect(topLeftCorner, curSize));
                        break;

                    case OxFigure.Shape.Line:
                        if (!IsClicked)
                        {
                            visual.Vect1 = Point1 - topLeftCorner;
                            visual.Vect2 = Point2 - topLeftCorner;
                        }
                        Point1 = new Point(visual.Vect1.X + topLeftCorner.X, visual.Vect1.Y + topLeftCorner.Y);
                        Point2 = new Point(visual.Vect2.X + topLeftCorner.X, visual.Vect2.Y + topLeftCorner.Y);
                        //   drawingPen = new Pen(Brushes.Black, 2)
                        dc.DrawLine(DrawingPen, Point1, Point2);
                        break;
                }
                visual.size = curSize;
            }

        }
    }
}