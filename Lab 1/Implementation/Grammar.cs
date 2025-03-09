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

        // Addition to Grammar.cs
        public string ClassifyGrammar()
        {
            // Type 0: Unrestricted Grammar - Default
            // Type 1: Context-Sensitive Grammar
            // Type 2: Context-Free Grammar
            // Type 3: Regular Grammar (Right or Left Linear)

            bool isType3 = true;
            bool isRightLinear = true;
            bool isLeftLinear = true;
            bool isType2 = true;
            
            foreach (var rule in P)
            {
                foreach (var production in rule.Value)
                {
                    // Check for Type 3 (Regular Grammar)
                    // Right-linear: A → aB or A → a
                    // Left-linear: A → Ba or A → a
                    
                    if (production.Length > 2)
                    {
                        isRightLinear = false;
                        isLeftLinear = false;
                    }
                    else if (production.Length == 2)
                    {
                        // Check if the first symbol is terminal and second is non-terminal (Right-linear)
                        bool firstIsTerminal = V_T.Contains(production[0]);
                        bool secondIsNonTerminal = V_N.Contains(production[1]);
                        
                        if (!(firstIsTerminal && secondIsNonTerminal))
                        {
                            isRightLinear = false;
                        }
                        
                        // Check if the first symbol is non-terminal and second is terminal (Left-linear)
                        bool firstIsNonTerminal = V_N.Contains(production[0]);
                        bool secondIsTerminal = V_T.Contains(production[1]);
                        
                        if (!(firstIsNonTerminal && secondIsTerminal))
                        {
                            isLeftLinear = false;
                        }
                    }
                    
                    // If neither right-linear nor left-linear, it's not Type 3
                    if (!isRightLinear && !isLeftLinear)
                    {
                        isType3 = false;
                    }
                    
                    // Check for Type 2 (Context-Free Grammar)
                    // All productions must be of the form A → α where A is a single non-terminal
                    // This is already satisfied by our grammar representation
                }
            }
            
            if (isType3)
            {
                if (isRightLinear)
                    return "Type 3: Regular Grammar (Right-Linear)";
                else if (isLeftLinear)
                    return "Type 3: Regular Grammar (Left-Linear)";
                else
                    return "Type 3: Regular Grammar";
            }
            else if (isType2)
            {
                return "Type 2: Context-Free Grammar";
            }
            
            // For now, we'll assume we don't have Type 0 or Type 1 grammars
            return "Type 2: Context-Free Grammar";
        }
    }
}
