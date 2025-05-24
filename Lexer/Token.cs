public enum TokenType {
    Unknwon,
    Number,
    Text,
    Keyword,
    Identifier,
    Symbol
}
public class Token
{
    public string value { get; }
    public CodeLocation codeLocation { get; }
    public TokenType tokenType { get; }
    public Token(TokenType tokenType, string Value,  CodeLocation codeLocation)
    {
        this.codeLocation = codeLocation;
        this.tokenType = tokenType;
        this.value = Value;
    }
    public override string ToString()
    {
        return $"{tokenType}: {value} (Line: {codeLocation.Line}, Column: {codeLocation.Column})";
    }
}
public class CodeLocation
{
    public string? File;
    public int Line;
    public int Column;
}
public class TokenValues
{
    protected TokenValues() { }

    public const string Add = "Addition"; // +
    public const string Sub = "Subtract"; // -
    public const string Mul = "Multiplication"; // *
    public const string Div = "Division"; // /
    public const string Pow = "Pow"; // **
    public const string Module = "Module"; // %

    public const string Assign = "Assign"; // <-
    public const string ValueSeparator = "ValueSeparator"; // _ 
    public const string StatementSeparator = "StatementSeparator"; // \n

    public const string OpenBracket = "OpenBracket"; // ( 
    public const string ClosedBracket = "ClosedBracket"; // )
    // public const string OpenQM = "quotation marks"; // "
    // public const string CloseQM = "quotation marks"; // "

    public const string Or = "Or"; // ||
    public const string And = "And"; // && 
    public const string Equal = "Equal"; // ==
    public const string NotEqual = "NotEqual"; // !=
    public const string Greater = "Greater"; // >
    public const string Less = "Less"; // < 
    public const string GreaterT = "GreaterThan"; // >= 
    public const string LessT = "LessThan"; // <= 
    public const string Spawn = "Spawn";
    public const string Color = "Color";
    public const string Size = "Size";
    public const string DrawLine = "DrawLine";
    public const string DrawCircle = "DrawCircle";
    public const string DrawRectangle = "DrawRectangle";
    public const string Fill = "Fill";
    public const string GoTo = "GoTo";
    public const string GetActualX = "GetActualX";
    public const string GetActualY = "GetActualY";
    public const string GetCanvasSize = "GetCanvasSize";
    public const string GetColorCount = "GetColorCount";
    public const string IsBrushColor = "IsBrushColor";
    public const string IsBrushSize = "IsBrushSize";
    public const string IsCanvasColor = "IsCanvasColor";
    public const string OpenSquareBracket = "OpenSquareBracket"; // [
    public const string ClosedSquareBracket = "ClosedSquareBracket"; // ]
    public const string Comma = "Comma"; // ,
    public const string Identifier = "Identifier"; // Ej: "n", "loop1"
    public const string StringLiteral = "StringLiteral"; // Ej: "Red", "Blue"
    public const string BooleanLiteral = "BooleanLiteral"; // true/false (si aplica)
}