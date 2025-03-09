# Determinism in Finite Automata. Conversion from NDFA 2 DFA. Chomsky Hierarchy.

### Course: Formal Languages & Finite Automata
### Author: Tatrintev Denis (FAF-232)

----

## Theory
**An alphabet** is a finite, nonempty set of symbols. By convention it is used the symbol ∑ for an alphabet. 

**A language** is:
    1. a set of strings from some alphabet (finite or infinite);
    2. any subset L of Σ*

**A grammar** G is an ordered quadruple G = ($V_N, V_T, P, S$)
where:
- $V_N$ - is a finite set of non-terminal symbols;
- $V_T$ - is a finite set of terminal symbols;
- S is a start symbol;
- P – is a finite set of productions of rules.

**Finite Automata** (**FA**) are like simple machines that follow a set of rules to process a sequence of inputs (like letters or numbers) and decide if they are valid or not.

## Objectives:
1. Discover what a language is and what it needs to have in order to be considered a formal one;
2. Provide the initial setup for the evolving project that you will work on during this semester. You can deal with each laboratory work as a separate task or project to demonstrate your understanding of the given themes, but you also can deal with labs as stages of making your own big solution, your own project. Do the following:
    -  Create GitHub repository to deal with storing and updating your project;
    - Choose a programming language. Pick one that will be easiest for dealing with your tasks, you need to learn how to solve the problem itself, not everything around the problem (like setting up the project, launching it correctly and etc.);
    - Store reports separately in a way to make verification of your work simpler (duh)
3. According to your variant number, get the grammar definition and do the following:
    - Implement a type/class for your grammar;
    - Add one function that would generate 5 valid strings from the language expressed by your given grammar;
    - Implement some functionality that would convert and object of type Grammar to one of type Finite Automaton;
    - For the Finite Automaton, please add a method that checks if an input string can be obtained via the state transition from it;

## My variant
``` 
Variant 26
Q = {q0,q1,q2,q3},
∑ = {a,b,c},
F = {q3},
δ(q0,a) = q1,
δ(q1,b) = q1,
δ(q1,a) = q2,
δ(q0,a) = q0,
δ(q2,c) = q3,
δ(q3,c) = q3 
```

## Implementation description
* For the 1st step, I implemented a new class `Grammar`. It contains the HashSets for Terminal and Non-terminal symbols, a string as an initial symbol and a Dictionary for rules. I did not used constructor for this lab, and if it will be needed for next labs, I will implement it

```cs
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
    }
```

* The I added a `generateString()` method, for generation a random string using the grammar I have. Here is the most important condition for this method. It uses a rand class to choose randomly any next step from rules P.

```cs
if (P.ContainsKey(currentChar))
{
    List<string> productions = P[currentChar];
    string chosenProduction = [rand.Next(productions.Count)];
    word.Remove(i, 1);
    word.Insert(i, chosenProduction);
    replaced = true;
    break;
}
```

* The next step was creating a `FiniteAutomaton` for converting a grammar object to finite automata object. It is very similar to `Grammar` class and the only difference is in the dictionary delta that uses a tuple. I also created a `ConvertFromGrammar` method in FA that uses pretty similar structure as `Grammar` class. Here is the constructor for FA:

``` cs
public FiniteAutomaton(HashSet<string> q, HashSet<char> sigma, Dictionary<(string, char), string> Delta, string Q0, HashSet<string> f)
{
    Q = q;
    Sigma = sigma;
    delta = Delta;
    q0 = Q0;
    F = f;
}
```

* Here is the pivotal line in method `stringBelongsToLanguage()` in FA to change the current states

```cs
if (delta.ContainsKey((currentState, symbol))) { currentState = delta[(currentState, symbol)]; }
```

* The final step was a `Program` class to output the results of the laboratory work. Here I add to a HashSet 5 different strings and output them using `foreach` loop and then output the result: does input string belongs to language from my variant or not

```cs
while(set_of_5.Count<5) { set_of_5.Add(grammar.generateString()); }
foreach(string s in set_of_5) { Console.WriteLine(s); }
//
Console.WriteLine($"Does {input} belong to language? " + fa.stringBelongToLanguage(input));

```


## Results
- Output 1:
```powershell
db
dabaad
dad
dabab
dabcbab
Enter any string: 
dabaabcd
Does dabaabcd belong to language? True
```

- Output 2:
```powershell
db
dad
dabcd
dabaabcd
dabaabcbcd
Enter any string:
grammar
Does grammar belong to language? False
```
## Conclusions 
In conclution I could add that it was interesting to get new skills and implement new techniques in this lab. I learned a lot about Grammars and Finite Automata until this day and I hope I will learn even more. I also learned how to work with HashSets and Dictionaries during this lab.
## References
1. Cretu's GitHub Repository: https://github.com/filpatterson/DSL_laboratory_works/tree/master/1_RegularGrammars
2. LFA ELSE Course: https://else.fcim.utm.md/course/view.php?id=98
