using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public class Parser
{
    private List<Token> tokens;
    private List<CompilingError> errors;
    private TokenStream stream;
    Canvas Canvas;
    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }
    #region Expression
    private Expression expression()
    {
        return Or();
    }
    private Expression Or()
    {
        Expression expr = And();
        while (stream.Match(TokenValues.Or))
        {
            Expression right = And();
            expr = new Or(stream.Previous().codeLocation, expr, right);
        }
        return expr;
    }
    private Expression And()
    {
        Expression expr = Equality();
        while (stream.Match(TokenValues.And))
        {
            Expression right = Equality();
            expr = new And(stream.Previous().codeLocation, expr, right);
        }
        return expr;
    }
    private Expression Equality()
    {
        Expression exp = Comparation();
        while (!stream.End && stream.Match(TokenValues.Equal, TokenValues.NotEqual))
        {
            Token operatorToken = stream.Previous();
            Expression right = Comparation();
            if (operatorToken.value == TokenValues.Equal)
            {
                exp = new Equal(stream.Previous().codeLocation, exp, right);
            }
            else if (operatorToken.value == TokenValues.NotEqual)
            {
                exp = new NotEqual(stream.Previous().codeLocation, exp, right);
            }
        }
        return exp;
    }
    private Expression Comparation()
    {
        Expression expr = Term();
        while (!stream.End && stream.Match(TokenValues.Greater, TokenValues.Less, TokenValues.LessT, TokenValues.GreaterT))
        {
            Token operatorToken = stream.Previous();
            Expression right = Term();
            if (operatorToken.value == TokenValues.Greater)
            {
                expr = new Greater(stream.Previous().codeLocation, expr, right);
            }
            else if (operatorToken.value == TokenValues.Less)
            {
                expr = new Less(stream.Previous().codeLocation, expr, right);
            }
            else if (operatorToken.value == TokenValues.LessT)
            {
                expr = new LessT(stream.Previous().codeLocation, expr, right);
            }
            else if (operatorToken.value == TokenValues.GreaterT)
            {
                expr = new GreaterT(stream.Previous().codeLocation, expr, right);
            }
        }
        return expr;
    }
    private Expression Term()
    {
        Expression expr = Factor();
        while (!stream.End && stream.Match(TokenValues.Add, TokenValues.Sub))
        {
            Token operatorToken = stream.Previous();
            Expression right = Factor();
            if (operatorToken.value == TokenValues.Add)
            {
                expr = new Add(stream.Previous().codeLocation, expr, right);
            }
            else if (operatorToken.value == TokenValues.Sub)
            {
                expr = new Subs(stream.Previous().codeLocation, expr, right);
            }
        }
        return expr;
    }
    private Expression Factor()
    {
        Expression expr = Power();
        while (!stream.End && stream.Match(TokenValues.Mul, TokenValues.Div, TokenValues.Module))
        {
            Token operatorToken = stream.Previous();
            Expression right = Power();
            if (operatorToken.value == TokenValues.Mul)
            {
                expr = new Mult(stream.Previous().codeLocation, expr, right);
            }
            else if (operatorToken.value == TokenValues.Div)
            {
                expr = new Div(stream.Previous().codeLocation, expr, right);
            }
            else if (operatorToken.value == TokenValues.Module)
            {
                expr = new Module(stream.Previous().codeLocation, expr, right);
            }
        }
        return expr;
    }
    private Expression Power()
    {
        Expression expr = Unary();
        while (!stream.End && stream.Match(TokenValues.Pow))
        {
            Expression right = Unary();
            expr = new Pow(stream.Previous().codeLocation, expr, right);
        }
        return expr;
    }
    private Expression Unary()
    {
        if (!stream.End && stream.Match(TokenValues.Sub))
        {
            Expression right = Unary();
            return new Number(-double.Parse(stream.Previous().value), stream.Previous().codeLocation);
        }
        return Primary();
    }
    private Expression Primary()
    {
        if (stream.Match(TokenValues.False))
            return new Bool(false, stream.Previous().codeLocation);

        if (stream.Match(TokenValues.True))
            return new Bool(true, stream.Previous().codeLocation);

        if (stream.Match(TokenValues.OpenBracket))
        {
            Expression expr = expression();
            stream.Consume(TokenValues.ClosedBracket, "A ) was expected after the expression");
            return new ParenthesizedExpression(stream.Previous().codeLocation, expr);
        }
        if (stream.Match(TokenType.Number))
            return new Number(double.Parse(stream.Previous().value), stream.Previous().codeLocation);

        if (stream.Match(TokenType.Text))
            return new Text(stream.Previous().value, stream.Previous().codeLocation);

        throw new CompilingError(stream.Peek().codeLocation, ErrorCode.Expected, "An expression was expected");
    }
    private Expression ProcessMemberAccess(Expression expression)
    {
        // En este lenguaje simple no hay acceso a miembros
        throw new CompilingError(stream.Peek().codeLocation, ErrorCode.Expected, "Member access is not available in this languaje");
    }
    #endregion
    #region Statement

    private Statement ParseStatement()
    {
        if (stream.Match(TokenValues.Spawn))
            return ParseSpawn();
        else if (stream.Match(TokenValues.Color))
            return ParseColor();
        else if (stream.Match(TokenValues.Size))
            return ParseSize();
        else if (stream.Match(TokenValues.DrawLine))
            return ParseDrawLine();
        else if (stream.Match(TokenValues.DrawCircle))
            return ParseDrawCircle();
        else if (stream.Match(TokenValues.DrawRectangle))
            return ParseDrawRectangle();
        else if (stream.Match(TokenValues.Fill))
            return ParseFill();
        else if (stream.Match(TokenValues.GetActualX))
            return ParseGetActualX();
        else if (stream.Match(TokenValues.GetActualY))
            return ParseGetActualY();
        else if (stream.Match(TokenValues.GetCanvasSize))
            return ParseGetCanvasSize();
        else if (stream.Match(TokenValues.GetColorCount))
            return ParseGetColorCount();
        else if (stream.Match(TokenValues.IsBrushColor))
            return ParseIsBrushColor();
        else if (stream.Match(TokenValues.IsBrushSize))
            return ParseIsBrushSize();
        else if (stream.Match(TokenValues.IsCanvasColor))
            return ParseIsCanvasColor();
        else if (stream.Match(TokenValues.GoTo))
            return ParseGoto();
        else if (stream.LookAhead().tokenType == TokenType.Identifier && stream.LookAhead(1).value == "<-")
            return ParseVariable();
        else if (stream.LookAhead().tokenType == TokenType.Identifier)
            return ParseLabel();
        throw new CompilingError(stream.LookAhead().codeLocation, ErrorCode.Expected, "Statement unrecognizable");
    }

    private Label ParseLabel()
    {
        var location = stream.LookAhead().codeLocation;
        var identifierToken = stream.Consume(TokenValues.Identifier, "Identifier expected");

        return new Label(location, identifierToken.value);
    }

    private Fill ParseFill()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.Fill, "A Fill was expected");
        return new Fill(location, Canvas);
    }

    private Spawn ParseSpawn()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.Spawn, "A Spawn was expected");
        stream.Consume(TokenValues.OpenBracket, "An ( was expected"); // '('

        var x = expression();
        stream.Consume(TokenValues.Comma, "A , was expected"); // ','
        var y = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'

        return new Spawn(location, x, y, Canvas);
    }
    private Color ParseColor()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.Color, "A Color was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        var x = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected");
        return new Color(location, x, Canvas);
    }
    private Size ParseSize()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.Size, "A Size was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        var x = expression();

        stream.Consume(TokenValues.Comma, "A ) was expected"); // ')'
        return new Size(location, x, Canvas);
    }
    private DrawLine ParseDrawLine()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.DrawLine, "A DrawLine was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        var x = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var y = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var z = expression();

        stream.Consume(TokenValues.Comma, "A ) was expected"); // ')'
        return new DrawLine(location, x, y, z, Canvas);
    }
    private DrawCircle ParseDrawCircle()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.DrawCircle, "A DrawCircle was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        var x = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var y = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var z = expression();

        stream.Consume(TokenValues.Comma, "A ) was expected"); // ')'
        return new DrawCircle(location, x, y, z, Canvas);
    }
    private DrawRectangle ParseDrawRectangle()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.DrawRectangle, "A DrawRectangle was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        var x = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var y = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var z = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var a = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var b = expression();

        stream.Consume(TokenValues.Comma, "A ) was expected"); // ')'
        return new DrawRectangle(location, a, b, x, y, z, Canvas);
    }
    private Variable ParseVariable()
    {

        var location = stream.LookAhead().codeLocation;

        var identifierToken = stream.Consume(TokenValues.Identifier, "An identifier was expected");

        stream.Consume(TokenValues.Assign, "A <- was expected"); // "<-"

        var exp = expression();

        return new Variable(identifierToken.value, exp, location);

    }
    private GoTo ParseGoto()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.GoTo, "A GoTo was expected");
        stream.Consume(TokenValues.OpenSquareBracket, "A [ was expected"); // '['

        var label = ParseLabel();

        stream.Consume(TokenValues.ClosedSquareBracket, "A ] was expected"); // ']'
        stream.Consume(TokenValues.OpenBracket, "A ( was expected");

        var expr = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        return new GoTo(location, label, expr);
    }
    private IsCanvasColor ParseIsCanvasColor()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.IsCanvasColor, "A IsCanvasColor was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        var x = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var y = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var z = expression();

        stream.Consume(TokenValues.Comma, "A ) was expected"); // ')'
        return new IsCanvasColor(location, x, y, z, Canvas);
    }

    private IsBrushSize ParseIsBrushSize()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.IsBrushSize, "A IsBrushSize was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        var x = expression();

        stream.Consume(TokenValues.Comma, "A ) was expected"); // ')'
        return new IsBrushSize(location, x, Canvas);   
    }

    private IsBrushColor ParseIsBrushColor()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.IsBrushColor, "A IsBrushColor was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        var x = expression();

        stream.Consume(TokenValues.Comma, "A ) was expected"); // ')'
        return new IsBrushColor(location, x, Canvas); 
    }

    private GetColorCount ParseGetColorCount()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.GetColorCount, "A GetColorCount was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        var x = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var y = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var z = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var a = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        var b = expression();

        stream.Consume(TokenValues.Comma, "A ) was expected"); // ')'
        return new GetColorCount(location, Canvas, b, x, y, z, a);
    }

    private GetCanvasSize ParseGetCanvasSize()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.GetCanvasSize, "A GetCanvasSize was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('
        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        return new GetCanvasSize(location, Canvas);
    }

    private GetActualY ParseGetActualY()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.GetActualY, "A GetActualY was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('
        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        return new GetActualY(location, Canvas);
    }

    private GetActualX ParseGetActualX()
    {
        var location = stream.LookAhead().codeLocation;
        stream.Consume(TokenValues.GetActualX, "A GetActualX was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('
        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        return new GetActualX(location, Canvas);
    }
    #endregion
}

