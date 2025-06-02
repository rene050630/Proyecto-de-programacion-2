
public class Program
{
    public static void TestParser()
{
    // Caso de prueba 1: Expresión simple
    var tokens = new List<Token>
    {
        new Token(TokenType.Keyword, "", new CodeLocation()),
    };
    
    var parser = new Parser(tokens, new TokenStream(tokens));

        try
        {
            var result = parser.ParseStatement();
            Console.WriteLine("Prueba 1 exitosa:");
            Console.WriteLine($"Tipo: {result.GetType().Name}");
        }
        catch (CompilingError ex)
        {
            System.Console.WriteLine(ex.Argument);
            Console.WriteLine($"Error en prueba 1: {ex.Message}");
        }
    
    // Añadir más casos de prueba...
}

// Ejecutar en Main
static void Main(string[] args)
{
    TestParser();
}
    // static void Main(string[] args)
    // {
    //     var lexer = Compiling.Lexical;

    //     // 2. Código de prueba
    //     string testCode = @"Spawn(0, 0) 'Circle' at (100, 200)
    //     n <- 5 + 3 * -2
    //     DrawLine from [0, 0] to [100, 100]
    //     xy = 42.5 * (GetActualX / 2)
    //     ";
    //     // 3. Procesar el código
    //     var errors = new List<CompilingError>();
    //     var tokens = lexer.GetTokens("test_file.txt", testCode, errors);
    //     var stream = new TokenStream(tokens);
    //     var parser = new Parser(tokens.ToList(), stream);

    //     // Act
    //     var program = parser.ParseProgram();
    //     System.Console.WriteLine(program);
    //     // Assert
    //     Assert.IsInstanceOf<Context>(program);
    //     Assert.AreEqual(3, program.Statements.Count);
    //     Assert.IsInstanceOf<Size>(program.Statements[0]);
    //     Assert.IsInstanceOf<Color>(program.Statements[1]);
    //     Assert.IsInstanceOf<Spawn>(program.Statements[2]);
    //     // 4. Mostrar resultados
    //     // Console.WriteLine("==== TOKENS ENCONTRADOS ====");
    //     // foreach (var token in tokens)
    //     // {
    //     //     Console.WriteLine(token);
    //     // }

    //     // Console.WriteLine("\n==== ERRORES ====");
    //     // foreach (var error in errors)
    //     // {
    //     //     Console.WriteLine(error);
    //     // }
    // }
}
// public class ParserTests
// {
//     public Parser CreateParser(string code)
//     {
//         var lexer = Compiling.Lexical;
//         var errors = new List<CompilingError>();
//         var tokens = lexer.GetTokens("test", code, errors);
//         var stream = new TokenStream(tokens);
//         return new Parser(new List<Token>(tokens), stream);
//     }

// }