class Program
{
    // static void Main(string[] args)
    // {
    //      var lexer = Compiling.Lexical;

    //     // 2. Código de prueba
    //     string testCode = @"
    //     Spawn 'Circle' at (100, 200)
    //     n <- 5 + 3 * 2
    //     DrawLine from [0, 0] to [100, 100]
    //     # Esto es un comentario (debe ser ignorado)
    //     xy = 42.5 * (GetActualX / 2)
    //     ";
    //     // 3. Procesar el código
    //     var errors = new List<CompilingError>();
    //     var tokens = lexer.GetTokens("test_file.txt", testCode, errors);

    //     // 4. Mostrar resultados
    //     Console.WriteLine("==== TOKENS ENCONTRADOS ====");
    //     foreach (var token in tokens)
    //     {
    //         Console.WriteLine(token);
    //     }

    //     Console.WriteLine("\n==== ERRORES ====");
    //     foreach (var error in errors)
    //     {
    //         Console.WriteLine(error);
    //     }
    // }
    static void Main(string[] args)
    {
        var a = new ParserTests();
        System.Console.WriteLine(a.CreateParser("5 + 8"));
    }
}
public class ParserTests
{
    public Parser CreateParser(string code)
    {
        var lexer = Compiling.Lexical;
        var errors = new List<CompilingError>();
        var tokens = lexer.GetTokens("test", code, errors);
        var stream = new TokenStream(tokens);
        return new Parser(new List<Token>(tokens), stream);
    }

}