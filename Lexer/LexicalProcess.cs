public class LexicalAnalyzer
    {
        Dictionary<string, string> operators = new Dictionary<string, string>();
        Dictionary<string, string> keywords = new Dictionary<string, string>();
        Dictionary<string, string> texts = new Dictionary<string, string>();

        public IEnumerable<string> Keywords { get { return keywords.Keys; } }

        /* Associates an operator symbol with the correspondent token value */
        public void RegisterOperator(string op, string tokenValue)
        {
            this.operators[op] = tokenValue;
        }

        /* Associates a keyword with the correspondent token value */
        public void RegisterKeyword(string keyword, string tokenValue)
        {
            this.keywords[keyword] = tokenValue;
        }

        /* Associates a Text literal starting delimiter with their correspondent ending delimiter */
        public void RegisterText(string start, string end)
        {
            this.texts[start] = end;
        }

        
        /* Matches a new symbol in the code and read it from the string. The new symbol is added to the token list as an operator. */
        private bool MatchSymbol(TokenReader stream, List<Token> tokens)
        {
            foreach (var op in operators.Keys.OrderByDescending(k => k.Length))
                if (stream.Match(op))
                {
                    tokens.Add(new Token(TokenType.Symbol, operators[op], stream.Location));
                    return true;
                }
            return false;
        }

        /* Matches a Text part in the code and read the literal from the stream.
        The tokens list is updated with the new string token and errors is updated with new errors if detected. */
        private bool MatchText (TokenReader stream, List<Token> tokens, List<CompilingError> errors)
        {
            foreach (var start in texts.Keys.OrderByDescending(k=>k.Length))
            {
                string text;
                if (stream.Match(start))
                {
                    if (!stream.ReadUntil(texts[start], out text))
                        errors.Add(new CompilingError(stream.Location, ErrorCode.Expected, texts[start]));
                    tokens.Add(new Token(TokenType.Text, text, stream.Location));
                    return true;
                }
            }
            return false;
        }

        /* Returns all tokens read from the code and populate the errors list with all lexical errors detected. */
        public IEnumerable<Token> GetTokens(string fileName, string code, List<CompilingError> errors)
        {
            List<Token> tokens = new List<Token>();

            TokenReader stream = new TokenReader(fileName, code);

            while (!stream.EOF)
            {
                string value;

                if (stream.ReadWhiteSpace())
                    continue;

                if (stream.ReadID(out value))
                {
                    if (keywords.ContainsKey(value))
                        tokens.Add(new Token(TokenType.Keyword, keywords[value], stream.Location));
                    else
                        tokens.Add(new Token(TokenType.Identifier, value, stream.Location));
                    continue;
                }

                if (stream.ReadNumber(out value))
                {
                    double d;
                    if (!double.TryParse(value, out d))
                        errors.Add(new CompilingError(stream.Location, ErrorCode.Invalid, "Number format"));
                    tokens.Add(new Token(TokenType.Number, value, stream.Location));
                    continue;
                }

                if (MatchText(stream, tokens, errors))
                    continue;

                if (MatchSymbol(stream, tokens))
                    continue;

                var unkOp = stream.ReadAny();
                errors.Add(new CompilingError(stream.Location, ErrorCode.Unknown, unkOp.ToString()));
            }

            return tokens;
        }

        /* Allows to read from a string numbers, identifiers and matching some prefix. 
        It has some useful methods to do that */
        class TokenReader
        {
            string FileName;
            string code;
            int pos;
            int line;
            int lastLB;

            public TokenReader(string fileName, string code)
            {
                this.FileName = fileName;
                this.code = code;
                this.pos = 0;
                this.line = 1;
                this.lastLB = -1;
            }

            public CodeLocation Location
            {
                get
                {
                    return new CodeLocation
                    {
                        File = FileName,
                        Line = line,
                        Column = pos - lastLB
                    };
                }
            }

            /* Peek the next character */
            public char Peek()
            {
                if (pos < 0 || pos >= code.Length)
                    throw new InvalidOperationException();

                return code[pos];
            }

            public bool EOF
            {
                get { return pos >= code.Length; }
            }

            public bool EOL
            {
                get { return EOF || code[pos] == '\n'; }
            }

            public bool ContinuesWith(string prefix)
            {
                if (pos + prefix.Length > code.Length)
                    return false;
                for (int i = 0; i < prefix.Length; i++)
                    if (code[pos + i] != prefix[i])
                        return false;
                return true;
            }

            public bool Match(string prefix)
            {
                if (ContinuesWith(prefix))
                {
                    pos += prefix.Length;
                    return true;
                }

                return false;
            }

            public bool ValidIdCharacter(char c, bool begining)
            {
                if (begining)
                return char.IsLetter(c); // Solo letras al inicio
                else
                return char.IsLetterOrDigit(c) || c == '_'; // Letras, números o _ después
            }

            public bool ReadID(out string id)
            {
                id = "";
                while (!EOL && ValidIdCharacter(Peek(), id.Length == 0))
                id += ReadAny();
    
                // Verificar si el identificador es inválido
                if (id.Length > 0 && (id[0] == '_' || char.IsDigit(id[0])))
                {
                    return false; // Opcional: no agregar el token
                }
    
    return id.Length > 0;
            }

            public bool ReadNumber(out string number)
            {
                number = "";
                while (!EOL && char.IsDigit(Peek()))
                    number += ReadAny();
                if (!EOL && Match("."))
                {
                    // read decimal part
                    number += '.';
                    while (!EOL && char.IsDigit(Peek()))
                        number += ReadAny();
                }

                if (number.Length == 0)
                    return false;

                // Load Number posfix, i.e., 34.0F
                // Not supported exponential formats: 1.3E+4
                while (!EOL && char.IsLetterOrDigit(Peek()))
                    number += ReadAny();

                return number.Length > 0;
            }

            public bool ReadUntil(string end, out string text)
            {
                text = "";
                while (!Match(end))
                {
                    if (EOL || EOF)
                        return false;
                    text += ReadAny();
                }
                return true;
            }

            public bool ReadWhiteSpace()
            {
                if (char.IsWhiteSpace(Peek()))
                {
                    ReadAny();
                    return true;
                }
                return false;
            }

            public char ReadAny()
            {
                if (EOF)
                    throw new InvalidOperationException();

                if (EOL)
                {
                    line++;
                    lastLB = pos;
                }
                return code[pos++];
            }
        }

    }