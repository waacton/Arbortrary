using SixLabors.ImageSharp;

namespace Wacton.Arbortrary
{
    internal class Node
    {
        public PointF Point { get; }
        public Hsba Color { get; }
        public double Bearing { get; }

        public Node(PointF point, Hsba color, double bearing)
        {
            Point = point;
            Color = color;
            Bearing = bearing;
        }
    }
}
