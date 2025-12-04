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

    public void Solve(){

        var counter = 0;

        for(int x = 0; x < grid.Count(); x++)
        {
            for(int y = 0; y < grid[0].Count(); y++)
            {
                if(grid[x][y]) //only check neighbours if there is a roll of paper
                {
                    if (CanBeAccessed(x,y))
                    {
                        counter++;
                    }
                }
            }
        }

        Console.WriteLine("Solution: " + counter);
    }
    
    public void Init(){
        string[] lines = Utils.ReadInput("D4.txt");
        ClearGrid();
        FillGrid(lines);
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