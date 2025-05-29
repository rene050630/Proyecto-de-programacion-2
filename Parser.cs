using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public class Parser
{
    private List<Token> tokens;
    int current = 0;
    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }
    private Expression Term()
    {

    } 
    private Expression Comparation()
    {
        Expression expr = Term();
        while (!Stream.End && Stream.Match(TokenValues.Greater, TokenValues.Less, TokenValues.LessT, TokenValues.GreaterT))
        {
            Token operatorToken = Stream.Previous();
            Expression right = Term();
            if (operatorToken.Value == TokenValues.Greater)
            {
                expr = new Greater(Stream.Previous().Location, expr, right);
            }
            else if (operatorToken.Value == TokenValue.minor)
            {
                expr = new Minor(Stream.Previous().Location, expr, right);
            }
            else if (operatorToken.value == TokenValue.lessOrEqual)
            {
                expr = new LessOrEqual(Stream.Previous().Location, expr, right);
            }
            else if (operatorToken.Value == TokenValue.greaterOrEqual)
            {
                expr = new GreaterOrEqual(Stream.Previous().Location, expr, right);
            }
        }
        return expr;
    }

    