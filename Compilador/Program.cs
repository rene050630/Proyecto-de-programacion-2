
public class Program
{
//     public static void TestParser()
// {
//     // // Caso de prueba 1: Expresión simple
//     // var lexer = Compiling.Lexical;
//     // string testCode = @"Spawn(5)";
//     // var errors = new List<CompilingError>();
//     // var tokens = lexer.GetTokens("test_file.txt", testCode, errors);
//     // Context context = new Context();
//     // Canvas canvas = new Canvas(256);
//     // var parser = new Parser(tokens.ToList(), new TokenStream(tokens), canvas, context);
//     // var result = parser.ParseProgram();
//     // System.Console.WriteLine(result.checksemantic(context, errors));
//     // // Añadir más casos de prueba...
// }

    //Ejecutar en Main
    static void Main(string[] args)
    {
        //TestParser();
        string codigoFuente = File.ReadAllText("test1.txt");
//         string codigoFuente = @"Spawn (1, 1)
// loop 
// GoTo [loop] (1 < 2)";
        Context Context = new Context();
        Canvas canvas = new Canvas(10);
        List<CompilingError> errors = new List<CompilingError>();
        var lexer = Compiling.Lexical;
        var tokens = lexer.GetTokens(codigoFuente, errors);
        Parser parser = new Parser(tokens.ToList(), new TokenStream(tokens), canvas, Context, errors);
        ProgramNode block = parser.ParseProgram();
        block.checksemantic(Context, errors);
        block.Execute();
        foreach (CompilingError item in errors)
        {
            System.Console.WriteLine($"Argument: {item.Argument}");
            System.Console.WriteLine($"line: {item.Location.Line}");
        }
    }
    // static void Main(string[] args)
    // {
    //     var lexer = Compiling.Lexical;

    //     // 2. Código de prueba
    //     string testCode = @"GoTo[loop] (true)";
    //     // 3. Procesar el código
    //     var errors = new List<CompilingError>();
    //     var tokens = lexer.GetTokens(testCode, errors);
    //     var stream = new TokenStream(tokens);
    //     foreach (var syy in stream)
    //     {
    //         System.Console.WriteLine(syy.value);
    //         //System.Console.WriteLine(syy.tokenType);
    //     }
    // }
}
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