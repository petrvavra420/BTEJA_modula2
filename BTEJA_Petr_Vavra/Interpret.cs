using Antlr.Runtime.Tree;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_Petr_Vavra
{
    internal class Interpret
    {
        static void Main(string[] args)
        {
            // Display the number of command line arguments.
            Console.WriteLine("Začátek programu");
            string filePath = "..\\..\\..\\sourceprog.txt"; 
            string fileContents = File.ReadAllText(filePath);

            ICharStream inputStream  = CharStreams.fromString(fileContents);
            Modula2Lexer gramatikaLexer = new Modula2Lexer(inputStream);
            CommonTokenStream tokens = new CommonTokenStream(gramatikaLexer);
            Modula2Parser parser = new Modula2Parser(tokens);
            Modula2Parser.ProgramContext gramatikaContext = parser.program();
           ModulaVisitor visitor = new ModulaVisitor();
            visitor.symbolTables.Push(new SymbolTable());

            visitor.Visit(gramatikaContext);
            
            }

    }
}
