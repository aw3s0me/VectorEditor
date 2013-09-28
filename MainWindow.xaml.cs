using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using PhoenixControlLib;
using Microsoft.Win32;
using System.IO;

namespace WpfApplication2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        #region peremennie

        // for replace
        private bool clicked = false;
        private bool isSel = false;
        private Point lastPoint;
        private bool colorChange = false;
        // for filling and cooking
        private bool isFil = false;
        private Color lastColor = system.getInstance.currentColor;
        // for line
        private bool isPoint = false;
        private Point p2;
        private Point p1;
        // for save
        private bool isSave = false;
        private string fileName;
        // i like to move it move it!
        private bool isDragging = false;
        private Figure selectedVisual;
        private Vector clickoffset;
        // for drawing
        private Brush drawingBrush;
        private BrushConverter brushConverter = new BrushConverter();
        private Brush selectedDrawingBrush = Brushes.LightGoldenrodYellow;
        private Pen drawingPen = new Pen(Brushes.Black, 2);
        // for Ner'Zul!
        private bool isChanged = false;
        #endregion

        private void cheakClearSelection()
        {
            if (selectedVisual != null)
                ClearSelection();
            if (system.getInstance.currentTool != tools.line)
                isPoint = false;
            if (system.getInstance.currentTool != tools.fill)
                isFil = false;
        }
        private void ClearSelection()
        {
            Point topLeftCorner = new Point(1, 1);
            if (selectedVisual.name != Figure.shape.line)
            {
                topLeftCorner = new Point(selectedVisual.ContentBounds.TopLeft.X + drawingPen.Thickness / 2,
                                                selectedVisual.ContentBounds.TopLeft.Y + drawingPen.Thickness / 2);
                bool flag = clicked;
                clicked = true;
                DrawFigure(selectedVisual, topLeftCorner, false);
                clicked = flag;
            }
            selectedVisual = null;
        }

        private void DrawFigure(Figure visual, Point topLeftCorner, bool isSelected)
        {
            Size curSize = new Size(1,1);
            if (colorChange && clicked)
                colorChange = false;
            drawingBrush = (Brush)brushConverter.ConvertFromString(visual.color.ToString());
            if (isFil)
                drawingBrush = (Brush)brushConverter.ConvertFromString(system.getInstance.currentColor.ToString());
            using (DrawingContext dc = visual.RenderOpen())
            {
                if (isSelected) drawingBrush = selectedDrawingBrush;
                switch (visual.name)
                {
                    case Figure.shape.point:
                        drawingBrush = (Brush)brushConverter.ConvertFromString(Colors.Black.ToString());
                        dc.DrawRoundedRectangle(drawingBrush, drawingPen, new Rect(topLeftCorner, new Size(7, 7)), 5, 5);
                        break;

                    case Figure.shape.square:
                        if (clicked)
                            curSize = visual.size;
                        else 
                        if (system.getInstance.currentSize == size.Small)
                            curSize = new Size(40, 40);
                        else if (system.getInstance.currentSize == size.Medium)
                            curSize = new Size(70, 70);
                        else curSize = new Size(110, 110);
                        dc.DrawRectangle(drawingBrush, drawingPen, new Rect(topLeftCorner, curSize));
                        break;

                    case Figure.shape.ellispe:
                        if (clicked)
                            curSize = visual.size;
                        else
                        if (system.getInstance.currentSize == size.Small)
                            curSize = new Size(40, 40);
                        else if (system.getInstance.currentSize == size.Medium)
                            curSize = new Size(70, 70);
                        else curSize = new Size(110, 110);
                        dc.DrawRoundedRectangle(drawingBrush, drawingPen, new Rect(topLeftCorner, curSize), 80, 80);
                        break;

                    case Figure.shape.rectangle:
                        if (clicked)
                            curSize = visual.size;
                        else
                        if (system.getInstance.currentSize == size.Small)
                            curSize = new Size(40, 23);
                        else if (system.getInstance.currentSize == size.Medium)
                            curSize = new Size(80, 45);
                        else curSize = new Size(120, 70);
                        dc.DrawRectangle(drawingBrush, drawingPen, new Rect(topLeftCorner, curSize));
                        break;

                    case Figure.shape.line:
                        if (!clicked)
                        {
                            visual.v1 = p1 - topLeftCorner;
                            visual.v2 = p2 - topLeftCorner;
                        }
                        p1 = new Point(visual.v1.X + topLeftCorner.X, visual.v1.Y + topLeftCorner.Y);
                        p2 = new Point(visual.v2.X + topLeftCorner.X, visual.v2.Y + topLeftCorner.Y);
                     //   drawingPen = new Pen(Brushes.Black, 2)
                        dc.DrawLine(drawingPen, p1, p2);
                        break;         
                }
                visual.size = curSize;
            }
            
        }

        private void drawingSurface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isChanged = true;
            bool isLin = false; // for line
            Point pointClicked = e.GetPosition(drawingSurface);
            Figure visual = new Figure();
            if (system.getInstance.currentTool != tools.erase && system.getInstance.currentTool != tools.hand && system.getInstance.currentTool != tools.fill)
            {
                #region point
                if (system.getInstance.currentTool == tools.point)
                {
                    visual.name = Figure.shape.point;
                }
                #endregion
                #region sqare
                if (system.getInstance.currentTool == tools.square) //
                {
                    visual.name = Figure.shape.square;
                }
                #endregion
                #region ellipse
                if (system.getInstance.currentTool == tools.ellipse)
                {
                    visual.name = Figure.shape.ellispe;
                }
                #endregion
                #region line
                if (system.getInstance.currentTool == tools.line)
                {
                    visual.name = Figure.shape.line;
                    if (isPoint)
                    {
                        p2 = pointClicked;
                        isLin = true;
                    }
                    else
                        p1 = pointClicked;
                    isPoint = !isPoint;
                }
                #endregion
                #region rect
                if (system.getInstance.currentTool == tools.rectangle)
                {
                    visual.name = Figure.shape.rectangle;
                }
                #endregion
                if (system.getInstance.currentTool == tools.line && isLin)
                {
                    DrawFigure(visual, pointClicked, false);
                    drawingSurface.AddVisual(visual);
                }
                else if (system.getInstance.currentTool != tools.line)
                {
                    DrawFigure(visual, pointClicked, false);
                    drawingSurface.AddVisual(visual);
                }
            }
            else
            {
                #region erase
                if (system.getInstance.currentTool == tools.erase)
                {
                    visual = drawingSurface.GetVisual(pointClicked);
                    if (visual != null) drawingSurface.DeleteVisual(visual);
                }
                #endregion
                #region hand
                if (system.getInstance.currentTool == tools.hand)
                {
                    Point topLeftCorner = new Point();
                    visual = drawingSurface.GetVisual(pointClicked);
                    if (visual != null)
                    {
                        if (visual.name == Figure.shape.line)
                        {
                            clicked = true;
                            DrawFigure(visual, topLeftCorner, true);
                            clickoffset = topLeftCorner - pointClicked;
                            isDragging = true;
                            if (selectedVisual != null && selectedVisual != visual)
                                ClearSelection();
                            selectedVisual = visual;
                            lastPoint = topLeftCorner;
                            isSel = true;
                        }
                        else
                        {
                            clicked = true;
                            topLeftCorner.X = visual.ContentBounds.TopLeft.X + drawingPen.Thickness / 2;
                            topLeftCorner.Y = visual.ContentBounds.TopLeft.Y + drawingPen.Thickness / 2;
                            DrawFigure(visual, topLeftCorner, true);
                            clickoffset = topLeftCorner - pointClicked;
                            isDragging = true;
                            if (selectedVisual != null && selectedVisual != visual)
                                ClearSelection();
                            selectedVisual = visual;
                            isSel = true;
                            lastPoint = topLeftCorner;
                        }
                    }
                    else if (isSel)
                    {
                        DrawFigure(selectedVisual, lastPoint, false);
                        isSel = false;
                    }
                }
                #endregion
                #region fill
                if (system.getInstance.currentTool == tools.fill)
                {
                    visual = drawingSurface.GetVisual(pointClicked);
                    if (visual != null)
                    {
                        isFil = true;
                        Point topLeftCorner = new Point(visual.ContentBounds.TopLeft.X + drawingPen.Thickness / 2,
                                                   visual.ContentBounds.TopLeft.Y + drawingPen.Thickness / 2);
                        visual.color = system.getInstance.currentColor;
                        clicked = true;
                        DrawFigure(visual, topLeftCorner, false);
                        clicked = false;
                    }
                    else
                    {
                        drawingBrush = (Brush)brushConverter.ConvertFromString(system.getInstance.currentColor.ToString());
                        drawingSurface.Background = drawingBrush;
                    }
                }
                #endregion
            }
        }
        private void drawingSurface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            if (isSel)
            {
                DrawFigure(selectedVisual, lastPoint, false);
                isSel = false;
            }
            clicked = false;
            isDragging = false;
        }

        private void mouse_move(object sender, MouseEventArgs e)
        {
            Point position = Mouse.GetPosition(drawingSurface);
            coords.Items[0] = Math.Round(position.X) + " : " + Math.Round(position.Y);
            if (isDragging)
            {
                Point pointDragged = new Point(1, 1);
                if (selectedVisual.name == Figure.shape.line)
                {
                    pointDragged = position;
                }
                else
                {
                    pointDragged = position + clickoffset;
                }
                DrawFigure(selectedVisual, pointDragged, true);
                lastPoint = pointDragged;
            }

        }

        private void About_click(object sender, RoutedEventArgs e)
        {
            var wind = new About();
            wind.Show();
        }
        private void Help_click(object sender, RoutedEventArgs e)
        {
            var wind = new help();
            wind.Show();
        }
        //pozition and remove
       
        #region current tool

        private void hand_click(object sender, RoutedEventArgs e)
        {
            system.getInstance.currentTool = tools.hand;
            isPoint = false;
            isFil = false;
        }
        private void point_click(object sender, RoutedEventArgs e)
        {
            system.getInstance.currentTool = tools.point;
            cheakClearSelection();
        }
        private void line_click(object sender, RoutedEventArgs e)
        {
            system.getInstance.currentTool = tools.line;
            cheakClearSelection();
        }
        private void square_click(object sender, RoutedEventArgs e)
        {
            system.getInstance.currentTool = tools.square;
            cheakClearSelection();
        }
        private void rect_click(object sender, RoutedEventArgs e)
        {
            system.getInstance.currentTool = tools.rectangle;
            cheakClearSelection();
        }
        private void ellipse_click(object sender, RoutedEventArgs e)
        {
            system.getInstance.currentTool = tools.ellipse;
            cheakClearSelection();
        }
        private void erase_click(object sender, RoutedEventArgs e)
        {
            system.getInstance.currentTool = tools.erase;
            cheakClearSelection();
        }
        private void fill_click(object sender, RoutedEventArgs e)
        {
            system.getInstance.currentTool = tools.fill;
            cheakClearSelection();
        }
        private void colorPick_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            system.getInstance.currentColor = colorPick.SelectedColor;
            colorChange = true;
            cheakClearSelection();
            lastColor = system.getInstance.currentColor;
        }

        #endregion 

        #region menu

        private void exit_click(object sender, RoutedEventArgs e)
        {
            if (isChanged)
            {
                var dialogRes = System.Windows.MessageBox.Show("Хотите сохранить изменения?","Save?",MessageBoxButton.YesNo);
                if (dialogRes == MessageBoxResult.Yes)
                    save_click(sender, e);
            }
            WpfApplication2.App.Current.Shutdown();
            
        }
        private void clear_click(object sender, RoutedEventArgs e)
        {
            isSave = false;
            drawingSurface.delAll();
            system.getInstance.currentTool = tools.hand;
            system.getInstance.currentColor = Colors.Black;
            colorPick.SelectedColor = Colors.Black;
            hand_btn.IsChecked = true;
        }
        private void open_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.FileName = "";
            openDialog.Filter = "All Files (*.*)|*.*|Image files|*.jpg; *.jpeg; *.bmp; *.gif;*.png";
            openDialog.Title = "Открыть файл";
            var dialogResult = openDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                ImageBrush ibrush = new ImageBrush();
                ibrush.ImageSource = new BitmapImage(new Uri(openDialog.FileName.ToString()));
                drawingSurface.Background = ibrush;
            }       
        }
        private void save_click(object sender, RoutedEventArgs e)
        {
            if (!isSave)
            {
                save_as_click(sender,e);
            }
             RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)drawingSurface.ActualWidth, (int)drawingSurface.ActualHeight,
                96d, 96d, PixelFormats.Pbgra32);
            // needed otherwise the image output is black
            drawingSurface.Measure(new Size((int)drawingSurface.ActualWidth, (int)drawingSurface.ActualHeight));
            drawingSurface.Arrange(new Rect(new Size((int)drawingSurface.ActualWidth, (int)drawingSurface.ActualHeight)));
            renderBitmap.Render(drawingSurface);
            //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            if (fileName != null) 
            using (FileStream file = File.Create(fileName))
                {
                    encoder.Save(file);
                }
            isChanged = false;
        }
        private void save_as_click(object sender, RoutedEventArgs e)
        {
            isSave = true;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Image files|*.png";
            var dialogResult = saveDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                fileName = saveDialog.FileName;
                save_click(sender, e);
            }
            else
            {
                isSave = false;
            }
            isChanged = false;
        }
        private void setFigSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (setFigSize.SelectedIndex == (int)0)
                system.getInstance.currentSize = size.Small;
            else if (setFigSize.SelectedIndex == 1)
                system.getInstance.currentSize = size.Medium;
            else system.getInstance.currentSize = size.Large;
        }
        private void undo_click(object sender, RoutedEventArgs e)
        {
            if (drawingSurface.ChildrenCount() > 0)
            {
                drawingSurface.delLast();
            }
        }
        private void redo_click(object sender, RoutedEventArgs e)
        {
            drawingSurface.copyLast();
        }

        #endregion

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Open_Checked(object sender, RoutedEventArgs e)
        {

        }


        private void New_click(object sender, RoutedEventArgs e)
        {

        }

        private void triangle_click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class DrawingCanvas : Canvas
    {
        private List<Visual> visuals = new List<Visual>();
        protected override int VisualChildrenCount
        {
            get
            {
                return visuals.Count;
            }
        }
        public int ChildrenCount()
        {
                return visuals.Count;
        }
        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }
        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);
            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }
        public void DeleteVisual(Visual visual)
        {
            visuals.Remove(visual);
            base.RemoveLogicalChild(visual);
            base.RemoveVisualChild(visual);
        }
        public Figure GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as Figure;
        }
        public void delAll()
        {
            int t = VisualChildrenCount;
            for (int i = t-1; i > -1; i--)
            {
                DeleteVisual(visuals[i]);
            }
            
            BrushConverter brushConverter = new BrushConverter();
            Brush drawingBrush = (Brush)brushConverter.ConvertFromString(Colors.White.ToString());
            this.Background = drawingBrush;
        }
        public void delLast()
        {
            DeleteVisual(GetVisualChild(VisualChildrenCount - 1));
        }
        public void copyLast()
        {
            //AddVisual(GetVisualChild(VisualChildrenCount - 1));
        }
    }

    public class Figure : DrawingVisual
    {
        public enum shape { point, ellispe, square, rectangle, line };
        public shape name;
        public Color color;
        public Size size;
        public Vector v1;
        public Vector v2;

        public Figure() : base()
        {
            color = system.getInstance.currentColor;
            size = new Size(0, 0);
        }
        public Figure(shape name)
            : base()
        {
            color = system.getInstance.currentColor;
            this.name = name;
        }
    }
}
