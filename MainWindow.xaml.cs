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
using WpfApplication2.Elements;
using WpfApplication2.Layers;
using Xceed.Wpf.Toolkit;
using PhoenixControlLib;
using Microsoft.Win32;
using System.IO;
using Size = WpfApplication2.Layers.Size;

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
        private Color lastColor = ServiceLayer.GetInstance.CurrentColor;
        // for line
        private bool isPoint = false;
        private Point p2;
        private Point p1;
        // for save
        private bool isSave = false;
        private string fileName;
        // i like to move it move it!
        private bool isDragging = false;
        private OxFigure selectedVisual;
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
            if (ServiceLayer.GetInstance.CurrentTool != Tools.Line)
                isPoint = false;
            if (ServiceLayer.GetInstance.CurrentTool != Tools.Fill)
                isFil = false;
        }
        private void ClearSelection()
        {
            Point topLeftCorner = new Point(1, 1);
            if (selectedVisual.Name != OxFigure.Shape.Line)
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

        private void DrawFigure(OxFigure visual, Point topLeftCorner, bool isSelected)
        {
            System.Windows.Size curSize = new System.Windows.Size(1,1);
            if (colorChange && clicked)
                colorChange = false;
            drawingBrush = (Brush)brushConverter.ConvertFromString(visual.Color.ToString());
            if (isFil)
                drawingBrush = (Brush)brushConverter.ConvertFromString(ServiceLayer.GetInstance.CurrentColor.ToString());
            using (DrawingContext dc = visual.RenderOpen())
            {
                if (isSelected) drawingBrush = selectedDrawingBrush;
                switch (visual.Name)
                {
                    case OxFigure.Shape.Point:
                        drawingBrush = (Brush)brushConverter.ConvertFromString(Colors.Black.ToString());
                        dc.DrawRoundedRectangle(drawingBrush, drawingPen, new Rect(topLeftCorner, new System.Windows.Size(7, 7)), 5, 5);
                        break;

                    case OxFigure.Shape.Square:
                        if (clicked)
                            curSize = visual.size;
                        else 
                        if (ServiceLayer.GetInstance.CurrentSize == Size.Small)
                            curSize = new System.Windows.Size(40, 40);
                        else if (ServiceLayer.GetInstance.CurrentSize == Size.Medium)
                            curSize = new System.Windows.Size(70, 70);
                        else curSize = new System.Windows.Size(110, 110);
                        dc.DrawRectangle(drawingBrush, drawingPen, new Rect(topLeftCorner, curSize));
                        break;

                    case OxFigure.Shape.Ellispe:
                        if (clicked)
                            curSize = visual.size;
                        else
                        if (ServiceLayer.GetInstance.CurrentSize == Size.Small)
                            curSize = new System.Windows.Size(40, 40);
                        else if (ServiceLayer.GetInstance.CurrentSize == Size.Medium)
                            curSize = new System.Windows.Size(70, 70);
                        else curSize = new System.Windows.Size(110, 110);
                        dc.DrawRoundedRectangle(drawingBrush, drawingPen, new Rect(topLeftCorner, curSize), 80, 80);
                        break;

                    case OxFigure.Shape.Rectangle:
                        if (clicked)
                            curSize = visual.size;
                        else
                        if (ServiceLayer.GetInstance.CurrentSize == Size.Small)
                            curSize = new System.Windows.Size(40, 23);
                        else if (ServiceLayer.GetInstance.CurrentSize == Size.Medium)
                            curSize = new System.Windows.Size(80, 45);
                        else curSize = new System.Windows.Size(120, 70);
                        dc.DrawRectangle(drawingBrush, drawingPen, new Rect(topLeftCorner, curSize));
                        break;

                    case OxFigure.Shape.Line:
                        if (!clicked)
                        {
                            visual.Vect1 = p1 - topLeftCorner;
                            visual.Vect2 = p2 - topLeftCorner;
                        }
                        p1 = new Point(visual.Vect1.X + topLeftCorner.X, visual.Vect1.Y + topLeftCorner.Y);
                        p2 = new Point(visual.Vect2.X + topLeftCorner.X, visual.Vect2.Y + topLeftCorner.Y);
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
            var visual = new OxFigure();
            if (ServiceLayer.GetInstance.CurrentTool != Tools.Erase && ServiceLayer.GetInstance.CurrentTool != Tools.Hand && ServiceLayer.GetInstance.CurrentTool != Tools.Fill)
            {
                #region point
                if (ServiceLayer.GetInstance.CurrentTool == Tools.Point)
                {
                    visual.Name = OxFigure.Shape.Point;
                }
                #endregion
                #region sqare
                if (ServiceLayer.GetInstance.CurrentTool == Tools.Square) //
                {
                    visual.Name = OxFigure.Shape.Square;
                }
                #endregion
                #region ellipse
                if (ServiceLayer.GetInstance.CurrentTool == Tools.Ellipse)
                {
                    visual.Name = OxFigure.Shape.Ellispe;
                }
                #endregion
                #region line
                if (ServiceLayer.GetInstance.CurrentTool == Tools.Line)
                {
                    visual.Name = OxFigure.Shape.Line;
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
                if (ServiceLayer.GetInstance.CurrentTool == Tools.Rectangle)
                {
                    visual.Name = OxFigure.Shape.Rectangle;
                }
                #endregion
                if (ServiceLayer.GetInstance.CurrentTool == Tools.Line && isLin)
                {
                    DrawFigure(visual, pointClicked, false);
                    drawingSurface.AddVisual(visual);
                }
                else if (ServiceLayer.GetInstance.CurrentTool != Tools.Line)
                {
                    DrawFigure(visual, pointClicked, false);
                    drawingSurface.AddVisual(visual);
                }
            }
            else
            {
                #region erase
                if (ServiceLayer.GetInstance.CurrentTool == Tools.Erase)
                {
                    visual = drawingSurface.GetVisual(pointClicked);
                    if (visual != null) drawingSurface.DeleteVisual(visual);
                }
                #endregion
                #region hand
                if (ServiceLayer.GetInstance.CurrentTool == Tools.Hand)
                {
                    Point topLeftCorner = new Point();
                    visual = drawingSurface.GetVisual(pointClicked);
                    if (visual != null)
                    {
                        if (visual.Name == OxFigure.Shape.Line)
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
                if (ServiceLayer.GetInstance.CurrentTool == Tools.Fill)
                {
                    visual = drawingSurface.GetVisual(pointClicked);
                    if (visual != null)
                    {
                        isFil = true;
                        Point topLeftCorner = new Point(visual.ContentBounds.TopLeft.X + drawingPen.Thickness / 2,
                                                   visual.ContentBounds.TopLeft.Y + drawingPen.Thickness / 2);
                        visual.Color = ServiceLayer.GetInstance.CurrentColor;
                        clicked = true;
                        DrawFigure(visual, topLeftCorner, false);
                        clicked = false;
                    }
                    else
                    {
                        drawingBrush = (Brush)brushConverter.ConvertFromString(ServiceLayer.GetInstance.CurrentColor.ToString());
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
                if (selectedVisual.Name == OxFigure.Shape.Line)
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
            ServiceLayer.GetInstance.CurrentTool = Tools.Hand;
            isPoint = false;
            isFil = false;
        }
        private void point_click(object sender, RoutedEventArgs e)
        {
            ServiceLayer.GetInstance.CurrentTool = Tools.Point;
            cheakClearSelection();
        }
        private void line_click(object sender, RoutedEventArgs e)
        {
            ServiceLayer.GetInstance.CurrentTool = Tools.Line;
            cheakClearSelection();
        }
        private void square_click(object sender, RoutedEventArgs e)
        {
            ServiceLayer.GetInstance.CurrentTool = Tools.Square;
            cheakClearSelection();
        }
        private void rect_click(object sender, RoutedEventArgs e)
        {
            ServiceLayer.GetInstance.CurrentTool = Tools.Rectangle;
            cheakClearSelection();
        }
        private void ellipse_click(object sender, RoutedEventArgs e)
        {
            ServiceLayer.GetInstance.CurrentTool = Tools.Ellipse;
            cheakClearSelection();
        }
        private void erase_click(object sender, RoutedEventArgs e)
        {
            ServiceLayer.GetInstance.CurrentTool = Tools.Erase;
            cheakClearSelection();
        }
        private void fill_click(object sender, RoutedEventArgs e)
        {
            ServiceLayer.GetInstance.CurrentTool = Tools.Fill;
            cheakClearSelection();
        }
        private void colorPick_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            ServiceLayer.GetInstance.CurrentColor = colorPick.SelectedColor;
            colorChange = true;
            cheakClearSelection();
            lastColor = ServiceLayer.GetInstance.CurrentColor;
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
            drawingSurface.DelAll();
            ServiceLayer.GetInstance.CurrentTool = Tools.Hand;
            ServiceLayer.GetInstance.CurrentColor = Colors.Black;
            colorPick.SelectedColor = Colors.Black;
            hand_btn.IsChecked = true;
        }
        private void open_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.FileName = "";
            openDialog.Filter = "All Files (*.*)|*.*|Image files|*.svg;";
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
            drawingSurface.Measure(new System.Windows.Size((int)drawingSurface.ActualWidth, (int)drawingSurface.ActualHeight));
            drawingSurface.Arrange(new Rect(new System.Windows.Size((int)drawingSurface.ActualWidth, (int)drawingSurface.ActualHeight)));
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
            saveDialog.Filter = "Image files|*.svg";
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
                ServiceLayer.GetInstance.CurrentSize = Size.Small;
            else if (setFigSize.SelectedIndex == 1)
                ServiceLayer.GetInstance.CurrentSize = Size.Medium;
            else ServiceLayer.GetInstance.CurrentSize = Size.Large;
        }
        private void undo_click(object sender, RoutedEventArgs e)
        {
            if (drawingSurface.ChildrenCount() > 0)
            {
                drawingSurface.DelLast();
            }
        }
        private void redo_click(object sender, RoutedEventArgs e)
        {
            drawingSurface.CopyLast();
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


}
