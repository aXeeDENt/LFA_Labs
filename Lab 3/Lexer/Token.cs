using System;
using System.Collections.Generic;

namespace LFA_Lab3.Lexer
{
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public int Line { get; }
        public int Position { get; }

        public Token(TokenType type, string value, int line = 0, int position = 0)
        {
            Type = type;
            Value = value;
            Line = line;
            Position = position;
        }

        public override string ToString()
        { return $"Token({Type}, {Value}, Line: {Line}, Position {Position})";}
    }
}