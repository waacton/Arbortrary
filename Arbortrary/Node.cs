﻿using SixLabors.ImageSharp;

namespace Wacton.Arbortrary
{
    internal class Node
    {
        public PointF Point { get; }
        public Colour Colour { get; }
        public double Bearing { get; }

        public Node(PointF point, Colour colour, double bearing)
        {
            Point = point;
            Colour = colour;
            Bearing = bearing;
        }
    }
}
