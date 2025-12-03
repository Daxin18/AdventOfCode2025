namespace AoC2025;

using System.IO;

public static class Utils{
    public static string[] ReadInput(string name){
        var path = Path.Combine("inputs", name);
        string[] lines = System.IO.File.ReadAllLines(path);
        return lines;
    }

    public static string[] TranslateCSV(string line){
        var values = line.Split(',');
        return values;
    }
}