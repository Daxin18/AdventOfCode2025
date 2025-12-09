namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class D9 : IDay
{
    List<(long x, long y)> red_tiles = new List<(long, long)>();

    bool do_puzzle_2 = false;

    public void Solve(){
        var counter = 0L;

        if(do_puzzle_2)
        {

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
    }

    private long SolveP1()
    {
        long largest = -1L;
        //var counter = 0;

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
                    //Console.WriteLine(one.x + "," + one.y + " : " + two.x + "," + two.y);
                }
                //counter++;
            }
        }

        //Console.WriteLine(counter);
        return largest;
    }  

    private long CalculateArea((long x, long y) one, (long x, long y) two)
    {
        var width = Math.Abs(one.x - two.x) + 1;
        var height = Math.Abs(one.y - two.y) + 1;
        return width * height;
    }
}