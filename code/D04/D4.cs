namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class D4 : IDay
{
    // bool as there either is or isn't a roll of paper
    List<List<bool>> grid = new List<List<bool>>();
    int access_threshold = 4;
    const char _paper = '@';
    const char _empty = '.';
    bool do_single_iteration = false;

    public void Solve(){

        var counter = 0;

        counter = RemovePaperRecursive(0, do_single_iteration);

        Console.WriteLine("Solution: " + counter);
    }
    
    public void Init(){
        string[] lines = Utils.ReadInput("D4.txt");
        ClearGrid();
        FillGrid(lines);
    }

    //NOTE: It's one-use only anyway, so I will modify property grid instead of passing a copy,
    // it saves a bit of time and can be easily changed anyway if need be
    private int RemovePaperRecursive(int total_removed, bool do_single_iteration)
    {
        var counter = 0;
        //working grid will be used to keep the updated grid version (paper removed where possible) while still applying the logic to the existing grid
        var working_grid = grid.Select(row => row.ToList()).ToList();

        for(int x = 0; x < grid.Count(); x++)
        {
            for(int y = 0; y < grid[0].Count(); y++)
            {
                if(grid[x][y]) //only check neighbours if there is a roll of paper
                {
                    if (CanBeAccessed(x,y))
                    {
                        counter++;
                        working_grid[x][y] = false; //if a rall of paper can be removed - it is
                    }
                }
            }
        }

        if (do_single_iteration) //return instantly if only one iteration needs to be done (part 1)
        {
            return counter;
        }

        if (counter == 0) //return total if nothing changed
        {
            return total_removed;
        }

        grid = working_grid; //update the grid
        return RemovePaperRecursive(total_removed + counter, false);
    }

    private void ClearGrid()
    {
        grid.Clear();
    }

    private void FillGrid(string[] lines)
    {
        var row = 0;
        foreach(string line in lines)
        {
            grid.Add(new List<bool>());
            foreach(char x in line)
            {
                grid[row].Add(x == _paper);
            }

            row++;
        }
    }

    private bool CanBeAccessed(int x, int y)
    {
        var counter = 0;

        //xi and yi are offsets to find neighbours
        for(int xi = -1; xi <= 1; xi++)
        {
            for(int yi = -1; yi <= 1; yi++)
            {
                // continue on (0;0) offset, aka don't check self
                if (xi == 0 && yi == 0)
                {
                    continue;
                }

                var new_x = x + xi;
                var new_y = y + yi;

                //check if neighbour is out of bounds (aka if they exist)
                if (new_x < 0 || new_x >= grid.Count() || new_y < 0 || new_y >= grid[0].Count())
                {
                    continue;
                }

                //true means it's paper, so we increase counter and check if it reached the threshold yet
                if(grid[x + xi][y + yi])
                {
                    counter++;
                    if(counter >= access_threshold)
                    {
                        return false;
                    }
                }
            }
        }

        // we end up here if threshold was not reached
        return true;
    }

}