using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using System.Xml;
using System.Xml.Serialization;

namespace PaintApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ColorDialog colorDialog = new ColorDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();


        private Point startPoint;
        private CanvasController canvasController;
        private CustomShape currentShape = new CustomShape();
        private Rectangle selectedBorder = new Rectangle();
        private double selectedBorderPadding = 30;
        private Point selectedStartPoint = new Point();
        private Point currentShapeStartPos = new Point();
        private List<CustomShape> shapes = new List<CustomShape>();
        private Stack<CustomShape> removedShapes = new Stack<CustomShape> ();
        private Brush currentColor = Brushes.Black;

        enum CurrentState
        {
            Default,
            IsSelected,
            IsDragging,
            IsSizeable
        }

        CurrentState currentState = new CurrentState();

        public MainWindow()
        {
            InitializeComponent();
            canvasController = new CanvasController();
            currentState = CurrentState.Default;
            InitializeSelectedBorder();
            canvas.Children.Add(selectedBorder);
        }

        private void canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || currentShape.Shape == null)
                return;

            if (currentState == CurrentState.IsDragging)
            {
                Point pos = e.GetPosition(canvas);
                Canvas.SetLeft(currentShape.Shape, currentShapeStartPos.X - (selectedStartPoint.X - pos.X));
                Canvas.SetTop(currentShape.Shape, currentShapeStartPos.Y - (selectedStartPoint.Y - pos.Y));
            }

            else if (currentState == CurrentState.Default || currentState == CurrentState.IsSizeable)
            {
                var pos = e.GetPosition(canvas);

                var x = Math.Min(pos.X, currentShape.StartPoint.X);
                var y = Math.Min(pos.Y, currentShape.StartPoint.Y);

                var w = Math.Max(pos.X, currentShape.StartPoint.X) - x;
                var h = Math.Max(pos.Y, currentShape.StartPoint.Y) - y;

                currentShape.Shape.Width = w;
                currentShape.Shape.Height = h;

                Canvas.SetLeft(currentShape.Shape, x);
                Canvas.SetTop(currentShape.Shape, y);
            }
        }

        private void btn_Polygon_Click(object sender, RoutedEventArgs e)
        {
            canvasController.CurrentShapeController = canvasController.PolygonController;
            
        }

        private void btn_Rectangle_Click(object sender, RoutedEventArgs e)
        {
            canvasController.CurrentShapeController = canvasController.RectangleController;
            currentState = CurrentState.Default;
            HideButton(btn_ChangeSize);
            HideSelectedBorder(selectedBorder);
        }

        private void btn_Ellipse_Click(object sender, RoutedEventArgs e)
        {
            canvasController.CurrentShapeController = canvasController.EllipseController;
            currentState = CurrentState.Default;
            HideButton(btn_ChangeSize);
            HideSelectedBorder(selectedBorder);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RedrawShapes();
        }

        private async void btn_Serialize_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string xamlShapes = XamlServices.Save(shapes);
                using (FileStream fs = new FileStream(dialog.FileName + "/shapes.xaml", FileMode.Create))
                {
                    byte[] buffer = Encoding.Default.GetBytes(xamlShapes);
                    await fs.WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }

        private async void btn_Deserialize_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open))
                {
                    byte[] buffer = new byte[fs.Length];
                    await fs.ReadAsync(buffer, 0, buffer.Length);
                    string shapesStr = Encoding.Default.GetString(buffer);
                    List<CustomShape> DeserializedShapes = (List<CustomShape>)System.Windows.Markup.XamlReader.Parse(shapesStr);
                    shapes.Clear();
                    canvas.Children.Clear();
                    HideSelectedBorder(selectedBorder);
                    HideButton(btn_ChangeSize);
                    foreach (CustomShape customShape in DeserializedShapes)
                    {
                        shapes.Add(customShape);
                        Canvas.SetLeft(customShape.Shape, customShape.StartPoint.X);
                        Canvas.SetTop(customShape.Shape, customShape.StartPoint.Y);
                        canvas.Children.Add(customShape.Shape);
                    }
                    canvas.Children.Add(selectedBorder);
                    canvas.Children.Add(btn_ChangeSize);
                }
            }
        }

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectShape(e.GetPosition(canvas));
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (currentState == CurrentState.Default || (currentState == CurrentState.IsSelected && !ShapeContainsPoint(currentShape, e.GetPosition(canvas))))
            {
                startPoint = e.GetPosition(canvas);
                currentShape = new CustomShape(new Point(startPoint.X, startPoint.Y), canvasController.CreateShape());
                currentShape.Shape.Stroke = currentColor;
                currentShape.Shape.StrokeThickness = 2;

                Canvas.SetLeft(currentShape.Shape, startPoint.X);
                Canvas.SetTop(currentShape.Shape, startPoint.Y);

                canvas.Children.Add(currentShape.Shape);
                currentState = CurrentState.Default;
            }
            else
            {
                selectedStartPoint = e.GetPosition(canvas);
                if (ShapeContainsPoint(currentShape, selectedStartPoint))
                {
                    currentShapeStartPos.X = Canvas.GetLeft(currentShape.Shape);
                    currentShapeStartPos.Y = Canvas.GetTop(currentShape.Shape);
                    HideSelectedBorder(selectedBorder);
                    HideButton(btn_ChangeSize);
                    currentState = CurrentState.IsDragging;
                }
            }
            
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (currentState == CurrentState.IsDragging || currentState == CurrentState.IsSizeable)
            {
                SetSelectedBorder(currentShape.Shape, selectedBorder);
                SetButton(selectedBorder, btn_ChangeSize);
                currentState = CurrentState.IsSelected;
                currentShape.StartPoint = new Point(Canvas.GetLeft(currentShape.Shape), Canvas.GetTop(currentShape.Shape));
            }
            else if (currentState == CurrentState.Default)
            {
                currentShape.StartPoint = new Point(Canvas.GetLeft(currentShape.Shape), Canvas.GetTop(currentShape.Shape));
                shapes.Add(currentShape);
                SetSelectedBorder(currentShape.Shape, selectedBorder);
                SetButton(selectedBorder, btn_ChangeSize);
                currentState = CurrentState.IsSelected;
            }
        }

        private void btn_ChangeSize_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            HideSelectedBorder(selectedBorder);
            HideButton(btn_ChangeSize);
            currentState = CurrentState.IsSizeable;
        }

        private void btn_ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (currentState == CurrentState.IsSelected)
                currentShape.Shape.Stroke = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                else
                currentColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
            }
        }

        private void InitializeSelectedBorder()
        {
            selectedBorder.Stroke = Brushes.Black;
            selectedBorder.StrokeThickness = 2;
            selectedBorder.StrokeDashArray = new DoubleCollection { 2 };
        }

        private void SetSelectedBorder(Shape shape, Rectangle selectedBorder)
        {
            double left = Canvas.GetLeft(shape);
            double top = Canvas.GetTop(shape);

            selectedBorder.Width = shape.Width + selectedBorderPadding;
            selectedBorder.Height = shape.Height + selectedBorderPadding;

            double borderLeft = left - (selectedBorder.Width - shape.Width) / 2;
            double borderTop = top - (selectedBorder.Height - shape.Height) / 2;

            Canvas.SetLeft(selectedBorder, borderLeft);
            Canvas.SetTop(selectedBorder, borderTop);
            selectedBorder.Visibility = Visibility.Visible;
        }

        private void HideSelectedBorder(Rectangle selectedBorder)
        {
            selectedBorder.Visibility = Visibility.Hidden;
        }

        private void SetButton(Rectangle rect, System.Windows.Controls.Button button)
        {
            Canvas.SetLeft(btn_ChangeSize, Canvas.GetLeft(rect) + rect.Width);
            Canvas.SetTop(btn_ChangeSize, Canvas.GetTop(rect) + rect.Height);
            btn_ChangeSize.Visibility = Visibility.Visible;
        }

        private void HideButton(System.Windows.Controls.Button button)
        {
            button.Visibility = Visibility.Hidden;
        }

        private void RedrawShapes()
        {
            canvas.Children.Clear();
            foreach (CustomShape shape in shapes)
            {
                canvas.Children.Add(shape.Shape);
            }
        }

        private void SelectShape(Point pos)
        {
            foreach (CustomShape shape in shapes)
            {
                if (ShapeContainsPoint(shape, pos))
                {
                    currentShape = shape;

                    SetSelectedBorder(shape.Shape, selectedBorder);

                    SetButton(selectedBorder, btn_ChangeSize);

                    currentState = CurrentState.IsSelected;
                    break;
                }
            }
        }

        private bool ShapeContainsPoint(CustomShape shape, Point point)
        {
            double left = shape.StartPoint.X;
            double top = shape.StartPoint.Y;
            return (left < point.X && point.X < left + shape.Shape.Width) && (top < point.Y && point.Y < top + shape.Shape.Height);
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Z)
            {
                {
                    if (shapes.Count > 0)
                    {
                        var shapeToRemove = shapes[shapes.Count - 1];
                        canvas.Children.Remove(shapeToRemove.Shape);
                        shapes.Remove(shapeToRemove);
                        removedShapes.Push(shapeToRemove);
                        HideButton(btn_ChangeSize);
                        HideSelectedBorder(selectedBorder);
                    }
                }
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Y)
            {
                if (removedShapes.Count > 0)
                {
                    var shapeToAdd = removedShapes.Pop();
                    SetSelectedBorder(shapeToAdd.Shape, selectedBorder);
                    SetButton(selectedBorder, btn_ChangeSize);
                    Canvas.SetLeft(shapeToAdd.Shape, shapeToAdd.StartPoint.X);
                    Canvas.SetTop(shapeToAdd.Shape, shapeToAdd.StartPoint.Y);
                    canvas.Children.Add(shapeToAdd.Shape);
                    shapes.Add(shapeToAdd);

                }
            }
        }
    }
}
