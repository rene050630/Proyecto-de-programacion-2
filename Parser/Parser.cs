using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
//revisar expresiones
public class Parser
{
    private List<Token> tokens;
    private List<CompilingError> errors;
    private TokenStream stream;
    private Context context;
    Canvas Canvas;
    public Parser(List<Token> tokens, TokenStream stream, Canvas canvas, Context context)
    {
        this.tokens = tokens;
        this.stream = stream;
        errors = new List<CompilingError>();
        Canvas = canvas;
        this.context = context;
    }
     
    #region Expression
    public Expression expression()
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
        while (stream.Match(TokenValues.Equal, TokenValues.NotEqual))
        {
            Expression right = Comparation();
            Token operatorToken = stream.Previous();
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
        while (stream.Match(TokenValues.Greater, TokenValues.Less, TokenValues.LessT, TokenValues.GreaterT))
        {
            Expression right = Term();
            Token operatorToken = stream.Previous();
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
        while (stream.Match(TokenValues.Add, TokenValues.Sub))
        {
            Expression right = Factor();
            Token operatorToken = stream.Previous();
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
        while (stream.Match(TokenValues.Mul, TokenValues.Div))
        {
            Expression right = Power();
            Token operatorToken = stream.Previous();
            if (operatorToken.value == TokenValues.Mul)
            {
                expr = new Mult(stream.Previous().codeLocation, expr, right);
            }
            else if (operatorToken.value == TokenValues.Div)
            {
                expr = new Div(stream.Previous().codeLocation, expr, right);
            }
            
        }
        return expr;
    }
    private Expression Power()
    {
        Expression expr = Unary();
        while (stream.Match(TokenValues.Pow, TokenValues.Module))
        {
            Expression right = Unary();
            Token Operator = stream.Previous();
            if (Operator.value == TokenValues.Pow)
            {
                expr = new Pow(stream.Previous().codeLocation, expr, right);
            }
            else if (Operator.value == TokenValues.Module)
            {
                expr = new Module(stream.Previous().codeLocation, expr, right);
            }
        }
        return expr;
    }
    private Expression Unary()
    {
        if (stream.Match(TokenValues.Sub))
        {
            Expression right = Unary();
            return new Number(-Convert.ToInt32(stream.Previous().value), stream.Previous().codeLocation);
        }
        return Primary();
    }
    private Expression Primary()
    {
        if (stream.Match(TokenValues.False))
            return new Bool(false, stream.Previous().codeLocation);
        else if (stream.Match(TokenType.Text))
            return new Text(stream.Previous().value, stream.Previous().codeLocation);
        else if (stream.Match(TokenValues.True))
            return new Bool(true, stream.Previous().codeLocation);

        else if (stream.Match(TokenValues.OpenBracket))
        {
            Expression expr = expression();
            stream.Consume(TokenValues.ClosedBracket, "A ) was expected after the expression");
            return new ParenthesizedExpression(stream.Previous().codeLocation, expr);
        }
        else if (stream.Match(TokenType.Number))
        {
            System.Console.WriteLine(stream.Previous().value);
            Token variable = stream.Previous();
            return new Number(Convert.ToInt32(variable.value), stream.Previous().codeLocation);   
        }
        else if (stream.Match(TokenType.Identifier))
        {
            Token variable = stream.Previous();
            Expression expr = new Variable(stream.Peek().codeLocation, variable.value, context);
            return expr;
        }
        else
            throw new CompilingError(stream.Previous().codeLocation, ErrorCode.Expected, "An expression was expected");
    }
    #endregion
    #region Statement
    public ProgramNode ParseProgram()
    {
        List<Statement> statements = new List<Statement>();
        do
        {
            try
            {
                statements.Add(ParseStatement());
                // if (!stream.Match(TokenValues.StatementSeparator) && !stream.End)
                // {
                //     throw new CompilingError(stream.LookAhead().codeLocation, ErrorCode.Expected, "A newline was expected");
                // }   
            }
            catch (CompilingError error)
            {
                //errors.Add(error); // Registrar error
                System.Console.WriteLine(error.Argument);
                if (Synchronize(error)) break; // Saltar tokens hasta un punto seguro
            }
            // Consumir múltiples saltos de línea
            //while (stream.Match(TokenValues.StatementSeparator)) ;
        } while (!stream.End);
        return new ProgramNode(new CodeLocation(), statements);
    }
    public bool Synchronize(CompilingError error) //Recupera el análisis después de un error
    {
        errors.Add( error);
        while (!stream.End)
        {
            if (stream.End || stream.Peek().value == TokenValues.ClosedBracket) return true;
            else stream.MoveNext(1);
        }
        return false;
    }
    public Statement ParseStatement()
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
        else if (stream.LookAhead().tokenType == TokenType.Identifier && stream.LookAhead(1).value == TokenValues.Assign)
            return Declaration();
        else if (stream.LookAhead().tokenType == TokenType.Identifier)
            return ParseLabel();
        throw new CompilingError(stream.LookAhead().codeLocation, ErrorCode.Expected, "Statement unrecognizable");
    }
    public Statement VarDeclaration(Expression expresions, CodeLocation location)
    {
        Token operador = stream.Previous();
        if (stream.Match(TokenValues.Fill)) return ParseFill();
        else if (stream.Match(TokenValues.GetActualX)) return ParseGetActualX();
        else if (stream.Match(TokenValues.GetActualY)) return ParseGetActualY();
        else if (stream.Match(TokenValues.GetCanvasSize)) return ParseGetCanvasSize();
        else if (stream.Match(TokenValues.GetColorCount)) return ParseGetColorCount();
        else if (stream.Match(TokenValues.IsBrushColor)) return ParseIsBrushColor();
        else if (stream.Match(TokenValues.IsBrushSize)) return ParseIsBrushSize();
        else if (stream.Match(TokenValues.IsCanvasColor)) return ParseIsCanvasColor();
        Expression initializer = expression();
        return new VariableDec(expresions, initializer, operador, location, context);
    }
    public Statement Declaration()
    {
        Statement statement = null;
        if (stream.Match(TokenType.Identifier))
        {
            stream.MoveNext(-1);
            CodeLocation loc = stream.Peek().codeLocation;
            Expression expresions = expression();
            statement = new ExpressionEvaluator(expresions, expresions.location);
            if (stream.Match(TokenValues.Assign))
            {
                statement = VarDeclaration(expresions, loc);
            }
        }
        else throw new CompilingError(stream.Peek().codeLocation, ErrorCode.Invalid, "Invalid Expression");
        return statement;
    }
    private Label ParseLabel()
    {
        var location = stream.LookAhead().codeLocation;

        //stream.Consume(TokenValues.StatementSeparator, "A newline was expected"); // '\n'
        return new Label(location, stream.LookAhead().value);
    }

    private Fill ParseFill()
    {
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.Fill, "A Fill was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('
        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new Fill(location, Canvas);
    }

    private Spawn ParseSpawn()
    {
        Expression x = null;
        Expression y = null;
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.Spawn, "A Spawn was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        x = expression();
        stream.Consume(TokenValues.Comma, "A , was expected"); // ','
        y = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'

        return new Spawn(location, x, y, Canvas);
    }
    private Color ParseColor()
    {
        Expression x = null;
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.Color, "A Color was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        x = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected");
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new Color(location, x, Canvas);
    }
    private Size ParseSize()
    {
        Expression x = null;
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.Size, "A Size was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        x = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new Size(location, x, Canvas);
    }
    private DrawLine ParseDrawLine()
    {
        Expression x = null;
        Expression y = null;
        Expression z = null;
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.DrawLine, "A DrawLine was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        x = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        y = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        z = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new DrawLine(location, x, y, z, Canvas);
    }
    private DrawCircle ParseDrawCircle()
    {
        Expression x = null;
        Expression y = null;
        Expression z = null;
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.DrawCircle, "A DrawCircle was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        x = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        y = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        z = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new DrawCircle(location, x, y, z, Canvas);
    }
    private DrawRectangle ParseDrawRectangle()
    {
        Expression x = null;
        Expression y = null;
        Expression z = null;
        Expression a = null;
        Expression b = null;
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.DrawRectangle, "A DrawRectangle was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        x = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        y = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        z = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        a = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        b = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new DrawRectangle(location, a, b, x, y, z, Canvas);
    }
    // private VariableDec ParseVariable()
    // {
    //     var location = stream.LookAhead().codeLocation;
    //     // Expression Id = expression();
    //     string n = stream.LookAhead().value;
    //     stream.Advance();
    //     Token op = stream.Consume(TokenValues.Assign, "A <- was expected");
    //     Expression initializer = expression();
    //     return new VariableDec(n, initializer, op, location);
    // }
    private GoTo ParseGoto()
    {
        string label = null;
        Expression expr = null;
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.GoTo, "A GoTo was expected");
        stream.Consume(TokenValues.OpenSquareBracket, "A [ was expected"); // '['
        label = stream.LookAhead().value;
        stream.Advance();
        stream.Consume(TokenValues.ClosedSquareBracket, "A ] was expected"); // ']'
        stream.Consume(TokenValues.OpenBracket, "A ( was expected");

        expr = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new GoTo(location, label, expr, context);
    }
    private IsCanvasColor ParseIsCanvasColor()
    {
        Expression x = null;
        Expression y = null;
        Expression z = null;
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.IsCanvasColor, "A IsCanvasColor was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        x = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        y = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        z = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new IsCanvasColor(location, x, y, z, Canvas);
    }

    private IsBrushSize ParseIsBrushSize()
    {
        Expression x = null;
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.IsBrushSize, "A IsBrushSize was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        x = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new IsBrushSize(location, x, Canvas);   
    }

    private IsBrushColor ParseIsBrushColor()
    {
        Expression x = null;
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.IsBrushColor, "A IsBrushColor was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        x = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new IsBrushColor(location, x, Canvas); 
    }

    private GetColorCount ParseGetColorCount()
    {
        Expression x = null;
        Expression y = null;
        Expression z = null;
        Expression a = null;
        Expression b = null;
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.GetColorCount, "A GetColorCount was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

        x = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        y = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        z = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        a = expression();
        stream.Consume(TokenValues.Comma, "A comma was expected");
        b = expression();

        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new GetColorCount(location, Canvas, b, x, y, z, a);
    }

    private GetCanvasSize ParseGetCanvasSize()
    {
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.GetCanvasSize, "A GetCanvasSize was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('
        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new GetCanvasSize(location, Canvas);
    }

    private GetActualY ParseGetActualY()
    {
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.GetActualY, "A GetActualY was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('
        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new GetActualY(location, Canvas);
    }

    private GetActualX ParseGetActualX()
    {
        var location = stream.LookAhead().codeLocation;
        //stream.Consume(TokenValues.GetActualX, "A GetActualX was expected");
        stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('
        stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
        //stream.Consume(TokenValues.StatementSeparator, "A \n was expected"); // '\n'
        return new GetActualX(location, Canvas);
    }
    #endregion
}

