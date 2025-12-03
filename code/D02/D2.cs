namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class D2 : IDay
{
    List<(long from,long to)> ranges = new List<(long, long)>();
    long invalid_sum = 0;

    public void Solve(){
        foreach ((long from, long to) range in ranges)
        {
            for (long i = range.from; i <= range.to; i++)
            {
                if (CheckInvalid(i))
                    invalid_sum += i;
            }
        }

        Console.WriteLine("Solution: " + invalid_sum);
    }
    
    public void Init(){
        string[] lines = Utils.ReadInput("D2.txt");
        string[] string_ranges = [];
        if(lines.Length == 1) //always true in this puzzle if all goes well
        {
            string_ranges = Utils.TranslateCSV(lines[0]);
            Console.WriteLine("Translated the first line");
        }

        if (string_ranges.Length > 0) //always true in this puzzle if all goes well
        {
            foreach (string range in string_ranges)
            {
                var split = range.Split('-');
                if (split.Length == 2) //always true in this puzzle if all goes well
                {
                    var from = Convert.ToInt64(split[0]);
                    var to = Convert.ToInt64(split[1]);
                    ranges.Add((from, to));
                }
            }
        }
    }

    private bool CheckInvalid(long id)
    {
        var stringified = id.ToString();

        if (stringified.Length % 2 != 0)
            return false;
        
        var half = stringified.Length / 2;

        var first = stringified.Substring(0, half);
        var second = stringified.Substring(half);

        return first == second;
    }

}