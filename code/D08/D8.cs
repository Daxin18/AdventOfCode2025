namespace AoC2025;

using System;
using System.Collections.Generic;
using System.IO;


public class D8 : IDay
{
    //raw coordinates
    List<JunctionBox> junction_boxes = new List<JunctionBox>();
    //all connections, no duplicates (like from->to and to->from)
    List<BoxConnection> connections = new List<BoxConnection>();
    //map of all created connections for each junction box
    List<List<JunctionBox>> circuits = new List<List<JunctionBox>>();

    bool do_puzzle_2 = false;
    bool do_debug = false;

    public void Solve(){
        var counter = 0L;

        if (do_debug)
        {
            //NOPE
            //used this for some crazy bugs i had
            //not needed, just a working space, ignore it
        }
        else if(do_puzzle_2)
        {

        }
        else
        {
            counter = SolveP1(1000);
        }

        Console.WriteLine("Solution: " + counter);
    }
    
    public void Init(){
        ClearAll();
        string[] lines = Utils.ReadInput("D8.txt");

        //create junction_boxes
        foreach (var line in lines)
        {
            List<int> coordinates = line
                                .Split(',')
                                .Select(x => Convert.ToInt32(x))
                                .ToList();

            var jb = new JunctionBox(coordinates[0], coordinates[1], coordinates[2]);
            junction_boxes.Add(jb);
        }

        //create connections
        //this method only creates all of the COMBINATIONS, so no (from->to and to->from) duplicates
        for (int i = 0; i < junction_boxes.Count() - 1; i++)
        {
            var from = junction_boxes[i];
            for (int j = i + 1; j < junction_boxes.Count(); j++)
            {
                var to = junction_boxes[j];
                var conn = new BoxConnection(from, to); //note: distance is calculated inside the BoxConnection constructor
                connections.Add(conn);
            }
        }
    }

    private void ClearAll()
    {
        junction_boxes.Clear();
        connections.Clear();
        circuits.Clear();
    }

    private long SolveP1(int connections_limit)
    {
        //counter will increase by 1 each time a new connection is made
        //FUNFACT: this is not how it's supposed to work....
        var counter = 0;
        var skip_counter = 0;
        // order connections be the distance to get the shortest ones first
        connections = connections.OrderBy(x => x.Distance).ToList();
        
        WriteToDebugFile();

        //iterate as long as possible, broken when counter hits connections_limit
        // 1. check if connection is part of the circuit
        //      1.1 yes - add it to the circuit 
        //      1.2 no - create a new circuit
        for(int i = 0; i < connections_limit; i++)
        {
            var conn = connections[i]; //get the shortest connection (as previous ones were already done)

            if(IsPartOfCircuit(conn, out var circuit))
            {
                if(AddConnectionToCircuit(conn, circuit))
                {
                    counter++; //only add connection if something changed in a circuit
                }
                else{
                    skip_counter++;
                }
            }
            else
            {
                var new_circuit = new List<JunctionBox>();
                AddConnectionToCircuit(conn, new_circuit);
                circuits.Add(new_circuit);
                counter++; //in this case - a connection is always added
            }

            FixInterCircuitConnection();

            // counter check 
            if (counter >= connections_limit)
            {
                //Console.WriteLine("This is COUNTERI: " + i);
                break;
            }
        }

        //Console.WriteLine("Skipped " + skip_counter + " connections");

        //not really needed... but add remaining JunctionBoxes as 1-size circuits
        foreach (var jb in junction_boxes)
        {
            if(!IsPartOfCircuit(jb, out var circuit))
            {
                var new_circuit = new List<JunctionBox>();
                new_circuit.Add(jb);
                circuits.Add(new_circuit);
            }
        }
        
        //SORT
        circuits = circuits.OrderByDescending(x => x.Count()).ToList();

        foreach (var circuit in circuits)
        {
            //Console.WriteLine(String.Join("; ", circuit));
            //Console.WriteLine(circuit.Count());
        }
        
        return circuits[0].Count() * circuits[1].Count() * circuits[2].Count();
    }

    //returns true if connection is part of any circuit
    private bool IsPartOfCircuit(BoxConnection conn, out List<JunctionBox> circuit)
    {
        bool flag = false;
        if(Math.Floor(conn.Distance) == 333)
        {
            //Console.WriteLine(conn.ToString());
            //flag = true;
        }

        foreach (var circ in circuits)
        {
            foreach(var node in circ)
            {
                if (flag)
                {
                    //Console.WriteLine(node.ToString() + ": " + node.Is(conn.To));
                }
                if (node.Is(conn.From) || node.Is(conn.To))
                {
                    circuit = circ;
                    return true;
                }
            }
        }

        circuit = null;
        return false;
    }

     //returns true if junctionbox is part of any circuit
    private bool IsPartOfCircuit(JunctionBox jb, out List<JunctionBox> circuit)
    {
        foreach(var circ in circuits)
        {
            foreach(var node in circ)
            {
                if (node.Is(jb))
                {
                    circuit = circ;
                    return true;
                }
            }
        }

        circuit = null;
        return false;
    }

    // adds JunctionBoxes to the circuit if they were not there previously
    // NOTE: this ensures that adding a connection with two junction boxes in the same circuit will not change anything!
    // the method returns true if at least one JunctionBox was added
    private bool AddConnectionToCircuit(BoxConnection conn, List<JunctionBox> circuit)
    {
        bool result = false;
        
        if(!circuit.Contains(conn.From))
        {
            circuit.Add(conn.From);
            result = true;
        }
        if (!circuit.Contains(conn.To))
        {
            circuit.Add(conn.To);
            result = true;
        }

        return result;
    }

    private void WriteToDebugFile(){

        var path = Path.Combine("inputs", "debug8p1.txt");
        File.Delete(path); //ensure file is empty (aka delete before writing)
        
        using (StreamWriter outputFile = new StreamWriter(path))
        {
            for(int i = 0; i < 11; i++)
            {
               outputFile.WriteLine(connections[i].ToString());
            }
        }
    }

    //after adding a connection to a circuit it COULD connect to a different circuit
    //this step merges circuits that have 
    private void FixInterCircuitConnection()
    {
        for (int i = 0; i < circuits.Count(); i++)
        {
            var circuit = circuits[i];
            for (int j = i + 1; j < circuits.Count(); j++)
            {
                var second = circuits[j];
                if(circuit.Intersect(second).Any())
                {
                    //Console.WriteLine(String.Join(" ; ", second));
                    circuits[i] = circuit.Union(second).ToList();
                    circuits.RemoveAt(j);
                    return; //max one intersection since we're doing it every loop
                }
            }
        }
    }
}