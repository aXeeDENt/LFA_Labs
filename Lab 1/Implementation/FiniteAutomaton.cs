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
        // Addition to FiniteAutomaton.cs
        public Grammar ConvertToGrammar()
        {
            // Create a new Grammar instance
            Grammar grammar = new Grammar();
            
            // Clear the default values
            grammar.V_N = new HashSet<char>();
            grammar.V_T = new HashSet<char>();
            grammar.P = new Dictionary<char, List<string>>();
            
            // Convert states to non-terminals
            foreach (string state in Q)
            {
                if (state.Length == 1)
                    grammar.V_N.Add(state[0]);
            }
            
            // Add terminals
            foreach (char symbol in Sigma)
            {
                grammar.V_T.Add(symbol);
            }
            
            // Set the start symbol
            grammar.S = q0.Length == 1 ? q0 : "S";
            
            // Create productions
            foreach (string state in Q)
            {
                if (state.Length == 1)
                {
                    char nonTerminal = state[0];
                    List<string> productions = new List<string>();
                    
                    // Add transitions
                    foreach (var transition in delta)
                    {
                        if (transition.Key.Item1 == state)
                        {
                            char terminal = transition.Key.Item2;
                            string nextState = transition.Value;
                            
                            if (F.Contains(nextState))
                            {
                                // If it leads to a final state, add a production with just the terminal
                                productions.Add(terminal.ToString());
                            }
                            else if (nextState.Length == 1)
                            {
                                // Add a production with terminal + next state
                                productions.Add(terminal.ToString() + nextState);
                            }
                        }
                    }
                    
                    if (productions.Count > 0)
                    {
                        grammar.P[nonTerminal] = productions;
                    }
                }
            }
            
            return grammar;
        }

        // Check if FA is deterministic
        public bool IsDeterministic()
        {
            // A DFA has exactly one transition for each state and input symbol
            HashSet<(string, char)> transitions = new HashSet<(string, char)>();
            
            foreach (var transition in delta)
            {
                if (transitions.Contains(transition.Key))
                {
                    return false;  // Found multiple transitions for the same state and input
                }
                
                transitions.Add(transition.Key);
            }
            
            // Check if all states have transitions for all input symbols
            foreach (string state in Q)
            {
                foreach (char symbol in Sigma)
                {
                    if (!transitions.Contains((state, symbol)))
                    {
                        return false;  // Missing transition
                    }
                }
            }
            
            return true;
        }

        // Convert NDFA to DFA
        public FiniteAutomaton ConvertToDFA()
        {
            if (IsDeterministic())
            {
                return this;  // Already a DFA
            }
            
            // Implementation of subset construction algorithm
            HashSet<string> newQ = new HashSet<string>();
            Dictionary<(string, char), string> newDelta = new Dictionary<(string, char), string>();
            HashSet<string> newF = new HashSet<string>();
            
            // Start with the initial state
            HashSet<string> initialStateSet = new HashSet<string> { q0 };
            string initialState = string.Join(",", initialStateSet);
            newQ.Add(initialState);
            
            // Queue for processing new states
            Queue<HashSet<string>> stateQueue = new Queue<HashSet<string>>();
            stateQueue.Enqueue(initialStateSet);
            
            // Process all reachable states
            while (stateQueue.Count > 0)
            {
                HashSet<string> currentStateSet = stateQueue.Dequeue();
                string currentState = string.Join(",", currentStateSet);
                
                // Check if currentState is final
                bool isFinal = false;
                foreach (string state in currentStateSet)
                {
                    if (F.Contains(state))
                    {
                        isFinal = true;
                        break;
                    }
                }
                
                if (isFinal)
                {
                    newF.Add(currentState);
                }
                
                // Process transitions for each input symbol
                foreach (char symbol in Sigma)
                {
                    HashSet<string> nextStateSet = new HashSet<string>();
                    
                    // Find all possible next states
                    foreach (string state in currentStateSet)
                    {
                        if (delta.ContainsKey((state, symbol)))
                        {
                            nextStateSet.Add(delta[(state, symbol)]);
                        }
                    }
                    
                    // Skip if no transitions
                    if (nextStateSet.Count == 0)
                    {
                        continue;
                    }
                    
                    // Create new state name
                    string nextState = string.Join(",", nextStateSet);
                    
                    // Add transition
                    newDelta[(currentState, symbol)] = nextState;
                    
                    // Add new state if needed
                    if (!newQ.Contains(nextState))
                    {
                        newQ.Add(nextState);
                        stateQueue.Enqueue(nextStateSet);
                    }
                }
            }
            
            return new FiniteAutomaton(newQ, Sigma, newDelta, initialState, newF);
        }

        // Add to FiniteAutomaton.cs
        public void RenderGraph(string outputPath = "automaton.png")
        {
            // This requires adding the ScottPlot NuGet package
            var plt = new ScottPlot.Plot(600, 400);
            
            // Dictionary to assign coordinates to each state
            Dictionary<string, (double x, double y)> stateCoordinates = new Dictionary<string, (double x, double y)>();
            
            // Arrange states in a circle
            int stateCount = Q.Count;
            double radius = 1.0;
            double centerX = 0.0;
            double centerY = 0.0;
            
            int i = 0;
            foreach (string state in Q)
            {
                double angle = 2 * Math.PI * i / stateCount;
                double x = centerX + radius * Math.Cos(angle);
                double y = centerY + radius * Math.Sin(angle);
                
                stateCoordinates[state] = (x, y);
                
                // Add state circle
                var circle = plt.AddCircle(x, y, 0.1);
                
                // Highlight initial and final states
                // if (state == q0)
                // {
                //     circle.lineWidth = 2;
                // }
                
                if (F.Contains(state))
                {
                    var innerCircle = plt.AddCircle(x, y, 0.08);
                    // innerCircle.lineWidth = 2;
                }
                
                // Add state label
                plt.AddText(state, x, y, size: 12);
                
                i++;
            }
            
            // Add transitions
            foreach (var transition in delta)
            {
                string fromState = transition.Key.Item1;
                char symbol = transition.Key.Item2;
                string toState = transition.Value;
                
                (double x1, double y1) = stateCoordinates[fromState];
                (double x2, double y2) = stateCoordinates[toState];
                
                // Draw arrow
                plt.AddArrow(x1, y1, x2, y2, lineWidth: 1);
                
                // Add transition label (symbol)
                double labelX = (x1 + x2) / 2;
                double labelY = (y1 + y2) / 2;
                plt.AddText(symbol.ToString(), labelX, labelY, size: 12);
            }
            
            // Save the plot to a file
            plt.SaveFig(outputPath);
            Console.WriteLine($"Automaton graph saved to {outputPath}");
        }

        // Add to FiniteAutomaton.cs (replaces the ScottPlot version)
        // Add to FiniteAutomaton.cs (replaces the ScottPlot version)
        public string GenerateDotGraph()
        {
            // Create a DOT representation of the automaton
            System.Text.StringBuilder dot = new System.Text.StringBuilder();
            dot.AppendLine("digraph finite_automaton {");
            dot.AppendLine("    rankdir=LR;");
            
            // Add node styles
            dot.AppendLine("    node [shape = circle];");
            
            // Mark final states with double circles
            if (F.Count > 0)
            {
                dot.Append("    node [shape = doublecircle]; ");
                foreach (string state in F)
                {
                    dot.Append($"\"{state}\" ");
                }
                dot.AppendLine(";");
                dot.AppendLine("    node [shape = circle];");
            }
            
            // Add a start state
            dot.AppendLine("    start [shape = point];");
            dot.AppendLine($"    start -> \"{q0}\";");
            
            // Add transitions
            foreach (var transition in delta)
            {
                string fromState = transition.Key.Item1;
                char symbol = transition.Key.Item2;
                string toState = transition.Value;
                
                dot.AppendLine($"    \"{fromState}\" -> \"{toState}\" [label = \"{symbol}\"];");
            }
            
            dot.AppendLine("}");
            return dot.ToString();
        }

        public void SaveDotToFile(string filename = "automaton.dot")
        {
            string dotContent = GenerateDotGraph();
            System.IO.File.WriteAllText(filename, dotContent);
            Console.WriteLine($"DOT graph saved to {filename}");
            Console.WriteLine("You can visualize this using online tools like http://viz-js.com/ or Graphviz");
        }
    }
}
