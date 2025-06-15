using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace WindowsFormsApp1
{
    public class Parser
    {
        private List<Token> tokens;
        private List<CompilingError> errors;
        private TokenStream stream;
        private Context context;
        Canvas Canvas;
        public Parser(List<Token> tokens, TokenStream stream, Canvas canvas, Context context, List<CompilingError> errors)
        {
            this.tokens = tokens;
            this.stream = stream;
            this.errors = errors;
            Canvas = canvas;
            this.context = context;
        }

        #region Expression
        public Expression expression()
        {
            return Or();
        }
        public Expression Or()
        {
            Expression expression = And();
            while (stream.Match(TokenValues.Or))
            {
                Expression right = And();
                expression = new Or(stream.Previous().codeLocation, expression, right);
            }
            return expression;
        }
        public Expression And()
        {
            Expression expression = Equality();
            while (stream.Match(TokenValues.And))
            {
                Expression right = Equality();
                expression = new And(stream.Previous().codeLocation, expression, right);
            }
            return expression;
        }
        public Expression Equality()
        {
            Expression expression = Comparation();
            while (stream.Match(TokenValues.Equal, TokenValues.NotEqual))
            {
                Token Operator = stream.Previous();
                Expression right = Comparation();
                if (Operator.value == TokenValues.Equal)
                {
                    expression = new Equal(stream.Previous().codeLocation, expression, right);
                }
                else if (Operator.value == TokenValues.NotEqual)
                {
                    expression = new NotEqual(stream.Previous().codeLocation, expression, right);
                }
            }
            return expression;
        }
        public Expression Comparation()
        {
            Expression expression = Term();
            while (stream.Match(TokenValues.Greater, TokenValues.Less, TokenValues.GreaterT, TokenValues.LessT))
            {
                
                Token Operator = stream.Previous();
                Expression right = Term();
                if (Operator.value == TokenValues.Greater)
                {
                    expression = new Greater(stream.Previous().codeLocation, expression, right);
                }
                else if (Operator.value == TokenValues.Less)
                {
                    expression = new Less(stream.Previous().codeLocation, expression, right);
                }
                else if (Operator.value == TokenValues.LessT)
                {
                    expression = new LessT(stream.Previous().codeLocation, expression, right);
                }
                else if (Operator.value == TokenValues.GreaterT)
                {
                    expression = new GreaterT(stream.Previous().codeLocation, expression, right);
                }
            }
            return expression;
        }
        public Expression Term()
        {
            Expression expression = Factor();
            while (stream.Match(TokenValues.Add, TokenValues.Sub))
            {
       
                Token Operator = stream.Previous();
                Expression right = Factor();
                if (Operator.value == TokenValues.Add)
                {
                    expression = new Add(stream.Previous().codeLocation, expression, right);
                }
                else if (Operator.value == TokenValues.Sub)
                {
                    expression = new Subs(stream.Previous().codeLocation, expression, right);
                }
            }
            return expression;
        }
        public Expression Factor()
        {
            Expression expression = Exponent();
            while (stream.Match(TokenValues.Mul, TokenValues.Div))
            {
               
                Token Operator = stream.Previous();
                Expression right = Exponent();
                if (Operator.value == TokenValues.Mul)
                {
                    expression = new Mult(stream.Previous().codeLocation, expression, right);
                }
                else if (Operator.value == TokenValues.Div)
                {
                    expression = new Div(stream.Previous().codeLocation, expression, right, errors);
                }
            }
            return expression;
        }
        public Expression Exponent()
        {
            Expression expression = Unary();
            while (stream.Match(TokenValues.Pow, TokenValues.Module))
            {
                Token Operator = stream.Previous();
                Expression right = Unary();
                if (Operator.value == TokenValues.Pow)
                {
                    expression = new Pow(stream.Previous().codeLocation, expression, right);
                }
                else if (Operator.value == TokenValues.Module)
                {
                    expression = new Module(stream.Previous().codeLocation, expression, right);
                }
            }
            return expression;
        }
        private Expression Unary()
        {
            if (stream.Match(TokenValues.Sub))
            {
                Expression right = Unary();
                return new Number(-Convert.ToInt32(right.Value), stream.Previous().codeLocation);
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
            else if (stream.Match(TokenType.Number))
            {
                Expression left = new Number(Convert.ToInt32(stream.Previous().value), stream.Previous().codeLocation);
                return left;
            }
            else if (stream.Match(TokenValues.GetActualX))
            {
                Expression left = ParseGetActualX();
                return left;
            }
            else if (stream.Match(TokenValues.GetActualY))
            {
                Expression left = ParseGetActualY();
                return left;
            }
            else if (stream.Match(TokenValues.GetCanvasSize))
            {
                Expression left = ParseGetCanvasSize();
                return left;
            }
            else if (stream.Match(TokenValues.GetColorCount))
            {
                Expression left = ParseGetColorCount();
                return left;
            }
            else if (stream.Match(TokenValues.IsBrushColor))
            {
                Expression left = ParseIsBrushColor();
                return left;
            }
            else if (stream.Match(TokenValues.IsBrushSize))
            {
                Expression left = ParseIsBrushSize();
                return left;
            }
            else if (stream.Match(TokenValues.IsCanvasColor))
            {
                Expression left = ParseIsCanvasColor();
                return left;
            }
            else if (stream.Match(TokenValues.OpenBracket))
            {
                Expression expr = expression();
                stream.Consume(TokenValues.ClosedBracket, "A ) was expected after the expression");
                return new ParenthesizedExpression(stream.Previous().codeLocation, expr);
            }
            else if (stream.Match(TokenType.Identifier))
            {
                Token variableToken = stream.Previous();
                Expression expr = new Variable(
                    variableToken.codeLocation,
                    variableToken.value,
                    context
                );
                return expr;
            }
            else
                throw new CompilingError(stream.Previous().codeLocation, ErrorCode.Expected, "An expression was expected");
        }

        #endregion
        #region Statement
        public ProgramNode ParseProgram()
        {
            CollectLabels();
            stream.Reset();
            List <Statement>statements = new List<Statement>();
            while (stream.Match(TokenType.EOL)) ;
            do
            {
                try
                {
                    statements.Add(ParseStatement());
                    if (!stream.Match(TokenType.EOL))
                    {
                        if (!(stream.Peek().tokenType == TokenType.End))
                        {
                            throw new CompilingError(stream.Peek().codeLocation, ErrorCode.Expected,
                                "End of line expected after statement");
                        }
                    }
                    while (stream.Match(TokenType.EOL)) ;
                }
                catch (CompilingError error)
                {
                    if (Synchronize(error)) break;
                }
            } while (!stream.End && !stream.Match(TokenType.End));
            return new ProgramNode(new CodeLocation(), statements);
        }
        private void CollectLabels()
        {
            while (stream.Match(TokenType.EOL));

            while (!stream.End)
            {
                if (stream.LookAhead().tokenType == TokenType.Identifier && stream.LookAhead(1).value != TokenValues.Assign && stream.LookAhead(1).value != TokenValues.ClosedSquareBracket)
                {
                    Token labelToken = stream.LookAhead();
                    if (context.LabelExists(labelToken.value))
                    {
                        errors.Add(new CompilingError(labelToken.codeLocation, ErrorCode.UndefinedLabel, $"Label '{labelToken.value}' is already defined"));
                    }
                    else
                    {
                        context.RegisterLabel(labelToken.value, labelToken.codeLocation.Line);
                        context.Labels.Add(labelToken.value);
                    }
                }
                while (!stream.End && !stream.Match(TokenType.EOL))
                {
                    stream.MoveNext(1);
                }
                while (stream.Match(TokenType.EOL)) ;
            }
        }
        public bool Synchronize(CompilingError error)
        {
            errors.Add(error);
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
            else if (stream.Match(TokenValues.GoTo))
                return ParseGoto();
            else if (stream.LookAhead().tokenType == TokenType.Identifier)
            {
                if(stream.LookAhead(1).value == TokenValues.Assign)
                return Declaration();
                else return ParseLabel();
            }
            throw new CompilingError(stream.LookAhead().codeLocation, ErrorCode.Expected, "Statement unrecognizable");
        }
        public Statement VarDeclaration(Expression expresions, CodeLocation location)
        {
            Expression initializer = expression();
            return new VariableDec(expresions, initializer, location, context);
        }
        public Statement Declaration()
        {
            Statement statement = null;
            if (stream.Match(TokenType.Identifier))
            {
                stream.MoveNext(-1);
                CodeLocation loc = stream.Peek().codeLocation;
                Expression expresions = expression();
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
            stream.MoveNext(1);
            return new Label(location, stream.Previous().value, context);
        }

        private Fill ParseFill()
        {
            var location = stream.LookAhead().codeLocation;
            stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('
            stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
            return new Fill(location, Canvas);
        }
        private Spawn ParseSpawn()
        {
            Expression x = null;
            Expression y = null;
            var location = stream.LookAhead().codeLocation;
            stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

            x = expression();
            stream.Consume(TokenValues.Comma, "A , was expected"); // ','
            y = expression();

            stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'

            return new Spawn(location, x, y, Canvas);
        }
        private Colores ParseColor()
        {
            Expression x = null;
            var location = stream.LookAhead().codeLocation;
            stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

            x = expression();

            stream.Consume(TokenValues.ClosedBracket, "A ) was expected");
            return new Colores(location, x, Canvas);
        }
        private Sizes ParseSize()
        {
            Expression x = null;
            var location = stream.LookAhead().codeLocation;
            stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

            x = expression();

            stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
            return new Sizes(location, x, Canvas, errors);
        }
        private DrawLine ParseDrawLine()
        {
            Expression x = null;
            Expression y = null;
            Expression z = null;
            var location = stream.LookAhead().codeLocation;
            stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

            x = expression();
            stream.Consume(TokenValues.Comma, "A comma was expected");
            y = expression();
            stream.Consume(TokenValues.Comma, "A comma was expected");
            z = expression();

            stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
            return new DrawLine(location, x, y, z, Canvas, errors);
        }
        private DrawCircle ParseDrawCircle()
        {
            Expression x = null;
            Expression y = null;
            Expression z = null;
            var location = stream.LookAhead().codeLocation;
            stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

            x = expression();
            stream.Consume(TokenValues.Comma, "A comma was expected");
            y = expression();
            stream.Consume(TokenValues.Comma, "A comma was expected");
            z = expression();

            stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
            return new DrawCircle(location, x, y, z, Canvas, errors);
        }
        private DrawRectangle ParseDrawRectangle()
        {
            Expression x = null;
            Expression y = null;
            Expression z = null;
            Expression a = null;
            Expression b = null;
            var location = stream.LookAhead().codeLocation;
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
            return new DrawRectangle(location, x, y, z, a, b, Canvas, errors);
        }
        private GoTo ParseGoto()
        {
            string LabelName = null;
            Expression expr = null;
            var location = stream.LookAhead().codeLocation;
            stream.Consume(TokenValues.OpenSquareBracket, "A [ was expected"); // '['

            if (stream.Match(TokenType.Identifier))
            {
                LabelName = stream.Previous().value;
            }
            else
            {
                errors.Add(new CompilingError(location, ErrorCode.Expected, "An identifier was expected"));
            }   
            stream.Consume(TokenValues.ClosedSquareBracket, "A ] was expected"); // ']'
            stream.Consume(TokenValues.OpenBracket, "A ( was expected");
            expr = expression();
            stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
            return new GoTo(location, LabelName, expr);
        }
        private IsCanvasColor ParseIsCanvasColor()
        {
            Expression x = null;
            Expression y = null;
            Expression z = null;
            var location = stream.LookAhead().codeLocation;
            stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

            x = expression();
            stream.Consume(TokenValues.Comma, "A comma was expected");
            y = expression();
            stream.Consume(TokenValues.Comma, "A comma was expected");
            z = expression();

            stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
            return new IsCanvasColor(location, x, y, z, Canvas);
        }

        private IsBrushSize ParseIsBrushSize()
        {
            Expression x = null;
            var location = stream.LookAhead().codeLocation;
            stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

            x = expression();

            stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
            return new IsBrushSize(location, x, Canvas);
        }

        private IsBrushColor ParseIsBrushColor()
        {
            Expression x = null;
            var location = stream.LookAhead().codeLocation;
            stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('

            x = expression();

            stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
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
            return new GetColorCount(location, Canvas, x, y, z, a, b, errors);
        }

        private GetCanvasSize ParseGetCanvasSize()
        {
            var location = stream.LookAhead().codeLocation;
            stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('
            stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
            return new GetCanvasSize(location, Canvas);
        }

        private GetActualY ParseGetActualY()
        {
            var location = stream.LookAhead().codeLocation;
            stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('
            stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'
            return new GetActualY(location, Canvas);
        }

        private GetActualX ParseGetActualX()
        {
            var location = stream.LookAhead().codeLocation;

            stream.Consume(TokenValues.OpenBracket, "A ( was expected"); // '('
            stream.Consume(TokenValues.ClosedBracket, "A ) was expected"); // ')'

            return new GetActualX(location, Canvas);
        }
        #endregion
    }

}
