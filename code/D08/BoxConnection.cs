namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class BoxConnection
{
    public JunctionBox From {get; set;}
    public JunctionBox To {get; set;}
    public double Distance {get; set;}

    public BoxConnection(JunctionBox from, JunctionBox to)
    {
        From = from;
        To = to;
        Distance = From.GetDistanceTo(To);
    }

    public override string ToString()
    {
        return String.Join("; ", [From.ToString(), To.ToString(), Distance.ToString()]);
    }
}