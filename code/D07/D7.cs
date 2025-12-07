namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;

public class D7 : IDay
{
    List<string> data = new List<string>();
    List<List<char>> working_data = new List<List<char>>();
    List<List<string>> p2_working_data = new List<List<string>>();

    bool do_puzzle_2 = true;

    private const char _start = 'S';
    private const char _split = '^';
    private const char _empty = '.';
    private const char _beam = '|';

    public void Solve(){
        var counter = 0L;

        if (do_puzzle_2)
        {
            DrawTimelineBeams();

            //WriteP2DataToDebugFile();
    
            counter = CountTimelines();
        }
        else
        {
            //NOTE: this gives me "game dev working on terrain generation" vibes
            DrawBeams();
            counter = CountSplits();
        }

        Console.WriteLine("Solution: " + counter);
    }
    
    public void Init(){
        ClearAll();

        string[] lines = Utils.ReadInput("D7.txt");
        data = lines.ToList();

        foreach (string line in data)
        {
            working_data.Add(line.ToList());
            p2_working_data.Add(line.Select(c => c.ToString()).ToList());
        }
    }

    private void ClearAll()
    {
        data.Clear();
        working_data.Clear();
        p2_working_data.Clear();
    }

    //used in a slow recursive solution
    private int GetStartingPoint()
    {
        for(int i = 0; i < data[0].Length; i++)
        {
            if(data[0][i] == _start)
            {
                return i;
            }
        }

        Console.WriteLine("Well... that's bad");
        return -1;
    }

    //simple, but too taxing and slow
    private int CountSplittingRecursive(int row, int column)
    {
        while(row < data.Count())
        {
            if(data[row][column] == _split)
            {
                //return this single split + all of the splits from the following beams
                //(NOTE: might produce too high of a result if beams merge)
                Console.WriteLine("split on: " + row + ", " + column);
                return 1 + CountSplittingRecursive(row, column - 1) + CountSplittingRecursive(row, column + 1);
            }

            row += 1;
        }

        return 0; //only reached when we hit the bottom, meaning no more splits
    }

    //replace _empty with _beam if:
    // 1. there is a _start above (if not found already)
    // 2. there is a beam above
    // 3. there is a splitter on left/right with a beam above the spliiter
    private void DrawBeams()
    {
        //reduces the number of comparisons needed
        bool start_not_found = true;

        for (int y = 1; y < working_data.Count(); y++) //start in the second row as the first one only contains an S
        {
            for (int x = 0; x < working_data[0].Count(); x++)
            {
                //only changing status if we're on an empty field
                if(working_data[y][x] == _empty)
                {
                    // 1. there is a _start above (if not found already)
                    if(start_not_found)
                    {
                        if(working_data[y-1][x] == _start)
                        {
                            start_not_found = false;
                            working_data[y][x] = _beam;
                            break; // finding start means there are no more beams on this level, so we cut like 70 more comparisons
                        }
                        // if the start was not found, there are no beams, thus - there is no point if checking other conditions
                        // yes, this could backfire...
                        continue;
                    }

                    // 2. there is a beam above
                    if (working_data[y-1][x] == _beam)
                    {
                        working_data[y][x] = _beam;
                        continue; //just to make sure we don't check other conditions, probably not needed
                    }
                    // 3.1. there is a splitter on right with a beam above the spliiter
                    else if (x < working_data[0].Count() - 1 && working_data[y][x+1] == _split && working_data[y-1][x+1] == _beam)
                    {
                        working_data[y][x] = _beam;
                        continue; //just to make sure we don't check other conditions, probably not needed
                    }
                    // 3.2. there is a splitter on left with a beam above the spliiter
                    else if (x > 0 && working_data[y][x-1] == _split && working_data[y-1][x-1] == _beam)
                    {
                        working_data[y][x] = _beam;
                        continue; //just to make sure we don't check other conditions, probably not needed
                    }
                }
            }
        }
    }

    //count each _split with a _beam above it
    // (there are less splitters than beams)
    private long CountSplits()
    {
        var counter = 0L;

        for (int y = 1; y < working_data.Count() - 1; y++)
        {
            for (int x = 0; x < working_data[0].Count(); x++)
            {
                if(working_data[y][x] == _split)
                {
                    if (working_data[y-1][x] == _beam)
                    {
                        counter++;
                    }
                }
            }
        }

        return counter;
    }

    //instead of drawing _beam, this one "draws" numbers that correspond to the amount of timelines in the beam
    // so the diagram looks like this:
    /*

    ......S......
    ......1......
    .....1^1.....
    .....1.1.....
    ....1^2^1....
    ....1.2.1....
    ...1^121^1...
    ...1.121.1...
    ..1^2^4^11...
    ..1.2.4.11...


    */
    //replace _empty with TOTAL if:
    // 1. there is a _start above (if not found already), replace with 1 (TOTAL = 1)
    // 2. there is a NUMBER above, add NUMBER to TOTAL
    // 3. there is a splitter on either left OR right with a NUMBER above the spliiter (add NUMBER to TOTAL)
    //NOTE: this looks UGLY with the amount of tabs needed... but hopefully it functions well
    private void DrawTimelineBeams()
    {
        //reduces the number of comparisons needed
        bool start_not_found = true;

        for (int y = 1; y < p2_working_data.Count(); y++) //start in the second row as the first one only contains an S
        {
            for (int x = 0; x < p2_working_data[0].Count(); x++)
            {
                //only changing status if we're on an empty field
                if(p2_working_data[y][x] == _empty.ToString())
                {
                    // 1. there is a _start above (if not found already)
                    if(start_not_found)
                    {
                        if(p2_working_data[y-1][x] == _start.ToString())
                        {
                            start_not_found = false;
                            p2_working_data[y][x] = "1";
                            break; // finding start means there are no more beams on this level, so we cut like 70 more comparisons
                        }
                        // if the start was not found, there are no beams, thus - there is no point if checking other conditions
                        // yes, this could backfire...
                        continue;
                    }

                    var total = 0L;

                    // 2. there is a NUMBER above
                    if (long.TryParse(p2_working_data[y-1][x], out var value))
                    {
                        total += value;
                    }
                    // 3.1.there is a splitter on right with a NUMBER above the spliiter
                    if (
                        x < p2_working_data[0].Count() - 1 
                        && p2_working_data[y][x+1] == _split.ToString()
                        && long.TryParse(p2_working_data[y-1][x+1], out var valueR)
                    ){
                        total += valueR;
                    }
                    // 3.2. there is a splitter on left with a NUMBER above the spliiter
                    if (
                        x > 0 
                        && p2_working_data[y][x-1] == _split.ToString()
                        && long.TryParse(p2_working_data[y-1][x-1], out var valueL)
                    ){
                        total += valueL;
                    }
                    
                    p2_working_data[y][x] = total.ToString();
                }
            }
        }
    }

    //just count the values of last beams
    private long CountTimelines()
    {
        var counter = 0L;
        
        foreach(string s in p2_working_data.Last())
        {
            if(long.TryParse(s, out var value))
            {
                counter += value;
            }
        }

        return counter;
    }

    private void WriteP2DataToDebugFile(){

        var path = Path.Combine("inputs", "debug7.txt");
        File.Delete(path); //ensure file is empty (aka delete before writing)
        
        using (StreamWriter outputFile = new StreamWriter(path))
        {
            foreach (var line in p2_working_data)
                outputFile.WriteLine(string.Join("", line));
        }
    }
}