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

    //Plan:
    // 1. iterate over all red tile combinations
    // 2. "continue;" on combinations that can be ignored because of safe(-ish) assumptions
    // 3. calculate area, if it's larger than current largest, verify it's good
    // 3.1. check for "inner space" red tiles, "continue;" if any were found
    // 3.2. check for "border spaces", "continue;" if any were not good
    // 4. everything was fine, reassign largest and go to next iteration
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
                    //if(!AreaGood(i, j))
                    if(!AreaGood(one, two))
                    {
                        continue;
                    }

                    largest = area;
                    //Console.WriteLine(one.x + "," + one.y + " : " + two.x + "," + two.y);
                }
            }
        }

        //Console.WriteLine(counter);
        return largest;
    }

    private bool AreaGood((long x, long y) one, (long x, long y) two)
    {
        for(int i = 0; i < red_tiles.Count(); i++)
        {
            var t1 = red_tiles[i];
            var t2 = GetTileAt(i + 1);
            
            //make those variables as they will be needed multiple times
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

    /*
        1. drop the check for border and inner points... (keep the safe(-ish) assumptions)
        2. determine which "origin" tile has minY, treat its idx as "ONE", the other "origin" tiles' as "TWO"
        3. slice red tiles into two sets:
            - S1: ONE -> TWO (all tiles between ONE and TWO; AKA i = ONE + 1; i < TWO; i++, with wrapping)
            - S2: TWO -> ONE (can just go red_tiles - S1 to simplify)
        4. look at x, if "ONE" has minx, choose path "5a", otherwise path "5b"
        5. check S1[0] for differences in x:

            5a.1. there is a difference in x, not in y; good if both are true:
                5a.1.1 S1.Where(t => (t.x >= maxx) || (t.y <= miny)).Count() == S1.Count();
                5a.1.2 S2.Where(t => (t.x <= minx) || (t.y >= maxy)).Count() == S2.Count();
            5a.2. no difference in x, but there is one in y; good if both are true:
                5a.2.1 S1.Where(t => (t.x <= minx) || (t.y >= maxy)).Count() == S1.Count();
                5a.2.2 S2.Where(t => (t.x >= maxx) || (t.y <= miny)).Count() == S2.Count();

            5b.1. there is a difference in x, not in y; good if both are true:
                5b.1.1 S1.Where(t => (t.x <= minx) || (t.y <= miny)).Count() == S1.Count();
                5b.1.2 S2.Where(t => (t.x >= maxx) || (t.y >= maxy)).Count() == S2.Count();
            5b.2. no difference in x, but there is one in y; good if both are true:
                5b.2.1 S1.Where(t => (t.x >= maxx) || (t.y >= maxy)).Count() == S1.Count();
                5b.2.2 S2.Where(t => (t.x <= minx) || (t.y <= miny)).Count() == S2.Count();
        
        6. Profit?
    */
    ///assumption: i < j
    // private bool AreaGood(int i, int j)
    // {
    //     //2. determine which "origin" tile has minY, treat its idx as "ONE", the other "origin" tiles' as "TWO"
    //     int ONE = 0;
    //     int TWO = 0;
    //     bool is_default = true;

    //     if (red_tiles[i].y < red_tiles[j].y)
    //     {
    //         ONE = i;
    //         TWO = j;
    //     }
    //     else
    //     {
    //         ONE = j;
    //         TWO = i;
    //         is_default = false;
    //     }
    //     (long x, long y) one = red_tiles[ONE];
    //     (long x, long y) two = red_tiles[TWO];

    //     // 3. slice red tiles into two sets:
    //     //     - S1: ONE -> TWO (all tiles between ONE and TWO; AKA i = ONE + 1; i < TWO; i++, with wrapping)
    //     //     - S2: TWO -> ONE (can just go red_tiles - S1 to simplify)
    //     List<(long x, long y)> S1 = new List<(long, long)>();
    //     List<(long x, long y)> S2 = new List<(long, long)>();

    //     var boundry = is_default ? TWO : red_tiles.Count() + TWO;
    //     for(int k = ONE + 1; k < boundry; k++)
    //     {
    //         S1.Add(GetTileAt(k));
    //     }

    //     S2 = red_tiles.Where(t => !S1.Contains(t)).ToList();

    //     //Console.WriteLine("ONE: " + one.x + "," + one.y + ", TWO: " + two.x + "," + two.y);
    //     //Console.WriteLine("DEBUG: S1.Count() = " + S1.Count());
    //     //Console.WriteLine("DEBUG: S2.Count() = " + S2.Count());
    //     //Console.WriteLine("DEBUG: red_tiles.Count() = " + red_tiles.Count());

    //     var cond_1 = true;
    //     var cond_2 = true;

    //     //4. look at x, if "ONE" has minx, choose path "5a", otherwise path "5b"
    //     if (one.x == Math.Min(one.x, two.x)) //5a
    //     {
    //         if(S1[0].x != one.x) //5a.1
    //         {
    //             cond_1 = ConditionCheck(S1, one, two, "maxd"); //check comment above implementation
    //             cond_2 = ConditionCheck(S2, one, two, "mind");
    //         }
    //         else //5a.2
    //         {
    //             cond_1 = ConditionCheck(S1, one, two, "mind");
    //             cond_2 = ConditionCheck(S2, one, two, "maxd");
    //         }
    //     }
    //     else //5b
    //     {
    //         if(S1[0].x != one.x) //5b.1
    //         {
    //             cond_1 = ConditionCheck(S1, one, two, "mins");
    //             cond_2 = ConditionCheck(S2, one, two, "maxs");
    //         }
    //         else //5b.2
    //         {
    //             cond_1 = ConditionCheck(S1, one, two, "maxs");
    //             cond_2 = ConditionCheck(S2, one, two, "mins");
    //         }
    //     }

    //     return cond_1 && cond_2;
    // }

    //path can be:
    // mins - minx, miny (same for y)
    // maxs - maxx, maxy (same for y)
    // mind - minx, maxy (different for y)
    // maxd - maxx, miny (different for y)
    /*
        1. keep all previous calculations and conditions for all paths in point 5.
        2. create "is_inside" flag, before the for loop, make it false by default
        2. modify the conditions to go tile-by-tile (instead of one-line check), do step 4 - 6 for both S1 and S2
        4. if the tile does not fullfil either condition (for x and for y), check the flag
            4.1. if it's true, return, the area is not good
            4.2. if it's false, continue, it's ok, but there can be more problems later on
        5. set flag to true if:
            - a tile that only fulfills a single condition (lets say x) is "inside rectangle" by the other condition (y < maxy && y > miny)
        6. set flag to false otherwise
    */
    private bool ConditionCheck(
        List<(long x, long y)> list,
        (long x, long y) one,
        (long x, long y) two,
        string path
    )
    {
        bool is_inside = false;

        //make those variables as they will be needed multiple times
        var minx = Math.Min(one.x, two.x);
        var maxx = Math.Max(one.x, two.x);
        var miny = Math.Min(one.y, two.y);
        var maxy = Math.Max(one.y, two.y);

        Func<(long x, long y), bool> minx_f = (t) => t.x <= minx;
        Func<(long x, long y), bool> maxx_f = (t) => t.x >= maxx;
        Func<(long x, long y), bool> miny_f = (t) => t.y <= miny;
        Func<(long x, long y), bool> maxy_f = (t) => t.y >= maxy;

        Func<(long x, long y), bool> cond_x = (t) => true;
        Func<(long x, long y), bool> cond_y = (t) => true;

        switch(path)
        {
            case "mins":
                cond_x = minx_f;
                cond_y = miny_f;
                break;
            case "maxs":
                cond_x = maxx_f;
                cond_y = maxy_f;
                break;
            case "mind":
                cond_x = minx_f;
                cond_y = maxy_f;
                break;
            case "maxd":
                cond_x = maxx_f;
                cond_y = miny_f;
                break;
        }

        for(int i = 0; i < list.Count(); i++)
        {
            var tile = list[i];
            var flag_x = cond_x(tile);
            var flag_y = cond_y(tile);

            if(!flag_x && !flag_y) //no condition fulfilled
            {
                if (is_inside)
                {
                    return false;
                }
                else
                {
                    continue;
                }
            }
            else if (flag_x && flag_y) //both fulfilled
            {
                is_inside = false;
            }
            else if (flag_x) // only x fulfilled
            {
                is_inside = tile.y <= maxy && tile.y >= miny;
            }
            else // only y fulfilled
            {
                is_inside = tile.x <= maxx && tile.x >= minx;
            }
        }

        return true;
    }

    // checks if area contains any tiles that are not good
    // both checks in same method to avoid iterating multiple times
    // private bool AreaNotGood((long x, long y) one, (long x, long y) two)
    // {
    //     //make those variables as they will be needed multiple times
    //     var minx = Math.Min(one.x, two.x);
    //     var maxx = Math.Max(one.x, two.x);
    //     var miny = Math.Min(one.y, two.y);
    //     var maxy = Math.Max(one.y, two.y);

    //     for(int i = 0; i < red_tiles.Count(); i++)
    //     {
    //         var tile = red_tiles[i];

    //         // both "origin" tiles are considered good, no need to make checks for them
    //         if(tile == one || tile == two)
    //         {
    //             continue;
    //         }

    //         // 3.1. check for "inner space" red tiles
    //         if(
    //             tile.x > minx
    //             && tile.x < maxx
    //             && tile.y > miny
    //             && tile.y < maxy
    //         )
    //         {
    //             return true;
    //         }

    //         //check the corners
    //         if (
    //             (tile.x == minx && tile.y == miny)
    //             || (tile.x == minx && tile.y == maxy)
    //             || (tile.x == maxx && tile.y == miny)
    //             || (tile.x == maxx && tile.y == maxy)
    //         )
    //         {
    //             if (isInnerCorner(i))
    //             {
    //                 return true;
    //             }
    //             continue;
    //         }

    //         // 3.2. check for "border spaces"
    //         if(
    //             tile.x == minx
    //             && (tile.y > miny && tile.y < maxy)
    //             && (GetTileAt(i + 1).x > minx
    //             || GetTileAt(i - 1).x > minx)
    //         )
    //         {
    //             return true;
    //         }
    //         else if( //else as we can't have both minx and maxx as the same coordinate
    //             tile.x == maxx
    //             && (tile.y > miny && tile.y < maxy)
    //             && (GetTileAt(i + 1).x < maxx
    //             || GetTileAt(i - 1).x < maxx)
    //         )
    //         {
    //             return true;
    //         }
    //         else if( //corner bt itself is fine, thus "else" is here
    //             tile.y == miny
    //             && (tile.x > minx && tile.x < maxx)
    //             && (GetTileAt(i + 1).y < miny
    //             || GetTileAt(i - 1).y < miny)
    //         )
    //         {
    //             return true;
    //         }
    //         else if(
    //             tile.y == maxy
    //             && (tile.x > minx && tile.x < maxx)
    //             && (GetTileAt(i + 1).y < maxy
    //             || GetTileAt(i - 1).y < maxy)
    //         )
    //         {
    //             return true;
    //         }
    //     }

    //     return false;
    // }

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

    //right turn = outer
    // left turn = inner
    private bool isInnerCorner(int i)
    {
        (long x, long y) prev = GetTileAt(i-1);
        (long x, long y) curr = red_tiles[i];
        (long x, long y) next = GetTileAt(i+1);

        (long x, long y) v1 = (curr.x - prev.x, curr.y - prev.y);
        (long x, long y) v2 = (next.x - curr.x, next.y - curr.y);

        var cross_prod = v1.x * v2.y - v1.y * v2.x;

        return cross_prod > 0; // > 0 means left turn, aka inner corner
    }

    private long CalculateArea((long x, long y) one, (long x, long y) two)
    {
        var width = Math.Abs(one.x - two.x) + 1;
        var height = Math.Abs(one.y - two.y) + 1;
        return width * height;
    }

    /*Place for a written brainstorm for P2...

    - creating a whole model of 100k x 100k grid is not feasible - too slow
        - need a way to calculate it mathematically or at least simplify the problem
    
    - what we KNOW (NOTE: "origin" tiles are the ones that were used as boundries of the rectangle)
        1. if any of the "inner spaces" is a red tile - the rectangle is not inside
        2. if any of the "border spaces" is a red tile:
            2.1. it's a corner
                - good only if immediate neighbor is good (aka corners are good by themselves)
            2.2. it's somewhere in the border
                - good if both it's immediate neighbors are to the same side of the border as it's on (simply put neighbor.x >= this.x with extra steps)
    
    - what can be safely(-ish) assumed:
        1. neighboring red tiles will not create the largest rectangle
            - the next closest neighbor will create a larger one, with direct neighbor as a corner
            - can ignore immediate neighbors (Math.Abs(i - j) == 1)
        2. only every second red tile (odd number of tiles in between the two) is a viable "big rectangle" candidate
            - if there is an even number of tiles in between two tiles, the next tile is either:
                - further away (making rectangle bigger)
                - or closer (making rectangle with this one not valid)
            - can ignore red tile pairs where ((i - j) % 2 != 0)

    - what are the PROBLEMS after implementing all above:
        1. corners are a bit more tricky as there are two types:
            1.1. "inner"
                - 90 degree angle OUTSIDE of the main shape
                - problematic, them being a corner of the rectangle means it's outside, not good
            1.2. "outer"
                - 270 degree angle OUTSIDE of the main shape
                - cool, them being a corner of the rectangle means it's right on the edge of the shape, good
        - centroid distance relative to neighbors won't work as the centroid can be outside of the shape...
        - ...
        - use vector cross product?
        2. The previous thing is only true IF the the corner is direct neighbor of the "origin" tiles which may not be the case...
            - gotta scrap the previous example


    -- NEW IDEA (pseudocode):
        1. drop the check for border and inner points... (keep the safe(-ish) assumptions)
        2. determine which "origin" tile has minY, treat its idx as "ONE", the other "origin" tiles' as "TWO"
        3. slice red tiles into two sets:
            - S1: ONE -> TWO (all tiles between ONE and TWO; AKA i = ONE + 1; i < TWO; i++, with wrapping)
            - S2: TWO -> ONE
        4. look at x, if "ONE" has minx, choose path "5a", otherwise path "5b"
        5. check S1[0] for differences in x:

            5a.1. there is a difference in x, not in y; good if both are true:
                5a.1.1 S1.Where(t => (t.x >= maxx) || (t.y <= miny)).Count() == S1.Count();
                5a.1.2 S2.Where(t => (t.x <= minx) || (t.y >= maxy)).Count() == S2.Count();
            5a.2. no difference in x, but there is one in y; good if both are true:
                5a.2.1 S1.Where(t => (t.x <= minx) || (t.y >= maxy)).Count() == S1.Count();
                5a.2.2 S2.Where(t => (t.x >= maxx) || (t.y <= miny)).Count() == S2.Count();

            5b.1. there is a difference in x, not in y; good if both are true:
                5b.1.1 S1.Where(t => (t.x <= minx) || (t.y <= miny)).Count() == S1.Count();
                5b.1.2 S2.Where(t => (t.x >= maxx) || (t.y >= maxy)).Count() == S2.Count();
            5b.2. no difference in x, but there is one in y; good if both are true:
                5b.2.1 S1.Where(t => (t.x >= maxx) || (t.y >= maxy)).Count() == S1.Count();
                5b.2.2 S2.Where(t => (t.x <= minx) || (t.y <= miny)).Count() == S2.Count();
        
        6. Profit? -NOPE... still fails with a simple U-shape

    -- SOLUTION:
        1. keep all previous calculations and conditions for all paths in point 5.
        2. create "is_inside" flag, before the for loop, make it false by default
        2. modify the conditions to go tile-by-tile (instead of one-line check), do step 4 - 6 for both S1 and S2
        4. if the tile does not fullfil either condition (for x and for y), check the flag
            4.1. if it's true, return, the area is not good
            4.2. if it's false, return, and assume the area is good (?)
        5. set flag to true if:
            - a tile that only fulfills a single condition (lets say x) is "inside rectangle" by the other condition (y < maxy && y > miny)
        6. set flag to false otherwise


    ---NEWNEWNEWNEW IDEA:
        1. fuck it, just check if any connection crosses the square...
        2.. fuck me, it worked
    */
}