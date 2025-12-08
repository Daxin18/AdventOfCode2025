namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class JunctionBox
{
    public int X {get; set;}
    public int Y {get; set;}
    public int Z {get; set;}

    public JunctionBox(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    //Euclidean distance, as per the link in the puzzle
    public double GetDistanceTo(JunctionBox other)
    {
        var x = Math.Pow((other.X - this.X),2);
        var y = Math.Pow((other.Y - this.Y),2);
        var z = Math.Pow((other.Z - this.Z),2);
        var distance = Math.Sqrt(x + y + z);
        return distance;
    }

    public bool Is(JunctionBox other)
    {
        return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
    }

    public override bool Equals(object? obj)
    {
        if(obj is JunctionBox other)
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        return false;
    }

    public override string ToString()
    {
        return String.Join(",", [X.ToString(), Y.ToString(), Z.ToString()]);
    }
}