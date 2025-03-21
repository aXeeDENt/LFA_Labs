using System;
using System.Collections.Generic;

namespace LFA_Lab3.Lexer
{
    public enum TokenType
    {
        // Keywords
        tok_Character, tok_Inventory, tok_Quest, tok_Dialogue,

        // Character Identifiers 
        tok_name, tok_lvl, tok_exp, tok_race, tok_magic_type, tok_second_role, tok_role, tok_intelligence, tok_strength, tok_agility, tok_health_points,

        // Inventory Identifiers
        tok_type, tok_item, tok_damage_type, tok_status, tok_heal, tok_protection_type,

        // Dialogue Identifiers
        tok_character_lines, tok_hello_line, tok_goodbye_line, tok_quest_accepted, tok_quest_declined, tok_quest_to_accept, tok_after_quest_accepted, tok_after_quest_declined,
        // Quest Identifiers
        tok_gold, tok_description, tok_briefly, tok_reward, 
        // Literals
        tok_str, tok_num, 

        // Symbols
        tok_left_brace, tok_right_brace, tok_right_arrow, tok_right_bracket, tok_left_bracket, tok_comma, tok_or, tok_plus, tok_minus,
        
        // Special 
        tok_end, tok_invalid
    }
}