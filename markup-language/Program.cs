/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System.Text;
using static TokenType;

class Program {
    private const string HTML_TEMPLATE_START = @"
<!DOCTYPE html>
<html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Document</title>
    </head>
    <body>

";
    private const string HTML_TEMPLATE_END = @"
    </body>
</html>";

    // TODO: come up with application name
    private const string HELP_MESSAGE = @"
Usage: dotnet run -- [options] file...
Options:
    -h, --help                  Display this information.
    -d, --document              Output complete HTML document
    -t, --tokens                Output tokenisation
    -o <file>, --out <file>     place output into <file>
";
    // debug
    private static bool debug = false;

    private static bool hadError = false;

    // command line options
    private static bool fullDoc = false;
    private static bool doOut = false;
    private static bool tokenOut = false;

    static void Main(string[] args) {
        // debug
        if (debug) {
            runPrompt();
            return;
        }

        string? fileOut = null;
        string? source = null;
        try {
            foreach (string arg in args) {
                if (arg is "-h" or "--help" && args.Length == 1) {
                    Console.WriteLine(HELP_MESSAGE);
                    return;
                }
                if (arg is "-d" or "--document")
                    // check for -d argument
                    fullDoc = true;
                else if (arg is "-o" or "--out")
                    doOut = true;
                else if (arg is "-t" or "--tokens")
                    tokenOut = true;
                else if (source == null && !arg.StartsWith("-")) {
                    // first argument that isn't an option is the source file
                    if (Path.GetExtension(arg) != ".mkl") throw new InvalidFileException("Source must be a .mkl file");
                    if (!File.Exists(arg)) throw new FileNotFoundException($"Cannot find {arg}: No such file");
                    source = arg;
                }
                else if (doOut && source != null && !arg.StartsWith("-") && fileOut == null) {
                    // specify the output file
                    fileOut = arg;
                }
                else {
                    throw new Exception("Invalid arguments");
                }
            }
            if (source == null)
                throw new Exception("Missing file");
            if (doOut && fileOut == null)
                throw new Exception("'-o' option given but not output file specified");

        }
        catch (FileNotFoundException e) {
            Console.WriteLine("Error: " + e.Message);
            Environment.Exit(1);
        }
        catch (InvalidFileException e) {
            Console.WriteLine("Error: " + e.Message);
            Environment.Exit(1);
        }
        catch (Exception e) {
            Console.WriteLine("Error: " + e.Message);
            Console.WriteLine("'dotnet run -- -h' for usage");
            Environment.Exit(1);
        }
        runFile(source, fileOut);
    }
    // debug repl
    static void runPrompt() {
        while (true) {
            Console.Write("> ");
            String? line = Console.ReadLine();
            if (line == null) break;
            string processed = run(line);
            Console.WriteLine(processed);
            Console.WriteLine();
        }

    }

    static string run(String source) {
        Lexer lexer = new Lexer(source);
        List<Token> tokens = lexer.scanTokens();
        if (tokenOut) {
            Console.WriteLine();
            foreach (Token token in tokens) {
                Console.Write($"{token.type} ");
                if (token.type == NL)
                    Console.WriteLine();
            }
            Console.WriteLine();
        }
        Parser parser = new Parser(tokens);
        ParseTree result = parser.parse();

        if (hadError)
            // exit on syntax error
            Environment.Exit(1);

        return result.process();
    }

    static void runFile(String path, string? output) {
        output ??= "./out.html"; // default value for output if null
        byte[] bytes = File.ReadAllBytes(path);
        String content = Encoding.Default.GetString(bytes);
        string processed = run(content);
        if (fullDoc) {
            string finalOutput = HTML_TEMPLATE_START + processed + HTML_TEMPLATE_END;
            File.WriteAllText(output, finalOutput);
        }
        else
            File.WriteAllText(output, processed);
        Console.WriteLine($"saved to {output}");
    }

    public static ParseError error(int line, string message) {
        hadError = true;
        Console.WriteLine($"Error at line {line}: {message}");
        return new ParseError();
    }
    public static ParseError error(int line, string message, string type) {
        hadError = true;
        Console.WriteLine($"Error at line {line}: {message}");
        return new ParseError(type);
    }
}

// TODO: add more error types
class ParseError : Exception {
    public string type { get; set; }
    public ParseError(string type = "generic") {
        this.type = type;

    }
}

class InvalidFileException : Exception {
    public InvalidFileException(string message) : base(message) { }
};