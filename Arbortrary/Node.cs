namespace Wacton.Arbortrary
{
    using SixLabors.ImageSharp;
    using Wacton.Unicolour;

    internal class Node
    {
        public PointF Point { get; }
        public Unicolour Colour { get; }
        public double Bearing { get; }

        public Node(PointF point, Unicolour colour, double bearing)
        {
            Point = point;
            Colour = colour;
            Bearing = bearing;
        }
    }
}
