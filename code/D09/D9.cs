namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class D9 : IDay
{
    List<(long x, long y)> red_tiles = new List<(long, long)>();
    List<List<char>> model = new List<List<char>>();

    bool do_puzzle_2 = true;

    public void Solve(){
        var counter = 0L;

        if(do_puzzle_2)
        {
            counter = SolveP2();
        }
        else
        {
            counter = SolveP1();
        }

        Console.WriteLine("Solution: " + counter);
    }

    public void Init(){
        ClearAll();
        string[] lines = Utils.ReadInput("D9.txt");

        foreach(var line in lines)
        {
            var coordinates = line.Split(',').Select(x => Convert.ToInt64(x)).ToList();
            //all lines contain a pair of numbers separated by a comma so this should be safe enough
            red_tiles.Add((coordinates[0], coordinates[1]));
        }
    }  

    private void ClearAll()
    {
        red_tiles.Clear();
        model.Clear();
    }

    private long SolveP1()
    {
        long largest = -1L;

        for(int i = 0; i < red_tiles.Count() - 1; i++)
        {
            var one = red_tiles[i];
            for(int j = i + 1; j < red_tiles.Count(); j++)
            {
                var two = red_tiles[j];
                var area = CalculateArea(one, two);
                if (area > largest)
                {
                    largest = area;
                }
            }
        }

        return largest;
    }  

    //Plan:
    // 1. iterate over all red tile combinations
    // 2. "continue;" on combinations that can be ignored because of safe(-ish) assumptions
    // 3. calculate area, if it's larger than current largest, verify it's good
    private long SolveP2()
    {
        long largest = -1L;

        // 1. iterate over all red tile combinations
        for(int i = 0; i < red_tiles.Count() - 1; i++)
        {
            var one = red_tiles[i];
            for(int j = i + 1; j < red_tiles.Count(); j++)
            {
                // 2. "continue;" on combinations that can be ignored because of safe(-ish) assumptions
                if (Math.Abs(i - j) == 1 || (i - j) % 2 != 0)
                {
                   continue;
                }

                var two = red_tiles[j];
                // 3. calculate area, if it's larger than current largest, verify it's good
                var area = CalculateArea(one, two);

                if (area > largest)
                {
                    if(!AreaGood(one, two))
                    {
                        continue;
                    }

                    largest = area;
                }
            }
        }

        return largest;
    }

    private bool AreaGood((long x, long y) one, (long x, long y) two)
    {
        for(int i = 0; i < red_tiles.Count(); i++)
        {
            var t1 = red_tiles[i];
            var t2 = GetTileAt(i + 1);
            
            //make those variables as they will be needed multiple times.
            var minx = Math.Min(one.x, two.x);
            var maxx = Math.Max(one.x, two.x);
            var miny = Math.Min(one.y, two.y);
            var maxy = Math.Max(one.y, two.y);

            if (
                (t1.x == minx && t1.y == miny)
                || (t1.x == minx && t1.y == maxy)
                || (t1.x == maxx && t1.y == miny)
                || (t1.x == maxx && t1.y == maxy)
            )
            {
                continue;
            }

            if(
                (t1.x <= minx && t2.x > minx && (t1.y > miny && t1.y < maxy))
                || (t1.y <= miny && t2.y > miny && (t1.x > minx && t1.x < maxx))
                || (t1.x >= maxx && t2.x < maxx && (t1.y > miny && t1.y < maxy))
                || (t1.y >= maxy && t2.y < maxy && (t1.x > minx && t1.x < maxx))
            )
            {
                return false;
            }       
        }
        return true;
    }

    //safe way to do wrapping neigbors
    //assumption: we don't go too far with that
    private (long x, long y) GetTileAt(int i)
    {
        if (i >= red_tiles.Count())
        {
            return red_tiles[i - red_tiles.Count()];
        }
        if (i <= -1)
        {
            return red_tiles[red_tiles.Count() + i];
        }
        return red_tiles[i];
    }

    private long CalculateArea((long x, long y) one, (long x, long y) two)
    {
        var width = Math.Abs(one.x - two.x) + 1;
        var height = Math.Abs(one.y - two.y) + 1;
        return width * height;
    }
}