namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class D2 : IDay
{
    List<(long from,long to)> ranges = new List<(long, long)>();
    long invalid_sum = 0;
    bool use_alternate_invalid = true;

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

        if (use_alternate_invalid)
            return AlternateCheckInvalid(stringified);

        if (stringified.Length % 2 != 0)
            return false;
        
        var half = stringified.Length / 2;

        var first = stringified.Substring(0, half);
        var second = stringified.Substring(half);

        return first == second;
    }

    private bool AlternateCheckInvalid(string id)
    {
        if (id.Length < 2)
            return false;

        if (OneLongSeq(id))
            return true;

        if (id.Length < 4)
            return false;

        int half = id.Length / 2;

        // i is the length of a sequence
        for (int i = 2; i <= half; i++)
        {
            if (id.Length % i != 0) //no point in searching for i-length sequence if ID is not divisible by it
                continue;
            
            var sequence = id.Substring(0, i); //get first sequence

            bool do_continue = false;

            for (int j = i; j < id.Length; j += i) //iterate over next sequences
            {
                if (id.Substring(j, i) != sequence) //if sequence differs from the first, move on to longer sequences
                {
                    do_continue = true;
                    break;
                }    
            }

            if (do_continue)
                continue;

            return true; //if this point is reached, the sequence was found
        }

        return false; // if no sequence is found - return false aka ID is valid;
    }

    private bool OneLongSeq(string id){
        foreach (char i in id)
        {
            if (i != id[0])
                return false;
        }
        return true;
    }

}