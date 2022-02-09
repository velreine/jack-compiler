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

        public Lexer(string inputFileContent)
        {
            _inputFileContent = inputFileContent;
        }

        public TokenList LexTokens() {
            TokenList tokens = new();

            List<string> wormdsUwU = new();            

            
            for(int i = 0; i < _inputFileContent.Length; i++) {

                char? next = i+1 == _inputFileContent.Length ? null : _inputFileContent[i+1];
                char current = _inputFileContent[i];

                // A whitespace is not considered a "word" so just advance the needle.
                if(char.IsWhiteSpace(current)) {
                    continue;
                }

                // If there is a comment, skip to next line.
                if (current == '/' && next == '/') {
                    int lineBreakIdx = _inputFileContent[i..].IndexOf("\n");
                    if (lineBreakIdx == -1) {
                        break;
                    }
                    i += lineBreakIdx + 1;
                    continue;
                }
                
                // Handle symbols.
                if (symbols.TryGetValue(current, out TokenType symbol)) {
                    tokens.Add(new Token(symbol));
                    continue;
                }

                // Handle string constants.
                if (current == '"') {
                    int start = i + 1;
                    int len = _inputFileContent[start..].IndexOf('"');
                    if (len == -1) {
                        System.Console.WriteLine(_inputFileContent[start..]);
                        throw new System.Exception("unterminated string");
                    }
                    string str = _inputFileContent.Substring(start, len);
                    tokens.Add(new Token(TokenType.STRING_CONSTANT, stringValue: str));
                    i += len+1;
                    continue;
                }

                // Handle integer constants.
                if (char.IsNumber(current)) {
                    // int number = current - '0';
                    int stop = i;

                    // Keep advancing until the next char is no longer a number.
                    while (stop < _inputFileContent.Length && char.IsNumber(_inputFileContent[stop])) {
                        stop++;
                    }
                    
                    // Finally parse it as a 16 bit number.
                    if (short.TryParse(_inputFileContent[i..stop], out short res)) {
                        tokens.Add(new Token(TokenType.INTEGER_CONSTANT, integerValue: res));
                    }
                    i = stop - 1;
                    continue;
                }

                // Handle identifiers and keywords.
                // Identifiers must start with underscore or a letter.
                // Identifier may contain number, but not begin with it.
                // _foo4Bar
                if (char.IsLetter(current) || current == '_') {
                    int stop = i;
                    while (stop < _inputFileContent.Length 
                        && (char.IsNumber(_inputFileContent[stop])
                        || char.IsLetter(_inputFileContent[stop])
                        || _inputFileContent[stop] == '_')) {
                        stop++;
                    }

                    string keywordOrId = _inputFileContent[i..stop];

                    if (keywords.TryGetValue(keywordOrId, out var keyword)) {
                        tokens.Add(new Token(keyword));
                    } else {
                        tokens.Add(new Token(TokenType.IDENTIFIER, stringValue: keywordOrId));
                    }
                    i = stop - 1;
                    continue;
                }

                continue;
            }

            foreach(var word in tokens) {
                System.Console.WriteLine("Cancer: " + word);
            }

            return tokens;
        }
    }
}

public class BadJokes{
    private static List<string> jokes = new(){
        "Joe?\nJoemama.", "Ligma?\nLigmaballs.",
        "Fuck Lukas.", "Din mor er så fed, at hendes blodtype er nutella.",
        "Din mor er så grim, at da hun tilmeldte sig grimhedskonkurrencen, blev hun afvist med svaret 'ingen professionelle.'",
    };
    public static string GetJoke(){
        Random rnd = new Random();
        return jokes[rnd.Next(0, jokes.Count)];
    }
}