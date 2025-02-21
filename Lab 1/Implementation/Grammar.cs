using System;
using System.Collections.Generic;
using System.Text;

namespace LFA_Lab1
{
    public class Grammar
    {
        public HashSet<char> V_N = new HashSet<char> {'S', 'A', 'B', 'C'};
        public HashSet<char> V_T = new HashSet<char> {'a', 'b', 'c', 'd'};
        public string S = "S";
        public Dictionary<char,List<string>> P = new Dictionary<char, List<string>>
        {
            { 'S', new List<string> { "dA" } },
            { 'A', new List<string> { "aB", "b" } },
            { 'B', new List<string> { "bC", "d" } },
            { 'C', new List<string> { "cB", "aA" } }
        };
        // public Grammar(HashSet<char> v_N, HashSet<char> v_T, char s, Dictionary<char,List<string>> p) { this.S = s; this.V_N = v_N; this.V_T = v_T; this.P = p; }

        public string generateString()
        {  
            Random rand = new Random();
            StringBuilder word = new StringBuilder(S.ToString());
            while (true)
            {
                bool replaced = false;
                for (int i=0; i<word.Length; i++)
                {
                    char currentChar = word[i];
                    if (P.ContainsKey(currentChar))
                    {
                        List<string> productions = P[currentChar];
                        string chosenProduction = productions[rand.Next(productions.Count)];
                        word.Remove(i, 1);
                        word.Insert(i, chosenProduction);
                        replaced = true;
                        break;
                    }
                }
                if (!replaced) break;
            }
            return word.ToString();
        }
    }
}
