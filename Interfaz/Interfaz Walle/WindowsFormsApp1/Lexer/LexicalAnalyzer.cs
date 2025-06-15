namespace WindowsFormsApp1
{
    class Compiling
    {
        private static LexicalAnalyzer __LexicalProcess;
        public static LexicalAnalyzer Lexical
        {
            get
            {
                if (__LexicalProcess == null)
                {
                    __LexicalProcess = new LexicalAnalyzer();


                    __LexicalProcess.RegisterOperator("+", TokenValues.Add);
                    __LexicalProcess.RegisterOperator("*", TokenValues.Mul);
                    __LexicalProcess.RegisterOperator("-", TokenValues.Sub);
                    __LexicalProcess.RegisterOperator("/", TokenValues.Div);
                    __LexicalProcess.RegisterOperator("<-", TokenValues.Assign);
                    __LexicalProcess.RegisterOperator("**", TokenValues.Pow);
                    __LexicalProcess.RegisterOperator("%", TokenValues.Module);

                    __LexicalProcess.RegisterOperator(",", TokenValues.Comma);
                    __LexicalProcess.RegisterOperator("(", TokenValues.OpenBracket);
                    __LexicalProcess.RegisterOperator(")", TokenValues.ClosedBracket);
                    __LexicalProcess.RegisterOperator("[", TokenValues.OpenSquareBracket);
                    __LexicalProcess.RegisterOperator("]", TokenValues.ClosedSquareBracket);

                    __LexicalProcess.RegisterOperator("||", TokenValues.Or);
                    __LexicalProcess.RegisterOperator("&&", TokenValues.And);
                    __LexicalProcess.RegisterOperator("<=", TokenValues.LessT);
                    __LexicalProcess.RegisterOperator(">=", TokenValues.GreaterT);
                    __LexicalProcess.RegisterOperator("==", TokenValues.Equal);
                    __LexicalProcess.RegisterOperator("!=", TokenValues.NotEqual);
                    __LexicalProcess.RegisterOperator(">", TokenValues.Greater);
                    __LexicalProcess.RegisterOperator("<", TokenValues.Less);

                    __LexicalProcess.RegisterText("\"", "\"");

                    __LexicalProcess.RegisterKeyword("Spawn", TokenValues.Spawn);
                    __LexicalProcess.RegisterKeyword("Color", TokenValues.Color);
                    __LexicalProcess.RegisterKeyword("Size", TokenValues.Size);
                    __LexicalProcess.RegisterKeyword("DrawLine", TokenValues.DrawLine);
                    __LexicalProcess.RegisterKeyword("DrawCircle", TokenValues.DrawCircle);
                    __LexicalProcess.RegisterKeyword("DrawRectangle", TokenValues.DrawRectangle);
                    __LexicalProcess.RegisterKeyword("Fill", TokenValues.Fill);
                    __LexicalProcess.RegisterKeyword("GoTo", TokenValues.GoTo);

                    __LexicalProcess.RegisterKeyword("GetActualX", TokenValues.GetActualX);
                    __LexicalProcess.RegisterKeyword("GetActualY", TokenValues.GetActualY);
                    __LexicalProcess.RegisterKeyword("GetCanvasSize", TokenValues.GetCanvasSize);
                    __LexicalProcess.RegisterKeyword("GetColorCount", TokenValues.GetColorCount);
                    __LexicalProcess.RegisterKeyword("IsBrushColor", TokenValues.IsBrushColor);
                    __LexicalProcess.RegisterKeyword("IsBrushSize", TokenValues.IsBrushSize);
                    __LexicalProcess.RegisterKeyword("IsCanvasColor", TokenValues.IsCanvasColor);

                }

                return __LexicalProcess;
            }
        }
    }
}