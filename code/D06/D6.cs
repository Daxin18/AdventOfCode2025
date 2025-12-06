namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class D6 : IDay
{
    List<string> raw_data = new List<string>();
    List<List<string>> data = new List<List<string>>();
    bool do_puzzle_2 = true;

    public void Solve(){
        long total_output = 0;

        if(do_puzzle_2)
        {
            ConvertRawToChunks();
            total_output = SolveP2();
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
            raw_data.Add(line);
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

    private long SolveP2()
    {
        long result = 0;

        for (int i = 0; i < data[0].Count(); i++)
        {
            var sign = data.Last()[i][0]; //only grab the first char as it's the sign
            long subresult = 0;
            var number_chunk = data
                            .SkipLast(1)
                            .Select(x => x[i])
                            .ToList();

            var numbers = new List<long>();

            for(int j = 0; j < number_chunk[0].Length; j++)
            {
                numbers.Add(
                    Convert.ToInt64( //convert to long
                        number_chunk
                            .Select(x => x[j]) //get the chars from given column
                            .Where(x => x != ' ') //remove spaces
                            .Aggregate("", (curr, next) => curr + next) //concatenate them
                            .ToString() //just to be sure
                    )
                );
            }

            switch(sign)
            {
                case '+':
                    subresult = numbers.Sum();
                    break;
                case '*':
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

    //finds the symbol that is always the first column of a chunk, cuts the data accordingly
    private void ConvertRawToChunks()
    {
        List<List<string>> tmp = new List<List<string>>();
        int starting_idx = 0;
        var last = raw_data.Last();

        //fill tmp for ease of use
        raw_data.ForEach(x => tmp.Add(new List<string>()));

        //starting from the second one so we do not run into issues
        for(int i = 1; i < last.Length; i++)
        {
            if(last[i] != ' ')
            {
                for(int j = 0; j < raw_data.Count(); j++)
                {
                    tmp[j].Add(raw_data[j][starting_idx..(i-1)]); //extract the chunk from each column, removing the space in between
                }

                starting_idx = i;
            }
        }

        //add last chunk
        if(starting_idx != last.Length)
        {
            for(int j = 0; j < raw_data.Count(); j++)
            {
                tmp[j].Add(raw_data[j][starting_idx..last.Length]);
            }
        }

        data.Clear();
        data = tmp;

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
        raw_data.Clear();
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