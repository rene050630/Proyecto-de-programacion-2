using System.Collections;
using System;
using System.Collections.Generic;
namespace WindowsFormsApp1
{
    public class TokenStream : IEnumerable<Token>
    {
        private List<Token> tokens;
        public int position;
        public int Position { get { return position; } }

        public TokenStream(IEnumerable<Token> tokens)
        {
            this.tokens = new List<Token>(tokens);
            position = 0;
        }
        public void Reset()
        {
            position = 0;
        }
        public bool End => position >= tokens.Count - 1;
        public bool Match(params string[] values)
        {
            foreach (var token in values)
            {
                if (Check(token))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }
        public bool Match(TokenType type)
        {
            if (CheckType(type))
            {
                Advance();
                return true;
            }
            return false;
        }
        public bool CheckType(TokenType type)
        {
            return Peek().tokenType == type;
        }
        public Token Advance()
        {
            if (!End) position++;
            return Previous();
        }
        public Token Peek()
        {
            return tokens[position];
        }
        public bool Check(string value)
        {
            return Peek().value == value;
        }
        public void MoveNext(int k)
        {
            position += k;
        }

        public void MoveBack(int k)
        {
            position -= k;
        }
        public bool Next()
        {
            if (position < tokens.Count - 1)
            {
                position++;
            }

            return position < tokens.Count;
        }
        public bool Next(TokenType type)
        {
            if (position < tokens.Count - 1 && LookAhead(1).tokenType == type)
            {
                position++;
                return true;
            }

            return false;
        }
        public bool Next(string value)
        {
            if (position < tokens.Count - 1 && LookAhead(1).value == value)
            {
                position++;
                return true;
            }

            return false;
        }
        public Token Consume(string value, string message)
        {
            if (Check(value)) return Advance();
            throw new CompilingError(Peek().codeLocation, ErrorCode.Expected, message);
        }
        public Token Previous()
        {
            return tokens[position - 1];
        }

        public bool CanLookAhead(int k = 0)
        {
            return tokens.Count - position > k;
        }

        public Token LookAhead(int k = 0)
        {
            return tokens[position + k];
        }

        public IEnumerator<Token> GetEnumerator()
        {
            for (int i = position; i < tokens.Count; i++)
                yield return tokens[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}