namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class D5 : IDay
{
    List<(long from, long to)> ranges = new List<(long,long)>();
    List<long> available_ids = new List<long>();
    bool do_puzzle_2 = true;

    public void Solve(){
        long counter = 0;

        if(do_puzzle_2)
        {
            var no_overlap = RemoveOverlap();

            foreach ((long from, long to) range in no_overlap)
            {
                counter += range.to - range.from + 1;
            }
        }
        else
        {
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

    private List<(long,long)> RemoveOverlap()
    {
        var result = new List<(long,long)>();
        int i = 0;

        //i is the index of the "current range"
        //we then iterate over all the ranges after that, extending the current range if they overlap
        //at the end of each iteration, "range" is the sum (merged range) of all ranges that overlap with the "current range"
        //after that, the lowest range that did not overlap is set as the new "current range"
        while (i < ranges.Count())
        {
            (long from, long to) range = ranges[i];
            var starting_i = i;
            var do_emergency_end = true; //just in case i == ranges.Count() - 1, it is impossible to enter the inner loop, so i won't change

            //iterate over remaining ranges, merging those that overlap
            for(int j = i + 1; j < ranges.Count(); j++)
            {
                do_emergency_end = false;
                (long from, long to) next_range = ranges[j];

                if (next_range.from <= range.to)
                {
                    if(range.to < next_range.to)
                    {
                        range.to = next_range.to;
                    }
                }
                else
                {
                    i = j;
                    break;
                }

                //only reachable if there was no range that did not overlap
                i = ranges.Count();
            }
            result.Add(range);

            if(do_emergency_end)
            {
                break;
            }
        }

        return result;
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

        //sort the ranges so that the second puzzle is more doable;
        ranges = ranges.OrderBy(r => r.from).ToList();
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