namespace AoC2025;

class Program
{
    //Reminder:
    // dotnet run -- arg0 arg1 arg2 
    public static void Main(string[] args)
    {
        string arg = args.Length > 0 ? args[0] : "default (day 1)";
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
            case "D2":
                day = new D2();
                break;
            case "D3":
                day = new D3();
                break;
            case "D4":
                day = new D4();
                break;
            case "D5":
                day = new D5();
                break;
            case "D6":
                day = new D6();
                break;
            case "D7":
                day = new D7();
                break;
            case "D8":
                day = new D8();
                break;
            //TODO: other days
            default:
                day = new D1();
                break;
        }

        return day;
    }
}



