using System;
using System.Collections.Generic;

namespace LFA_Lab1
{
    public class FiniteAutomaton
    {
        public HashSet<char> Q { get; private set; } 
        public HashSet<char> Sigma { get; private set; } 
        public Dictionary<(string, char), string> delta { get; private set; } // ?
        public char q0 { get; private set; }
        public HashSet<string> F { get; private set; }

        public FiniteAutomaton(HashSet<char> q, HashSet<char> sigma, Dictionary<(string, char), string> Delta, char Q0, HashSet<string> f)
        {
            Q = q;
            Sigma = sigma;
            delta = Delta;
            q0 = Q0;
            F = f;
        }

        public static FiniteAutomaton ConvertFromGrammar(Grammar grammar)
        {
            HashSet<char> Q = new HashSet<char>(); 
            HashSet<char> Sigma = new HashSet<char>(); 
            Dictionary<(string, char), string> delta = new Dictionary<(string, char), string>();
            HashSet<string> F = new HashSet<string>();
            char q0 = grammar.S;

            foreach (char non_terminal in grammar.V_N) Q.Add(non_terminal);
            foreach (char terminal in grammar.V_T) Sigma.Add(terminal);

            // Build transition function
            foreach (var rule in grammar.P)
            {
                string state = rule.Key.ToString();
                foreach (string production in rule.Value)
                {
                    char terminal = production[0]; // First character is a terminal
                    if (production.Length > 1)
                    {
                        // Transition: (state, terminal) → next state
                        string nextState = production[1].ToString();
                        delta[(state, terminal)] = nextState;
                    }
                    else { F.Add(state); }
                }
            }
            return new FiniteAutomaton(Q, Sigma, delta, q0, F);
        }
    }
}
