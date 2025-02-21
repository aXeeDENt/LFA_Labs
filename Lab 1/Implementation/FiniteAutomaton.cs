using System;
using System.Collections.Generic;

namespace LFA_Lab1
{
    public class FiniteAutomaton
    {
        public HashSet<string> Q { get; private set; } 
        public HashSet<char> Sigma { get; private set; } 
        public Dictionary<(string, char), string> delta { get; private set; } // ?
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
            HashSet<string> Q = new HashSet<string>(); // Set of states
            HashSet<char> Sigma = new HashSet<char>(); // Input alphabet
            Dictionary<(string, char), string> delta = new Dictionary<(string, char), string>(); // Transition function
            HashSet<string> F = new HashSet<string>(); // Final states
            string q0 = grammar.S; // Start state

            foreach (char non_terminal in grammar.V_N)
                Q.Add(non_terminal.ToString());

            foreach (char terminal in grammar.V_T)
                Sigma.Add(terminal);

            // Process rules
            foreach (var rule in grammar.P)
            {
                string state = rule.Key.ToString(); // Convert non-terminal to state

                foreach (string production in rule.Value)
                {
                    char terminal = production[0]; // First character is the terminal

                    if (production.Length > 1)
                    {
                        string nextState = production[1].ToString();
                        delta[(state, terminal)] = nextState;
                    }
                    else
                    {
                        // If production is a single terminal (e.g., A → b), it means we accept `b` in `A`
                        delta[(state, terminal)] = state; // Loop to itself
                        F.Add(state); // Mark as final state
                    }
                }
            }

            return new FiniteAutomaton(Q, Sigma, delta, q0, F);
        }


        public bool stringBelongToLanguage(string inputString)
        {
            string currentState = q0; // Start from initial state

            foreach (char symbol in inputString)
            {
                if (delta.ContainsKey((currentState, symbol)))
                {
                    currentState = delta[(currentState, symbol)];
                }
                else
                {
                    return false; // If no transition exists, reject the string
                }
            }

            return F.Contains(currentState); // Accept if final state is reached
        }

    }
}
