# Intro to formal languages. Regular grammars. Finite Automata.

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
Variant 26:
VN={S, A, B, C},
VT={a, b, c, d}, 
P={ 
    S → dA     
    A → aB    
    B → bC   
    C → cB    
    B → d
    C → aA
    A → b
}
```

## Implementation description
* About 2-3 sentences to explain each piece of the implementation.

```
public static void main() 
{

}
```

## Results

## Conclusions 

## References
1. Cretu's GitHub Repository: https://github.com/filpatterson/DSL_laboratory_works/tree/master/1_RegularGrammars
2. LFA ELSE Course: https://else.fcim.utm.md/course/view.php?id=98
