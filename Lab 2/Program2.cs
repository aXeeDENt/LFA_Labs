using System;
using System.Collections.Generic;

namespace LFA_Lab2
{
    // Import the classes from Lab 1 but adjust them to work in Lab 2
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

        public string generateString()
        {  
            Random rand = new Random();
            System.Text.StringBuilder word = new System.Text.StringBuilder(S.ToString());
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
        
        // New method for Lab 2
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
        
        // New methods for Lab 2
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

        public bool IsDeterministic()
        {
            // First check: no state can have multiple transitions for the same input symbol
            Dictionary<(string, char), int> transitionCount = new Dictionary<(string, char), int>();
            
            foreach (var transition in delta)
            {
                (string state, char symbol) key = transition.Key;
                
                if (transitionCount.ContainsKey(key))
                {
                    transitionCount[key]++;
                }
                else
                {
                    transitionCount[key] = 1;
                }
            }
            
            // If any state has multiple transitions for the same input, it's non-deterministic
            foreach (var count in transitionCount)
            {
                if (count.Value > 1)
                {
                    return false;
                }
            }
            
            // Second check: all states must have transitions for all input symbols
            foreach (string state in Q)
            {
                foreach (char symbol in Sigma)
                {
                    if (!delta.ContainsKey((state, symbol)))
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }

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

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lab 2: Finite Automata and Grammars");
            
            // Create a sample finite automaton
            HashSet<string> states = new HashSet<string> { "q0", "q1", "q2", "q3" };
            HashSet<char> alphabet = new HashSet<char> { 'a', 'b' };
            Dictionary<(string, char), string> transitions = new Dictionary<(string, char), string>
            {
                [("q0", 'a')] = "q1",
                [("q0", 'b')] = "q2",
                [("q1", 'a')] = "q1",
                [("q1", 'b')] = "q3",
                [("q2", 'a')] = "q1",
                [("q2", 'b')] = "q2",
                [("q3", 'a')] = "q1",
                [("q3", 'b')] = "q2"
            };
            string initialState = "q0";
            HashSet<string> finalStates = new HashSet<string> { "q3" };
            
            FiniteAutomaton fa = new FiniteAutomaton(states, alphabet, transitions, initialState, finalStates);
            
            // Test if the FA is deterministic
            Console.WriteLine("Is the FA deterministic? " + fa.IsDeterministic());
            
            // Convert FA to regular grammar
            Grammar grammar = fa.ConvertToGrammar();
            
            // Print the grammar
            Console.WriteLine("\nRegular Grammar derived from FA:");
            Console.WriteLine("Non-terminals: " + string.Join(", ", grammar.V_N));
            Console.WriteLine("Terminals: " + string.Join(", ", grammar.V_T));
            Console.WriteLine("Start symbol: " + grammar.S);
            Console.WriteLine("Productions:");
            foreach (var production in grammar.P)
            {
                Console.WriteLine($"{production.Key} -> {string.Join(" | ", production.Value)}");
            }
            
            // Classify the grammar
            Console.WriteLine("\nGrammar Classification: " + grammar.ClassifyGrammar());
            
            // Convert NDFA to DFA if necessary
            if (!fa.IsDeterministic())
            {
                Console.WriteLine("\nConverting NDFA to DFA...");
                FiniteAutomaton dfa = fa.ConvertToDFA();
                Console.WriteLine("DFA has " + dfa.Q.Count + " states");
                
                // Save the DFA as a DOT file
                dfa.SaveDotToFile("dfa.dot");
            }
            else
            {
                // Save the FA as a DOT file
                fa.SaveDotToFile("fa.dot");
            }
            
            // Test string recognition
            Console.WriteLine("\nEnter a string to test:");
            string input = Console.ReadLine();
            Console.WriteLine("String belongs to language: " + fa.stringBelongToLanguage(input));
        }
    }
}