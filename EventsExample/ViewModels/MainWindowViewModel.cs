
using EventsExample.Models;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace EventsExample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<IShape> shapes;

        public MainWindowViewModel()
        {
            Shapes = new ObservableCollection<IShape>();
            Shapes.Add(new RectangleWithConnectors
            {
                Height = 100,
                Width = 100,
                Name = "First",
                StartPoint = new Avalonia.Point(100, 100),
            });

            Shapes.Add(new RectangleWithConnectors
            {
                Height = 100,
                Width = 100,
                Name = "Second",
                StartPoint = new Avalonia.Point(500, 100),
            });

            Shapes.Add(new RectangleWithConnectors
            {
                Height = 100,
                Width = 100,
                Name = "First",
                StartPoint = new Avalonia.Point(100, 500),
            });

            Shapes.Add(new RectangleWithConnectors
            {
                Height = 100,
                Width = 100,
                Name = "Second",
                StartPoint = new Avalonia.Point(500, 500),
            });
        }
        public ObservableCollection<IShape> Shapes
        {
            get => shapes;
            set => this.RaiseAndSetIfChanged(ref shapes, value);
        }
    }
}