namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class D3 : IDay
{
    string[] banks = [];
    long total_output = 0;
    bool use_recursion = true;

    public void Solve(){
        foreach (string bank in banks)
        {
            if (use_recursion)
            {
                total_output += GetMaxJoltageRec(bank, 12);
            }
            else
            {
                total_output += GetMaxJoltage(bank);
            }
        }

        Console.WriteLine("Solution: " + total_output);
    }
    
    public void Init(){
        string[] lines = Utils.ReadInput("D3.txt");
        banks = lines;
    }

    //find first instance of the highest number
    //find the highest number AFTER that instance
    //DONE
    //PREDICTION: the second part of the puzzle will increase the number of active batteries, forcing me to rewrite it for recursion
    private int GetMaxJoltage(string bank)
    {
        var first_idx = 0;
        var second_idx = 0;
        var current_max = '0';


        //finding highest number
        //going only up to length - 1, so the second number can be the last one
        for (int i = 0; i < bank.Length - 1; i++)
        {
            if (bank[i] > current_max)
            {
                current_max = bank[i];
                first_idx = i;
                if (bank[i] == '9') //instantly break on a 9 - the highest possible
                    break;
            }
        }

        current_max = '0'; //reset max
        second_idx = first_idx + 1; //set second_idx to the next number so we only search the following substring

        for (int i = second_idx; i < bank.Length; i++) //finding second highest number
        {
            if (bank[i] > current_max)
            {
                current_max = bank[i];
                second_idx = i;
                if (bank[i] == '9') //instantly break on a 9 - the highest possible
                    break;
            }
        }

        var result_string = bank[first_idx].ToString() + bank[second_idx].ToString();
        var result = Convert.ToInt32(result_string);
        return result;
    }

    //find biggest index, leaving space for the remaining iterations
    // cut substring
    // repeat
    // NOTE: iterations_remaining INCLUDES CURRENT ITERATION
    private long GetMaxJoltageRec(string bank, int iterations_remaining, string result = "")
    {
        var idx = 0;
        var current_max = '0';

        //finding highest number, leaving space for the rest
        for (int i = 0; i < bank.Length - (iterations_remaining - 1); i++)
        {
            if (bank[i] > current_max)
            {
                current_max = bank[i];
                idx = i;
                if (bank[i] == '9') //instantly break on a 9 - the highest possible
                    break;
            }
        }

        iterations_remaining -= 1;
        result += current_max.ToString();

        if (iterations_remaining == 0) //exit condition
        {
            var return_value = Convert.ToInt64(result);
            return return_value;
        }
        else 
        {
            var substring = bank.Substring(idx + 1);
            return GetMaxJoltageRec(substring, iterations_remaining, result);
        }
    }
}