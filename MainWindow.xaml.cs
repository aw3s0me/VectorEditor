using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApplication2.Elements;
using WpfApplication2.Helpers;
using WpfApplication2.Layers;
using Microsoft.Win32;
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

        private void drawingSurface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DrawingLayer.GetInstance.IsChanged = true;
            bool isLin = false; // for line
            Point pointClicked = e.GetPosition(drawingSurface);
            OxFigure visual = null;
            if (SelectionLayer.GetInstance.CurrentTool != Tools.Erase &&
                SelectionLayer.GetInstance.CurrentTool != Tools.Hand && SelectionLayer.GetInstance.CurrentTool != Tools.Fill)
            {
                #region point

                if (SelectionLayer.GetInstance.CurrentTool == Tools.Point)
                {
                    visual = new OxPoint {Name = OxFigure.Shape.Point};
                }

                #endregion

                #region square

                if (SelectionLayer.GetInstance.CurrentTool == Tools.Square) //
                {
                    visual = new OxRectangle {Name = OxFigure.Shape.Square};
                }

                #endregion

                #region ellipse

                if (SelectionLayer.GetInstance.CurrentTool == Tools.Ellipse)
                {
                    visual = new OxCircle {Name = OxFigure.Shape.Ellispe};
                }

                #endregion

                #region line

                if (SelectionLayer.GetInstance.CurrentTool == Tools.Line)
                {
                    visual = new OxLine {Name = OxFigure.Shape.Line};
                    if (DrawingLayer.GetInstance.IsPoint)
                    {
                        DrawingLayer.GetInstance.Point2 = pointClicked;
                        isLin = true;
                    }
                    else
                        DrawingLayer.GetInstance.Point1 = pointClicked;
                    DrawingLayer.GetInstance.IsPoint = !DrawingLayer.GetInstance.IsPoint;
                }

                #endregion

                #region rect

                if (SelectionLayer.GetInstance.CurrentTool == Tools.Rectangle)
                {
                    visual = new OxRectangle {Name = OxFigure.Shape.Rectangle};
                }

                #endregion

                if (visual == null)
                {
                    return;
                }

                if (SelectionLayer.GetInstance.CurrentTool == Tools.Line && isLin)
                {
                    DrawingLayer.GetInstance.DrawFigure(ref visual, pointClicked, false);
                    drawingSurface.AddVisual(visual);
                    UndoRedoLayer.GetInstance.Add(new AddElementCommand(visual, this.drawingSurface));
                }
                else if (SelectionLayer.GetInstance.CurrentTool != Tools.Line)
                {
                    DrawingLayer.GetInstance.DrawFigure(ref visual, pointClicked, false);
                    drawingSurface.AddVisual(visual);
                    //здесь добавление
                    UndoRedoLayer.GetInstance.Add(new AddElementCommand(visual, this.drawingSurface));
                }
            }
            else
            {
                #region erase

                if (SelectionLayer.GetInstance.CurrentTool == Tools.Erase)
                {
                    visual = drawingSurface.GetVisual(pointClicked);
                    if (visual != null) drawingSurface.DeleteVisual(visual);
                }

                #endregion

                #region hand

                if (SelectionLayer.GetInstance.CurrentTool == Tools.Hand)
                {
                    Point topLeftCorner = new Point();
                    visual = drawingSurface.GetVisual(pointClicked);
                    if (visual != null)
                    {
                        if (visual.Name == OxFigure.Shape.Line)
                        {
                            DrawingLayer.GetInstance.IsClicked = true;
                            DrawingLayer.GetInstance.DrawFigure(ref visual, topLeftCorner, true);
                            DrawingLayer.GetInstance.ClickOffset = topLeftCorner - pointClicked;
                            DrawingLayer.GetInstance.IsDragging = true;
                            if (DrawingLayer.GetInstance.SelectedVisual != null && DrawingLayer.GetInstance.SelectedVisual != visual)
                                DrawingLayer.GetInstance.ClearSelection();
                            DrawingLayer.GetInstance.SelectedVisual = visual;
                            DrawingLayer.GetInstance.LastPoint = topLeftCorner;
                            DrawingLayer.GetInstance.IsSel = true;
                        }
                        else
                        {
                            DrawingLayer.GetInstance.IsClicked = true;
                            topLeftCorner.X = visual.ContentBounds.TopLeft.X + DrawingLayer.GetInstance.DrawingPen.Thickness / 2;
                            topLeftCorner.Y = visual.ContentBounds.TopLeft.Y + DrawingLayer.GetInstance.DrawingPen.Thickness / 2;
                            DrawingLayer.GetInstance.DrawFigure(ref visual, topLeftCorner, true);
                            DrawingLayer.GetInstance.ClickOffset = topLeftCorner - pointClicked;
                            DrawingLayer.GetInstance.IsDragging = true;
                            if (DrawingLayer.GetInstance.SelectedVisual != null && DrawingLayer.GetInstance.SelectedVisual != visual)
                                DrawingLayer.GetInstance.ClearSelection();
                            DrawingLayer.GetInstance.SelectedVisual = visual;
                            DrawingLayer.GetInstance.IsSel = true;
                            DrawingLayer.GetInstance.LastPoint = topLeftCorner;
                        }
                    }
                    else if (DrawingLayer.GetInstance.IsSel)
                    {
                        OxFigure selectedVisual = DrawingLayer.GetInstance.SelectedVisual;
                        DrawingLayer.GetInstance.DrawFigure(ref selectedVisual, DrawingLayer.GetInstance.LastPoint, false);
                        DrawingLayer.GetInstance.IsSel = false;
                    }
                }

                #endregion

                #region fill

                if (SelectionLayer.GetInstance.CurrentTool == Tools.Fill)
                {
                    visual = drawingSurface.GetVisual(pointClicked);
                    if (visual != null)
                    {
                        DrawingLayer.GetInstance.IsFill = true;
                        Point topLeftCorner = new Point(visual.ContentBounds.TopLeft.X + DrawingLayer.GetInstance.DrawingPen.Thickness / 2,
                            visual.ContentBounds.TopLeft.Y + DrawingLayer.GetInstance.DrawingPen.Thickness / 2);
                        visual.Color = SelectionLayer.GetInstance.CurrentColor;
                        DrawingLayer.GetInstance.IsClicked = true;
                        DrawingLayer.GetInstance.DrawFigure(ref visual, topLeftCorner, false);
                        DrawingLayer.GetInstance.IsClicked = false;
                    }
                    else
                    {
                        DrawingLayer.GetInstance.DrawingBrush =
                            (Brush)DrawingLayer.GetInstance.BrushConverter.ConvertFromString(SelectionLayer.GetInstance.CurrentColor.ToString());
                        drawingSurface.Background = DrawingLayer.GetInstance.DrawingBrush;
                    }
                }

                #endregion
            }
        }

        private void drawingSurface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (DrawingLayer.GetInstance.IsSel)
            {
                OxFigure selectedVisual = DrawingLayer.GetInstance.SelectedVisual;
                DrawingLayer.GetInstance.DrawFigure(ref selectedVisual, DrawingLayer.GetInstance.LastPoint, false);
                DrawingLayer.GetInstance.IsSel = false;
            }
            DrawingLayer.GetInstance.IsClicked = false;
            DrawingLayer.GetInstance.IsDragging = false;
        }

        private void mouse_move(object sender, MouseEventArgs e)
        {
            Point position = Mouse.GetPosition(drawingSurface);
            coords.Items[0] = Math.Round(position.X) + " : " + Math.Round(position.Y);
            if (DrawingLayer.GetInstance.IsDragging)
            {
                Point pointDragged = new Point(1, 1);
                if (DrawingLayer.GetInstance.SelectedVisual.Name == OxFigure.Shape.Line)
                {
                    pointDragged = position;
                }
                else
                {
                    pointDragged = position + DrawingLayer.GetInstance.ClickOffset;
                }
                OxFigure selectedVisual = DrawingLayer.GetInstance.SelectedVisual;
                DrawingLayer.GetInstance.DrawFigure(ref selectedVisual, pointDragged, true);
                DrawingLayer.GetInstance.LastPoint = pointDragged;
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
            SelectionLayer.GetInstance.CurrentTool = Tools.Hand;
            DrawingLayer.GetInstance.IsPoint = false;
            DrawingLayer.GetInstance.IsFill = false;
        }
        private void point_click(object sender, RoutedEventArgs e)
        {
            SelectionLayer.GetInstance.CurrentTool = Tools.Point;
            DrawingLayer.GetInstance.CheakClearSelection();
        }
        private void line_click(object sender, RoutedEventArgs e)
        {
            SelectionLayer.GetInstance.CurrentTool = Tools.Line;
            DrawingLayer.GetInstance.CheakClearSelection();
        }
        private void square_click(object sender, RoutedEventArgs e)
        {
            SelectionLayer.GetInstance.CurrentTool = Tools.Square;
            DrawingLayer.GetInstance.CheakClearSelection();
        }
        private void rect_click(object sender, RoutedEventArgs e)
        {
            SelectionLayer.GetInstance.CurrentTool = Tools.Rectangle;
            DrawingLayer.GetInstance.CheakClearSelection();
        }
        private void ellipse_click(object sender, RoutedEventArgs e)
        {
            SelectionLayer.GetInstance.CurrentTool = Tools.Ellipse;
            DrawingLayer.GetInstance.CheakClearSelection();
        }
        private void erase_click(object sender, RoutedEventArgs e)
        {
            SelectionLayer.GetInstance.CurrentTool = Tools.Erase;
            DrawingLayer.GetInstance.CheakClearSelection();
        }
        private void fill_click(object sender, RoutedEventArgs e)
        {
            SelectionLayer.GetInstance.CurrentTool = Tools.Fill;
            DrawingLayer.GetInstance.CheakClearSelection();
        }
        private void colorPick_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            SelectionLayer.GetInstance.CurrentColor = colorPick.SelectedColor;
            DrawingLayer.GetInstance.ColorChange = true;
            DrawingLayer.GetInstance.CheakClearSelection();
            DrawingLayer.GetInstance.LastColor = SelectionLayer.GetInstance.CurrentColor;
        }

        #endregion 

        #region menu

        private void exit_click(object sender, RoutedEventArgs e)
        {
            if (DrawingLayer.GetInstance.IsChanged)
            {
                var dialogRes = System.Windows.MessageBox.Show("Хотите сохранить изменения?","Save?",MessageBoxButton.YesNo);
                if (dialogRes == MessageBoxResult.Yes)
                    save_click(sender, e);
            }
            WpfApplication2.App.Current.Shutdown();
            
        }
        private void clear_click(object sender, RoutedEventArgs e)
        {
            DrawingLayer.GetInstance.IsSave = false;
            drawingSurface.DelAll();
            SelectionLayer.GetInstance.CurrentTool = Tools.Hand;
            SelectionLayer.GetInstance.CurrentColor = Colors.Black;
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
                /*ImageBrush ibrush = new ImageBrush();
                ibrush.ImageSource = new BitmapImage(new Uri(openDialog.FileName.ToString()));
                drawingSurface.Background = ibrush; */
                SerializationLayer.DeserializeFromXML(openDialog.FileName, ref this.drawingSurface);
            }       
        }
        private void save_click(object sender, RoutedEventArgs e)
        {
            if (!DrawingLayer.GetInstance.IsSave)
            {
                save_as_click(sender,e);
            }
            /*RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)drawingSurface.ActualWidth, (int)drawingSurface.ActualHeight,
                96d, 96d, PixelFormats.Pbgra32);
            // needed otherwise the image output is black
            drawingSurface.Measure(new System.Windows.Size((int)drawingSurface.ActualWidth, (int)drawingSurface.ActualHeight));
            drawingSurface.Arrange(new Rect(new System.Windows.Size((int)drawingSurface.ActualWidth, (int)drawingSurface.ActualHeight)));
            renderBitmap.Render(drawingSurface);
            //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap)); */
            if (DrawingLayer.GetInstance.FileName != null)
                SerializationLayer.SerializeToXML(drawingSurface, DrawingLayer.GetInstance.FileName);
                    //encoder.Save(file);

            DrawingLayer.GetInstance.IsChanged = false;
        }
        private void save_as_click(object sender, RoutedEventArgs e)
        {
            DrawingLayer.GetInstance.IsSave = true;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Image files|*.svg";
            var dialogResult = saveDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                DrawingLayer.GetInstance.FileName = saveDialog.FileName;
                save_click(sender, e);
            }
            else
            {
                DrawingLayer.GetInstance.IsSave = false;
            }
            DrawingLayer.GetInstance.IsChanged = false;
        }
        private void setFigSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (setFigSize.SelectedIndex == (int)0)
                SelectionLayer.GetInstance.CurrentSize = Size.Small;
            else if (setFigSize.SelectedIndex == 1)
                SelectionLayer.GetInstance.CurrentSize = Size.Medium;
            else SelectionLayer.GetInstance.CurrentSize = Size.Large;
        }
        private void undo_click(object sender, RoutedEventArgs e)
        {
            UndoRedoLayer.GetInstance.Undo();
            /*if (drawingSurface.ChildrenCount() > 0)
            {
                drawingSurface.DelLast();
            } */
        }
        private void redo_click(object sender, RoutedEventArgs e)
        {
            UndoRedoLayer.GetInstance.Redo();
            //drawingSurface.CopyLast();
        }

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

        #endregion
    }


}
