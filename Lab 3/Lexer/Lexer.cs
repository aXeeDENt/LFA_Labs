using System;
using System.Collections.Generic;

namespace LFA_Lab3.Lexer
{
    public class Lexer
    {
        public string Input { get; set; }
        public int Position { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public Lexer (string input)
        {
            Input = input;
            Position = 0;
            Row = 1;
            Column = 1;
        }
        public bool Has_Next_Token() { return Position < Input.Length; }
        private void Skip_Whitespace()
        {
            while (Has_Next_Token() && char.IsWhiteSpace(Input[Position]))
            {
                if (Input[Position] == '\n')
                { Row++; Column = 1; }
                else { Column++; } 
                Position++;
            }
        }
        private Token Keyword_Identifier_Type()
        {
            int current_symbol = Position; 
            while (Has_Next_Token() && char.IsLetter(Input[Position])) { Position++; Column++; }
            string value = Input.Substring(current_symbol, Position - current_symbol);
            switch(value)
            {
                case "character": return new Token(TokenType.tok_Character, value, Row, Column - value.Length);
                case "name": return new Token(TokenType.tok_name, value, Row, Column - value.Length);
                case "LVL": return new Token(TokenType.tok_lvl, value, Row, Column - value.Length);
                case "EXP": return new Token(TokenType.tok_exp, value, Row, Column - value.Length);
                case "race": return new Token(TokenType.tok_race, value, Row, Column - value.Length);
                case "role": return new Token(TokenType.tok_role, value, Row, Column - value.Length);
                default: return new Token(TokenType.tok_invalid, value, Row, Column - value.Length);
            }
        }
        private Token Number_Type()
        {
            int begin = Position;
            while (Has_Next_Token() && char.IsDigit(Input[Position])) { Position++; Column++; }
            string value = Input.Substring(begin, Position - begin);
            return new Token(TokenType.tok_num, value, Row, Column - value.Length);
        }
        private Token String_Type()
        {
            Position++; Column++;
            int begin = Position;
            while (Has_Next_Token() && Input[Position] != '"') { Position++; Column++; }
            string value = Input.Substring(begin, Position - begin);
            Position++; Column++;
            return new Token(TokenType.tok_str, value, Row, Column - value.Length);
        }
        private Token Symbol_Type()
        {
            char current_symbol = Input[Position]; Position++; Column++; 
            switch(current_symbol)
            {
                case '{': return new Token(TokenType.tok_left_brace, "{", Row, Column - 1);
                case '}': return new Token(TokenType.tok_right_brace, "}", Row, Column - 1);
                default: return new Token(TokenType.tok_invalid, current_symbol.ToString(), Row, Column - 1);
            }
        }
        public Token Next_Token()
        {
           Skip_Whitespace();

           if (!Has_Next_Token()) { return new Token(TokenType.tok_end, "", Row, Column); }

           char current_char = Input[Position];

           if (char.IsLetter(current_char)) { return Keyword_Identifier_Type(); }
           if (current_char == '"') { return String_Type(); }
           if (char.IsDigit(current_char)) { return Number_Type(); }
           return Symbol_Type();
        }
        
    }
}