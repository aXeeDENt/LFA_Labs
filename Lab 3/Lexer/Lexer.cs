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
                // For Keywords
                case "Character": case "character": 
                return new Token(TokenType.tok_Character, value, Row, Column - value.Length);
                case "Inventory": case "inventory": 
                return new Token(TokenType.tok_Inventory, value, Row, Column - value.Length);
                case "Dialogue": case "dialogue": 
                return new Token(TokenType.tok_Dialogue, value, Row, Column - value.Length);
                case "Quest": case "quest": 
                return new Token(TokenType.tok_Quest, value, Row, Column - value.Length);
                // For Character
                case "name": return new Token(TokenType.tok_name, value, Row, Column - value.Length);
                case "INT": return new Token(TokenType.tok_intelligence, value, Row, Column - value.Length);
                case "STR": return new Token(TokenType.tok_strength, value, Row, Column - value.Length);
                case "AGI": return new Token(TokenType.tok_agility, value, Row, Column - value.Length);
                case "HP": return new Token(TokenType.tok_health_points, value, Row, Column - value.Length);
                case "LVL": return new Token(TokenType.tok_lvl, value, Row, Column - value.Length);
                case "EXP": return new Token(TokenType.tok_exp, value, Row, Column - value.Length);
                case "race": return new Token(TokenType.tok_race, value, Row, Column - value.Length);
                case "role": return new Token(TokenType.tok_role, value, Row, Column - value.Length);
                case "magicType": return new Token(TokenType.tok_magic_type, value, Row, Column - value.Length);
                case "secondRole": return new Token(TokenType.tok_second_role, value, Row, Column - value.Length);
                case "Gold": return new Token(TokenType.tok_gold, value, Row, Column - value.Length);
                // For Inventory
                case "type": return new Token(TokenType.tok_type, value, Row, Column - value.Length);
                case "item": return new Token(TokenType.tok_item, value, Row, Column - value.Length);
                case "damageType": return new Token(TokenType.tok_damage_type, value, Row, Column - value.Length);
                case "status": return new Token(TokenType.tok_status, value, Row, Column - value.Length);
                case "protectionType": return new Token(TokenType.tok_protection_type, value, Row, Column - value.Length);
                case "heal": return new Token(TokenType.tok_heal, value, Row, Column - value.Length);
                // For Dialogue
                case "characterLines": return new Token(TokenType.tok_character_lines, value, Row, Column - value.Length);
                case "helloLine": return new Token(TokenType.tok_hello_line, value, Row, Column - value.Length);
                case "goodbyeLine": return new Token(TokenType.tok_goodbye_line, value, Row, Column - value.Length);
                case "questAccepted": return new Token(TokenType.tok_quest_accepted, value, Row, Column - value.Length);
                case "questDeclined": return new Token(TokenType.tok_quest_declined, value, Row, Column - value.Length);
                case "questToAccept": return new Token(TokenType.tok_quest_to_accept, value, Row, Column - value.Length);
                case "afterQuestAccepted": return new Token(TokenType.tok_after_quest_accepted, value, Row, Column - value.Length);
                case "afterQuestDeclined": return new Token(TokenType.tok_after_quest_declined, value, Row, Column - value.Length);
                // For Quest
                case "description": return new Token(TokenType.tok_description, value, Row, Column - value.Length);
                case "briefly": return new Token(TokenType.tok_briefly, value, Row, Column - value.Length);
                case "reward": return new Token(TokenType.tok_reward, value, Row, Column - value.Length);
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
                // Character (Basic) Symbols
                case '{': return new Token(TokenType.tok_left_brace, "{", Row, Column - 1);
                case '}': return new Token(TokenType.tok_right_brace, "}", Row, Column - 1);
                // Inventory Symbols
                case '>': return new Token(TokenType.tok_right_arrow, ">", Row, Column - 1);
                case '[': return new Token(TokenType.tok_left_bracket, "[", Row, Column - 1);
                case ']': return new Token(TokenType.tok_right_bracket, "]", Row, Column - 1);
                case ',': return new Token(TokenType.tok_comma, ",", Row, Column - 1);
                // Quest Symbols
                case '|': return new Token(TokenType.tok_or, "|", Row, Column - 1);
                case '+': return new Token(TokenType.tok_plus, "+", Row, Column - 1);
                case '-': return new Token(TokenType.tok_minus, "-", Row, Column - 1);
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