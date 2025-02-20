using System;
using System.Collections.Generic;
using LFA_Lab1;

public class Program
{
    public static void Main()
    {
        Grammar grammar = new Grammar();
        HashSet<string> set_of_5 = new HashSet<string>();
        while(set_of_5.Count<5) { set_of_5.Add(grammar.generateString()); }
        foreach(string s in set_of_5) { Console.WriteLine(s); }
    }
}