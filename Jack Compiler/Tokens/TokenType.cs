using System.Collections.Generic;
using System.IO;
using System.Linq;

public enum TokenType
{
  // keywords
  KEYWORD_CLASS,              // class
  KEYWORD_CONSTRUCTOR,        // constructor
  KEYWORD_FUNCTION,           // function
  KEYWORD_METHOD,             // method
  KEYWORD_FIELD,              // field
  KEYWORD_STATIC,             // static
  KEYWORD_VAR,                // var
  KEYWORD_INT,                // int
  KEYWORD_CHAR,               // char
  KEYWORD_BOOLEAN,            // boolean
  KEYWORD_VOID,               // void
  KEYWORD_TRUE,               // true
  KEYWORD_FALSE,              // false
  KEYWORD_NULL,               // null
  KEYWORD_THIS,               // this
  KEYWORD_LET,                // let
  KEYWORD_DO,                 // do
  KEYWORD_IF,                 // if
  KEYWORD_ELSE,               // else
  KEYWORD_WHILE,              // while
  KEYWORD_RETURN,             // return
                              // grouping operators
  SYMBOL_LEFT_PARENTHESIS,    // (
  SYMBOL_RIGHT_PARENTHESIS,   // )
  SYMBOL_LEFT_BRACE,    // {
  SYMBOL_RIGHT_BRACE,   // }
                        // arrays n shit
  SYMBOL_LEFT_SQUARE,         // [
  SYMBOL_RIGHT_SQUARE,        // ]
                              // inter expression operators
  SYMBOL_DOT,                 // .
  SYMBOL_COMMA,               // , 
  SYMBOL_SEMI_COLON,          // ;
  SYMBOL_PLUS,                // +
  SYMBOL_MINUS,               // -
  SYMBOL_ASTERISK,            // *
  SYMBOL_FORWARD_SLASH,       // /
                              // Comparison operators
  SYMBOL_AMPERSAND,           // &
  SYMBOL_PIPE,                // |
  SYMBOL_LESS_THAN,           // <
  SYMBOL_GREATER_THAN,        // >
  SYMBOL_EQUALITY,            // =
  SYMBOL_TILDE,               // ~

  INTEGER_CONSTANT,           // 32405
  STRING_CONSTANT,            // "foo bar some long string 39023902"

  IDENTIFIER,                 // var v
}

// var -> keyword_var
// string -> tokens