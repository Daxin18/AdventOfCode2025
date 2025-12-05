namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class D5 : IDay
{
    List<(long from, long to)> ranges = new List<(long,long)>();
    List<long> available_ids = new List<long>();

    public void Solve(){
        int counter = 0;

        foreach (long id in available_ids)
        {
            foreach ((long from, long to) range in ranges)
            {
                if (id >= range.from && id <= range.to)
                {
                    counter++;
                    break; //only breaks the inner loop
                }
            }
        }

        Console.WriteLine("Solution: " + counter);
    }
    
    public void Init(){
        ClearAll();

        string[] lines = Utils.ReadInput("D5.txt");

        int idx = 0;
        ExtractRanges(lines, ref idx);
        //increment idx so it points to the first available id instead of an empty line
        idx++;
        ExtractIds(lines, idx);
    } 

    private void ClearAll()
    {
        ranges.Clear();
        available_ids.Clear();
    }

    //the idx will be modified, after the method it will point to the first line without the range (empty line)
    private void ExtractRanges(string[] lines, ref int idx)
    {
        //file starts with a set of ranges, each range has '-' in it
        while(lines[idx].Contains('-'))
        {
            string[] range = lines[idx].Split('-');
            long[] range_longified = {Convert.ToInt64(range[0]), Convert.ToInt64(range[1])};

            ranges.Add((range_longified[0], range_longified[1]));

            idx++;
        }
    }

    private void ExtractIds(string[] lines, int idx)
    {
        for(int i = idx; i < lines.Length; i++)
        {
            long id = Convert.ToInt64(lines[i]); //is it safe? nope
            available_ids.Add(id);
        }
    }
}