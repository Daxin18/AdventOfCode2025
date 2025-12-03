namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class D1 : IDay
{
    const int min_number = 0;
    const int max_number = 99;
    int current_number = 50;
    int desired_number = 0;
    int counter = 0;
    bool count_all_zeros = true;

    List<int> rotations = new List<int>();

    public void Solve(){
        foreach(int rotation in rotations)
        {
            RotateSlow(rotation);
            if (current_number == desired_number)
            {
                if(!count_all_zeros)
                    counter ++;
            } 
        }

        Console.WriteLine("Password: " + counter);
    }
    
    public void Init(){
        string[] lines = Utils.ReadInput("D1.txt");
        rotations.Clear();
        TranslateToRotations(lines);
    }

    private void TranslateToRotations(string[] lines){
        foreach (string line in lines)
        {
            int number = Convert.ToInt32(line.Substring(1));
            number *= line[0] == 'L' ? -1 : 1;
            rotations.Add(number);
        }
    }

    private void RotateSlow(int by){
        if (by > 0)
        {
            while (by > 0){
                by -= 1;
                current_number += 1;
                if (current_number > max_number)
                {
                    current_number = min_number;
                    if (count_all_zeros)
                        counter++;
                }
            }
        }
        else
        {
            while (by < 0){
                by += 1;
                current_number -= 1;
                if (count_all_zeros && current_number == desired_number)
                {
                    counter ++;
                }
                if (current_number < min_number)
                {
                    current_number = max_number;
                }
            }
        }
    }

}