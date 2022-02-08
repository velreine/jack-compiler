using System;
using System.Collections.Generic;
using System.Linq;

// string -> tokens
namespace Jack_Compiler.Tokens
{
    public class Lexer
    {
        public static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>() {
            {"class", TokenType.KEYWORD_CLASS},
            {"constructor", TokenType.KEYWORD_CONSTRUCTOR},
            {"function", TokenType.KEYWORD_FUNCTION},
            {"method", TokenType.KEYWORD_METHOD},
            {"field", TokenType.KEYWORD_FIELD},
            {"static", TokenType.KEYWORD_STATIC},
            {"var", TokenType.KEYWORD_VAR},
            {"int", TokenType.KEYWORD_INT},
            {"char", TokenType.KEYWORD_CHAR},
            {"boolean", TokenType.KEYWORD_BOOLEAN},
            {"void", TokenType.KEYWORD_VOID},
            {"true", TokenType.KEYWORD_TRUE},
            {"false", TokenType.KEYWORD_FALSE},
            {"null", TokenType.KEYWORD_NULL},
            {"this", TokenType.KEYWORD_THIS},
            {"let", TokenType.KEYWORD_LET},
            {"do", TokenType.KEYWORD_DO},
            {"if", TokenType.KEYWORD_IF},
            {"else", TokenType.KEYWORD_ELSE},
            {"while", TokenType.KEYWORD_WHILE},
            {"return", TokenType.KEYWORD_RETURN},
        };

        public static readonly Dictionary<TokenType, string> keywordsToStr = keywords.ToDictionary((i) => i.Value, (i) => i.Key);

        public static readonly Dictionary<char, TokenType> symbols = new Dictionary<char, TokenType>() {
            {'{', TokenType.SYMBOL_LEFT_BRACE},
            {'}', TokenType.SYMBOL_RIGHT_BRACE},
            {'(', TokenType.SYMBOL_LEFT_PARENTHESIS},
            {')', TokenType.SYMBOL_RIGHT_PARENTHESIS},
            {'[', TokenType.SYMBOL_LEFT_SQUARE},
            {']', TokenType.SYMBOL_RIGHT_SQUARE},
            {'.', TokenType.SYMBOL_DOT},
            {',', TokenType.SYMBOL_COMMA},
            {';', TokenType.SYMBOL_SEMI_COLON},
            {'+', TokenType.SYMBOL_PLUS},
            {'-', TokenType.SYMBOL_MINUS},
            {'*', TokenType.SYMBOL_ASTERISK},
            {'/', TokenType.SYMBOL_FORWARD_SLASH},
            {'&', TokenType.SYMBOL_AMPERSAND},
            {'|', TokenType.SYMBOL_PIPE},
            {'<', TokenType.SYMBOL_LESS_THAN},
            {'>', TokenType.SYMBOL_GREATER_THAN},
            {'=', TokenType.SYMBOL_EQUALITY},
            {'~', TokenType.SYMBOL_TILDE},
        };

        public static readonly Dictionary<TokenType, char> symbolsToStr = symbols.ToDictionary((i) => i.Value, (i) => i.Key);

        private string _inputFileContent { get; }
        private List<Token> tokens { get; } = new();
        // private int _position = 0;

        public Lexer(string inputFileContent)
        {
            _inputFileContent = inputFileContent;
        }

        // private bool IsAtEnd() {
        //     return _position >= _inputFileContent.Length;
        // }

        // private char GetChar() {
        //     return _inputFileContent[_position];
        // }

        // private void SkipWhitespace() {
        //     while (!IsAtEnd() && char.IsWhiteSpace(GetChar())) {
        //         _position++;
        //     }
        // }

        // private Token GetToken() {
            
        // }

        public TokenList LexTokens() {
            TokenList tokens = new();

            List<string> wormdsUwU = new();            

            int substringStart = 0;
            for(int i = 0; i < _inputFileContent.Length; i++) {

              char? next = i+1 == _inputFileContent.Length ? null : _inputFileContent[i+1];
              char current = _inputFileContent[i];

              // If we have a next character.
              if(next is char nxt) {

                // And the next character is a symbol.
                if(symbols.ContainsKey(nxt) || nxt == ' '){
                  // Then all the previous should be looked up in keyword.
                  // if it does not match with a keyword it must be an identifier.

                  // main()
                  // startPosition = 0
                  // endPosition = 3
                  // foo() main() bar()

                  // identifier = _inputFileContent.Substring(startPosition, endPosition);
                  var line = _inputFileContent[substringStart..i]; // fUwUck cUwUck
                  //wormdsUwU.Add(_inputFileContent.Substring(substringStart, i - substringStart + 1));
                  var a = _inputFileContent.Substring(substringStart, i - substringStart + 1);

                  if(!String.IsNullOrEmpty(a.Trim())){
                    wormdsUwU.Add(a.Trim());
                  }

                  // Where the identifier starts will move ahead.
                  substringStart = i+1;
                }
              }
              

            }

            foreach(string word in wormdsUwU) {
              System.Console.WriteLine("Cancer: " + word);
            }

            return tokens;

            // dead zombie code airbow or below =)))))

            string[] words = _inputFileContent.Split(' ');

            foreach(var word in words) {
                Console.WriteLine("LEX: " + word);
                // word can be symbol, keyword, int, string, identifier

                // Check if word is a symbol.
                if (symbols.TryGetValue(word[0], out var tokenType)) {
                    tokens.Add(new Token(tokenType));
                    continue;
                } 
                
                // Check if word is a keyword.
                if (keywords.TryGetValue(word, out tokenType)) {
                    tokens.Add(new Token(tokenType));
                    continue;
                }
                
                // Check if integer constant
                if (short.TryParse(word, out short res)) {
                    tokens.Add(new Token(TokenType.INTEGER_CONSTANT, integerValue: res));
                    continue;
                }
                
                // Check if string constant
                if (word.StartsWith('"') && word.EndsWith('"')) {
                    tokens.Add(new Token(TokenType.STRING_CONSTANT, stringValue: word[1..^1]));
                    continue;
                }
                
                // Check if identifier
                // A valid identifier starts with a letter or an underscore.
                // It can contain numbers, but must not start with it.
                if (char.IsLetter(word[0]) || word.StartsWith('_')) {
                    if (word.All(c => !char.IsLetter(c) && !char.IsNumber(c) && c != '_')) {
                        throw new System.Exception($"Identifier not allowed only [a-zA-Z0-9_] allowed, got {word}");
                    }
                    tokens.Add(new Token(TokenType.IDENTIFIER, stringValue: word));
                    continue;
                }

                // If the token is unhandled throw an exception.
                throw new System.Exception($"Got unexpected token: {word}");
            }
            
            return tokens;
        }
    }
}