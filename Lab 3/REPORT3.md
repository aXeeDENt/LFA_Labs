# Topic: Lexer & Scanner

### Course: Formal Languages & Finite Automata
### Author: Tatrintev Denis (FAF-232)

----

## Theory
The term lexer comes from lexical analysis which, in turn, represents the process of extracting lexical tokens from a string of characters. There are several alternative names for the mechanism called lexer, for example tokenizer or scanner. The lexical analysis is one of the first stages used in a compiler/interpreter when dealing with programming, markup or other types of languages.     The tokens are identified based on some rules of the language and the products that the lexer gives are called lexemes. So basically the lexer is a stream of lexemes. Now in case it is not clear what's the difference between lexemes and tokens, there is a big one. The lexeme is just the byproduct of splitting based on delimiters, for example spaces, but the tokens give names or categories to each lexeme. So the tokens don't retain necessarily the actual value of the lexeme, but rather the type of it and maybe some metadata.

A scanner (or `“lexer”`) takes in the linear stream of characters and chunks them together into a series of something more
akin to `“words”`. In programming languages, each of these words is called a token. Some tokens are single characters, like (
and `,.` Others may be several characters long, like numbers (`123`), string literals (`"hi!"`), and identifiers (`min`).

When it comes to implementing a language, the first thing needed is the ability to process a text file and recognize what it
says. The traditional way to do this is to use a `“lexer”` (aka `‘scanner’`) to break the input up into `“tokens”`. Each token
returned by the lexer includes a token code and potentially some metadata (e.g. the numeric value of a number).



## Objectives:
1. Understand what lexical analysis [1] is.
2. Get familiar with the inner workings of a lexer/scanner/tokenizer.
3. Implement a sample lexer and show how it works.
> Note: Just because too many students were showing me the same idea of lexer for a calculator, I've decided to specify requirements for such case. Try to make it at least a little more complex. Like, being able to pass integers and floats, also to be able to perform trigonometric operations (cos and sin). But it does not mean that you need to do the calculator, you can pick anything interesting you want

## Implementation description
* Here is presented the Code part from `TokenType.cs`, an enum, where are located all tokens that could be used for Lexer. Here are 4 Keywords, 11 identifiers for `Character` Keyword, 8 identifiers for `Dialogue` Keyword, 6 identifiers for `Inventory` Keyword, 4 identifiers for `Quest` keyword, 2 literals, 9 for symbols and 2 special words (46 overall). Each of them show the specific parameter in Lexer.

```cs
public enum TokenType
    {
        // Keywords
        tok_Character, tok_Inventory, tok_Quest, tok_Dialogue,
        ...
    }
```

* I created the `Token` class to get the tokens and added there 4 parameters, tokentype from the enum I described above, value that the token has exactly, lu=ine and column that outline the position of the token. here are presented the constructor of this class. It also has a ToString() overriden method.  

```cs
public Token(TokenType type, string value, int line = 0, int column = 0)
{
    Type = type;
    Value = value;
    Line = line;
    Column = column;
}
```

* Next Step was creating the most important `Lexer.cs` class. Here I outlined all cases that can be met in input. Keywords that can be started both with lower and uppercase, identifiers, symbols, strings and numbers. I also added methods that ignore the whitespaces and then verify the next token exists or not.

```cs
private Token Number_Type()
{
    int begin = Position;
    while (Has_Next_Token() && char.IsDigit(Input[Position])) { Position++; Column++; }
    string value = Input.Substring(begin, Position - begin);
    return new Token(TokenType.tok_num, value, Row, Column - value.Length);
}
```
* In `Program3.cs` I outputted the case which user wants to check (only Dialogue, only Character, only Quest or Combined). Then program gets the path and outputs the result into new `output.txt` file.

```cs
switch(choice)
{
    case "1": filepath = "Samples\\character.txt"; break;
    case "2": filepath = "Samples\\quest.txt"; break;
    case "3": filepath = "Samples\\dialogue.txt"; break;
    case "4": filepath = "Samples\\combined.txt"; break;
    default: filepath = "Samples\\combined.txt"; break;
}
```
## Input Example
```powershell
Quest {
    name "Village Savior"
    description "Far away from our Kingdom is located a village named OIIA. Dear warrior, guild wants you to get there and to settle up the situation with Goblins!"
    briefly "Kill the Goblins in OIIA village"
    reward {
        Gold 341
        STR | INT | AGI + 2
        item "Potion" > name "LVL UP"
        quest {
            name "Lonely Lady"
            status "Available"
        }
    }
    status "Available and Accepted"
}
```
## Result Example
```txt
tok_Dialogue, Dialogue, Line: 1, Column 1
tok_left_brace, {, Line: 1, Column 10
tok_character_lines, characterLines, Line: 2, Column 5
tok_str, John, Line: 2, Column 22
tok_left_brace, {, Line: 3, Column 5
tok_hello_line, helloLine, Line: 4, Column 9
tok_str, Greetings, Line: 4, Column 21
tok_quest_accepted, questAccepted, Line: 5, Column 9
tok_str, I will do that for ya, pal!, Line: 5, Column 25
tok_quest_declined, questDeclined, Line: 6, Column 9
tok_str, I can't do that, Line: 6, Column 25
tok_goodbye_line, goodbyeLine, Line: 7, Column 9
tok_str, Farewell, Line: 7, Column 23
tok_right_brace, }, Line: 8, Column 5
tok_character_lines, characterLines, Line: 9, Column 5
tok_str, Young Lady, Line: 9, Column 22
tok_left_brace, {, Line: 10, Column 5
tok_hello_line, helloLine, Line: 11, Column 9
tok_str, Happy to see you again, Line: 11, Column 21
tok_quest_to_accept, questToAccept, Line: 12, Column 9
tok_str, I want you to help me, Line: 12, Column 25
tok_after_quest_accepted, afterQuestAccepted, Line: 13, Column 9
tok_str, I'm so happy!, Line: 13, Column 30
tok_after_quest_declined, afterQuestDeclined, Line: 14, Column 9
tok_str, Such a pity, Line: 14, Column 30
tok_goodbye_line, goodbyeLine, Line: 15, Column 9
tok_str, Have a good day, Line: 15, Column 23
tok_right_brace, }, Line: 16, Column 5
tok_right_brace, }, Line: 17, Column 1

```

## Conclusion
Here’s a conclusion for your lab report:  

---

## Conclusion  
In this lab, we explored the fundamental concepts of lexical analysis and implemented a custom lexer to tokenize structured input. The lexer was designed to recognize keywords, identifiers, symbols, and literals, providing categorized tokens as output. By implementing a `TokenType` enum, a `Token` class, and a `Lexer` class, we structured the lexical analysis process efficiently. The program successfully parsed input files containing structured data, demonstrating its ability to process and categorize tokens accurately.  

Through this implementation, we gained a deeper understanding of how lexers function as a critical component of compilers and interpreters. The project reinforced concepts such as tokenization, handling whitespace, and managing different token types while maintaining structured output. The final results confirm that the lexer can correctly process and identify tokens based on predefined rules, making it a useful foundation for further language processing tasks.
## References
1. Cretu's GitHub Repository: https://github.com/filpatterson/DSL_laboratory_works/tree/master/1_RegularGrammars
2. LFA ELSE Course: https://else.fcim.utm.md/course/view.php?id=98
