namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class D6 : IDay
{
    List<List<string>> data = new List<List<string>>();
    bool do_puzzle_2 = false;

    public void Solve(){
        long total_output = 0;

        if(do_puzzle_2)
        {

        }
        else
        {
            total_output = SolveP1();
        }

        Console.WriteLine("Solution: " + total_output);
    }
    
    public void Init(){
        ClearAll();
        string[] lines = Utils.ReadInput("D6.txt");

        foreach (var line in lines)
        {
            var cleared = line
                        .Split(' ')
                        .Where(x => x != "")
                        .ToList();
            data.Add(cleared);
        }

        if (DataSanityCheck())
        {
            Console.WriteLine("List.Count() for each individual row/line is different");
        }

    }

    private long SolveP1()
    {
        long result = 0;

        for (int i = 0; i < data[0].Count(); i++)
        {
            var sign = data.Last()[i];
            long subresult = 0;
            var numbers = data
                            .SkipLast(1)
                            .Select(x => x[i])
                            .Select(x => Convert.ToInt64(x))
                            .ToList();

            switch(sign)
            {
                case "+":
                    subresult = numbers.Sum();
                    break;
                case "*":
                    subresult = numbers.Aggregate(1L, (acc, x) => acc * x);
                    break;
                default:
                    Console.WriteLine("We fucked up");
                    return -1;
            }

            result += subresult;
        }

        return result;
    }

    private void ClearAll()
    {
        data.Clear();
    }

    //returns true if we should panic
    private bool DataSanityCheck()
    {
        //Console.WriteLine(data[0].Count());

        foreach (List<string> line in data.Skip(1))
        {
            //Console.WriteLine(line.Count());
            if (line.Count() != data[0].Count())
            {
                return true;
            }
        }

        return false;
    }
}