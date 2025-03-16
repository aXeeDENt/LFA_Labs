using System;
using System.Collections.Generic;

namespace LFA_Lab1
{
    public class FiniteAutomaton
    {
        public HashSet<string> Q { get; private set; } 
        public HashSet<char> Sigma { get; private set; } 
        public Dictionary<(string, char), string> delta { get; private set; } 
        public string q0 { get; private set; }
        public HashSet<string> F { get; private set; }

        public FiniteAutomaton(HashSet<string> q, HashSet<char> sigma, Dictionary<(string, char), string> Delta, string Q0, HashSet<string> f)
        {
            Q = q;
            Sigma = sigma;
            delta = Delta;
            q0 = Q0;
            F = f;
        }

        public static FiniteAutomaton ConvertFromGrammar(Grammar grammar)
        {
            HashSet<string> Q = new HashSet<string>(); 
            HashSet<char> Sigma = new HashSet<char>(); 
            Dictionary<(string, char), string> delta = new Dictionary<(string, char), string>(); 
            HashSet<string> F = new HashSet<string>(); 
            string q0 = grammar.S; 

            foreach (char non_terminal in grammar.V_N) Q.Add(non_terminal.ToString());
            foreach (char terminal in grammar.V_T) Sigma.Add(terminal);
            foreach (var rule in grammar.P)
            {
                string state = rule.Key.ToString();
                foreach (string production in rule.Value)
                {
                    char terminal = production[0];
                    if (production.Length > 1)
                    {
                        string nextState = production[1].ToString();
                        delta[(state, terminal)] = nextState;
                    }
                    else
                    {
                        delta[(state, terminal)] = state;
                        F.Add(state);
                    }
                }
            }
            return new FiniteAutomaton(Q, Sigma, delta, q0, F);
        }

        public bool stringBelongToLanguage(string inputString)
        {
            string currentState = q0;
            foreach (char symbol in inputString)
            {
                if (delta.ContainsKey((currentState, symbol))) { currentState = delta[(currentState, symbol)]; }
                else { return false; }
            }
            return F.Contains(currentState); 
        }
    }
}
