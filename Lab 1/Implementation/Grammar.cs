using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
        public int GetChomskyType()
        {
            if (IsType3()) return 3;
            if (IsType2()) return 2;
            if (IsType1()) return 1;
            return 0;
        }
        private bool IsType3()
        {
            // Regular Grammar: All productions are of the form A → a or A → aB
            foreach (var rule in P)
            {
                foreach (var production in rule.Value)
                {
                    bool singleTerminal = production.Length == 1 && V_T.Contains(production[0]);
                    bool terminalNonTerminal = production.Length == 2 && V_T.Contains(production[0]) && V_N.Contains(production[1]);
                    if (!(singleTerminal || terminalNonTerminal)) return false;
                }
            }
            return true;
        }
        private bool IsType2()
        {
            // Context-Free Grammar: All productions are of the form A → α where α is any string of terminals and non-terminals
            foreach (var rule in P)
            {
                char nonTerminal = rule.Key;
                if (!V_N.Contains(nonTerminal)) return false;
            }
            return true;
        }
        private bool IsType1()
        {
            // Context-Sensitive Grammar: All productions are of the form αAβ → αγβ where γ is not empty
            foreach (var rule in P)
            {
                foreach (var production in rule.Value)
                {
                    if (production.Length == 0 && rule.Key == S[0])
                    {
                        // Check if S appears on the right side of any production
                        foreach (var r in P) { foreach (var p in r.Value) { if (p.Contains(S[0])) return false; } }
                    }
                    if (production.Length == 0) return false;
                }
            }
            return true;
        }

        public string GetChomskyTypeDescription()
        {
            int type = GetChomskyType();
            return type switch
            {
                0 => "Type 0: Unrestricted Grammar",
                1 => "Type 1: Context-Sensitive Grammar",
                2 => "Type 2: Context-Free Grammar",
                3 => "Type 3: Regular Grammar"
            };
        }
    }
}