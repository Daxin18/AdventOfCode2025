namespace AoC2025;

class Program
{
    //Reminder:
    // dotnet run -- arg0 arg1 arg2 
    public static void Main(string[] args)
    {
        string arg = args.Length > 0 ? args[0] : "default";
        IDay current_day = SetPuzzle(arg);
        Console.WriteLine("Solving puzzle for " + arg);

        current_day.Init();
        current_day.Solve();
    }

    private static IDay SetPuzzle(string arg){
        IDay day = null;

        switch(arg)
        {
            case "D1":
                day = new D1();
                break;
            //TODO: other days
            default:
                day = new D1();
                break;
        }

        return day;
    }
}



