using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using EventsExample.Models;
using EventsExample.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace EventsExample.Views
{
    public partial class MainWindow : Window
    {
        private Point pointPointerPressed;
        private Point pointerPositionIntoShape;
        private RectangleWithConnectors? secondRectangle;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs pointerPressedEventArgs)
        {
            if (pointerPressedEventArgs.Source is Control control)
            {
                if (control.DataContext is RectangleWithConnectors rectangle)
                {
                    pointPointerPressed = pointerPressedEventArgs
                        .GetPosition(
                        this.GetVisualDescendants()
                        .OfType<Canvas>()
                        .FirstOrDefault(canvas => string.IsNullOrEmpty(canvas.Name) == false &&
                        canvas.Name.Equals("highLevelCanvas")));

                    if (pointerPressedEventArgs.Source is not Ellipse)
                    {
                        pointerPositionIntoShape = pointerPressedEventArgs.GetPosition(control);
                        this.PointerMoved += PointerMoveDragShape;
                        this.PointerReleased += PointerPressedReleasedDragShape;
                    }
                    else
                    {
                        if (this.DataContext is MainWindowViewModel viewModel)
                        {
                            viewModel.Shapes.Add(new Connector
                            {
                                StartPoint = pointPointerPressed,
                                EndPoint = pointPointerPressed,
                                Name = "Connector",
                                FirstRectangle = rectangle,
                            });

                            
                            this.PointerMoved += PointerMoveDrawLine;
                            this.PointerReleased += PointerPressedReleasedDrawLine;
                        }
                    }
                }
            }
        }

        private void PointerMoveDragShape(object? sender, PointerEventArgs pointerEventArgs)
        {
            if (pointerEventArgs.Source is Control control)
            {
                if (control.DataContext is RectangleWithConnectors rectangle)
                {
                    Point currentPointerPosition = pointerEventArgs
                    .GetPosition(
                    this.GetVisualDescendants()
                    .OfType<Canvas>()
                    .FirstOrDefault());

                    rectangle.StartPoint = new Point(
                        currentPointerPosition.X - pointerPositionIntoShape.X,
                        currentPointerPosition.Y - pointerPositionIntoShape.Y);
                }
            }
        }

        private void PointerPressedReleasedDragShape(object? sender,
            PointerReleasedEventArgs pointerReleasedEventArgs)
        {
            this.PointerMoved -= PointerMoveDragShape;
            this.PointerReleased -= PointerPressedReleasedDragShape;
        }

        private void PointerMoveDrawLine(object? sender, PointerEventArgs pointerEventArgs)
        {
            if (this.DataContext is MainWindowViewModel viewModel)
            {
                Debug.WriteLine(sender);
                Connector connector = viewModel.Shapes[viewModel.Shapes.Count - 1] as Connector;
                Point currentPointerPosition = pointerEventArgs
                    .GetPosition(
                    this.GetVisualDescendants()
                    .OfType<Canvas>()
                    .FirstOrDefault());

                connector.EndPoint = new Point(
                        currentPointerPosition.X - 1,
                        currentPointerPosition.Y - 1);
            }
        }

        private void PointerPressedReleasedDrawLine(object? sender,
            PointerReleasedEventArgs pointerReleasedEventArgs)
        {
            this.PointerMoved -= PointerMoveDrawLine;
            this.PointerReleased -= PointerPressedReleasedDrawLine;

            var canvas = this.GetVisualDescendants()
                        .OfType<Canvas>()
                        .FirstOrDefault(canvas => string.IsNullOrEmpty(canvas.Name) == false &&
                        canvas.Name.Equals("highLevelCanvas"));

            var coords = pointerReleasedEventArgs.GetPosition(canvas);

            var element = canvas.InputHitTest(coords);
            MainWindowViewModel viewModel = this.DataContext as MainWindowViewModel;
            
            if (element is Ellipse ellipse)
            {
                if(ellipse.DataContext is RectangleWithConnectors rectangle)
                {
                    Connector connector = viewModel.Shapes[viewModel.Shapes.Count - 1] as Connector;
                    connector.SecondRectangle = rectangle;
                    return;
                }
            }

            viewModel.Shapes.RemoveAt(viewModel.Shapes.Count - 1);
        }
    }
}