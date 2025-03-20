using System;
using System.Collections.Generic;

namespace LFA_Lab3.Lexer
{
    public enum TokenType
    {
        // Keywords
        tok_Character,

        // Identifiers 
        tok_name, tok_lvl, tok_exp, tok_race, tok_role,

        // Literals
        tok_str, tok_num, 

        // Symbols
        tok_left_brace, tok_right_brace, 
        
        // Special 
        tok_end, tok_invalid
    }
}