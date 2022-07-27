using System.Collections;

namespace FMUSI;

public class AutomatGraph
{
    private readonly HashSet<string> eStates;
    private readonly int[,] ms;
    private readonly ArrayList nodes = new();
    public int start;
    private string startState;
    private readonly bool[] visited;

    //Konstruktor grafa
    public AutomatGraph(string startState, Dictionary<(string, char), HashSet<string>> delta)
    {
        // Postavljamo pocetno stanje
        // Iz Dictionary-a ucitavamo moguca stanja
        this.startState = startState;
        var keys = delta.Keys.ToArray();
        foreach (var var in keys)
        {
            var.ToTuple();
            if (!nodes.Contains(var.Item1)) nodes.Add(var.Item1);
        }

        // Provjeravamo da li imamo stanje koje nema ni jednog prelaza
        // I ako ono postoji dodajemo ga u listu
        foreach (var var in delta.Values)
        foreach (var pom in var)
            if (!nodes.Contains(pom))
                nodes.Add(pom);

        /*  foreach (var var in nodes)
        {
            Console.WriteLine(var);
        } */

        // Kreiramo matricu susjednosti te je popunjavamo na nacin da
        // Ukoliko imamo epsilon prelaz izmedju stanja unosimo 1 u matricu susjednosti
        // Inace unosimo 0
        ms = new int[nodes.Count, nodes.Count];
        for (var i = 0; i < nodes.Count; i++)
        for (var j = 0; j < nodes.Count; j++)
            if (delta.ContainsKey((nodes[i].ToString(), '$')))
            {
                if (delta[(nodes[i].ToString(), '$')].Contains(nodes[j].ToString()))
                    ms[i, j] = 1;
                else
                    ms[i, j] = 0;
            }

        /*    for (int i = 0; i < nodes.Count; i++)
        {
            Console.WriteLine("");
            for (int j = 0; j < nodes.Count; j++)
                Console.Write(ms[i, j] + " ");
        } */

        start = nodes.IndexOf(startState);
        visited = new bool[nodes.Count];
        eStates = new HashSet<string>();
    }

    // DFS obilazak
    public void dfs(int start)
    {
        // Dodajemo u set trenutno stanje
        eStates.Add(nodes[start].ToString());
        //  Console.WriteLine(nodes[start]);

        // Postavljamo trenutno stanje kao posjeceno
        visited[start] = true;

        // Za svaki cvor grafa
        for (var i = 0; i < nodes.Count; i++) // Ukoliko imamo epsilon prelaz ( 1 u matrici susjednosti)
            // i covr nije vec posjecen
            if (ms[start, i] == 1 && !visited[i])
                dfs(i);
    }

    public void setStart(string state)
    {
        start = nodes.IndexOf(state);
    }

    public HashSet<string> getEStates()
    {
        return eStates;
    }
}