using Relnet.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Relnet.Visualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RelnetWorld world;
        Dictionary<State, Brush> stateBrushes = new Dictionary<State, Brush>();
        Dictionary<Node, Point> nodePoints = new Dictionary<Node, Point>();
        List<Line> lines = new List<Line>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStep_Click(object sender, RoutedEventArgs e)
        {
            world.Step();
            DrawLines();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        private void BuildWorld()
        {
            const int NUM_NODES = 6;
            const int NUM_STATES = 2;
            var nodes = new List<Node>();

            var radius = (drawingCanvas.ActualHeight - 50) / 2;
            var cx = drawingCanvas.ActualWidth / 2;
            var cy = drawingCanvas.ActualHeight / 2;
            var angOfst = 2 * Math.PI / NUM_NODES;

            for (int i = 1; i <= NUM_NODES; i++)
            {
                var node = new Node { Name = $"N{i}" };
                nodes.Add(node);
                var coords = new Point(Math.Cos(angOfst * i) * radius + cx, Math.Sin(angOfst * i) * radius + cy);
                nodePoints.Add(node, coords);
                drawingCanvas.Children.Add(new Ellipse { Width = 20, Height = 20, Margin = new Thickness(coords.X - 10, coords.Y - 10, 0, 0), Fill = Brushes.Black });
            }
            var states = new List<State>();
            for (int i = 1; i <= NUM_STATES; i++)
            {
                states.Add(new State { Name = $"S{i}" });
            }
            stateBrushes.Add(states[0], Brushes.Orange);
            stateBrushes.Add(states[1], Brushes.Purple);
            var triConfigs = new List<TriangleConfiguration>();
            var tempConfig = new TriangleConfiguration(states);
            tempConfig.StateCounts[states[0]] = 3;
            tempConfig.StateWeights[states[0]][states[1]] = 100;
            triConfigs.Add(tempConfig);
            tempConfig = new TriangleConfiguration(states);
            tempConfig.StateCounts[states[1]] = 3;
            tempConfig.StateWeights[states[1]][states[0]] = 100;
            triConfigs.Add(tempConfig);
            tempConfig = new TriangleConfiguration(states);
            tempConfig.StateCounts[states[0]] = 2;
            tempConfig.StateCounts[states[1]] = 1;
            tempConfig.StateWeights[states[0]][states[1]] = 100;
            tempConfig.StateWeights[states[1]][states[0]] = 50;
            triConfigs.Add(tempConfig);
            tempConfig = new TriangleConfiguration(states);
            tempConfig.StateCounts[states[1]] = 2;
            tempConfig.StateCounts[states[0]] = 1;
            tempConfig.StateWeights[states[1]][states[0]] = 100;
            tempConfig.StateWeights[states[0]][states[1]] = 50;
            triConfigs.Add(tempConfig);
            world = new RelnetWorld(nodes, states, triConfigs);
        }

        private void canvas_loaded(object sender, RoutedEventArgs e)
        {
            BuildWorld();
            DrawLines();
            var timer = new Timer(x =>
            {
                world.Step();
                DrawLines();
            }, null, 0, 333);
        }

        private void DrawLines()
        {
            foreach (var line in lines)
                drawingCanvas.Children.Remove(line);
            lines.Clear();
            foreach (var rel in world.Relationships)
            {
                var n1p = nodePoints[rel.NodeOne];
                var n2p = nodePoints[rel.NodeTwo];
                var line = new Line { Stroke = stateBrushes[rel.State], X1 = n1p.X, X2 = n2p.X, Y1 = n1p.Y, Y2 = n2p.Y, StrokeThickness = 2 };
                lines.Add(line);
                drawingCanvas.Children.Add(line);
            }
        }
    }
}
