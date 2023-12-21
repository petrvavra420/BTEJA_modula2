using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BTEJA_Petr_Vavra
{
    public class Interpreter : Modula2BaseVisitor<object?>
    {

        //zásobník tabulek symbolů(proměnné a procedury)  
        public Stack<SymbolTable> symbolTables = new Stack<SymbolTable>();
        //seznam předdefinovaných procedur
        public List<string> builtInProcedures = new List<string>() { "ReadIn", "WriteOut", "WriteLine", "Rand" };

        public override object VisitVarStatement(Modula2Parser.VarStatementContext context)
        {
            string substrVAR = context.GetText().Substring(0, 3);

            var identifier = context.ident().GetText();
            DataType dataType = (DataType)VisitType(context.type());

            //pokud není pole
            if (dataType != DataType.ARRAY)
            {
                Variable varNew = new Variable();
                varNew.Name = identifier;
                varNew.DataType = dataType;

                //pokud obsahuje i inicializaci, načteme expr
                if (context.GetText().Contains(":="))
                {
                    // načtení hodnoty
                    varNew.Value = VisitExpression(context.expression());
                    // kontrola přiřazení správné hodnoty dle datového typu
                    Type valueType = varNew.Value.GetType();
                    if (
                        !((valueType == typeof(int) && varNew.DataType == DataType.INTEGER)
                        || (valueType == typeof(string) && varNew.DataType == DataType.CHAR)
                        || (valueType == typeof(float) && varNew.DataType == DataType.REAL)
                        || (valueType == typeof(int) && varNew.DataType == DataType.REAL)
                        )
                    )
                    {
                        throw new Exception("Invalid value assigned.");
                    }
                }

                // Přidání proměnné do SymbolTable aktuálního kontextu
                //zkontroluji jestli je v tabulce symbolů na spodu zásobníku proměnná se stejným názvem, pokud ano, vyhodím chybu
                var symbolTablesList = symbolTables.ToList();

                if (symbolTablesList.Last().VariableExists(varNew.Name))
                {
                    throw new Exception("Variable " + varNew.Name + " already exists.");
                }
                else
                {
                    //pokud ne, přidám proměnnou do tabulky symbolů aktuálního kontextu
                    symbolTables.Peek().AddVariable(varNew);
                }
            }
            else if (dataType == DataType.ARRAY)
            {
                // Předpokládám, že VisitArray vrací objekt typu Variable nebo podobný
                Variable arrayVar = (Variable)VisitArray(context.type().array());
                arrayVar.Name = identifier;

                // Přidání pole do SymbolTable aktuálního kontextu
                symbolTables.Peek().AddVariable(arrayVar);
            }

            return null;
        }

        public override object VisitArray(Modula2Parser.ArrayContext context)
        {
            int arraySize = (int)VisitExpression(context.expression());
            DataType arrayType = (DataType)VisitType(context.type());

            // Musíme zkontrolovat, jestli není null kvůli gramatice, kde expression nemusí existovat
            if (arraySize != null)
            {
                Variable variableArray = new Variable();
                variableArray.ArraySize = arraySize;
                variableArray.DataType = DataType.ARRAY;

                if (arrayType != DataType.ARRAY)
                {
                    variableArray.ArrayType = arrayType;

                    // Přidání pole do SymbolTable aktuálního kontextu
                    for (int i = 0; i < arraySize; i++)
                    {
                        variableArray.ArrayValues.Add(null);
                    }

                    return variableArray;
                }
                else if (arrayType == DataType.ARRAY)
                {
                    variableArray.ArrayType = DataType.ARRAY;

                    // Předpokládám, že VisitArray vrací objekt typu Variable nebo podobný
                    Variable varNew = (Variable)VisitArray(context.type().array());

                    for (int i = 0; i < arraySize; i++)
                    {
                        Variable varCopy = varNew.DeepCopy();
                        variableArray.ArrayValues.Add(varCopy);
                    }
                }
                return variableArray;
            }
            else
            {
                throw new Exception("Invalid definition of array.");
            }
        }

        public override object VisitAssignment(Modula2Parser.AssignmentContext context)
        {
            var varName = context.ident().GetText();
            var value = VisitExpression(context.expression());
            //Console.WriteLine(varName + " s hodnotou: " + value);

            // Dohledání proměnné v aktuálním kontextu
            Variable varFind = null;
            foreach (var table in symbolTables)
            {
                varFind = table.GetVariable(varName);
                if (varFind != null)
                    break;
            }

            // Pro případ, když se jedná o pole
            if (varFind?.DataType == DataType.ARRAY)
            {
                List<int> arrayIndexes = new List<int>();

                foreach (var index in context.arrayAccess())
                {
                    arrayIndexes.Add((int)VisitArrayAccess(index));
                }

                if (arrayIndexes.Count > 1)
                {
                    Variable finalFound = varFind;

                    for (int i = 0; i < arrayIndexes.Count - 1; i++)
                    {
                        finalFound = (Variable)finalFound.ArrayValues[arrayIndexes[i]];
                    }
                    finalFound.ArrayValues[arrayIndexes.Last()] = value;
                }
                else
                {
                    varFind.ArrayValues[arrayIndexes[0]] = value;
                }

                return null;
            }

            // Kontrola datových typů
            Type valueType = value.GetType();
            if (
                !((valueType == typeof(int) && varFind.DataType == DataType.INTEGER)
                || (valueType == typeof(string) && varFind.DataType == DataType.CHAR)
                || (valueType == typeof(float) && varFind.DataType == DataType.REAL)
                || (valueType == typeof(int) && varFind.DataType == DataType.REAL)
                || (valueType == typeof(float) && varFind.DataType == DataType.INTEGER)

            ))
            {
                // Pokud je hodnota string a typ je integer, zkuste převést
                if (valueType == typeof(string) && varFind.DataType == DataType.INTEGER)
                {
                    try
                    {
                        int parsedInt = int.Parse((string)value);
                        varFind.Value = parsedInt;
                        return null;
                    }
                    catch (Exception)
                    {
                        //chyba při převodu
                    }
                }
                // Pokud je hodnota string a typ je real, zkuste převést s ohledem na kulturu
                else if (valueType == typeof(string) && varFind.DataType == DataType.REAL)
                {
                    if (float.TryParse((string)value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float parsedFloat))
                    {
                        varFind.Value = parsedFloat;
                        return null;
                    }
                }
                throw new Exception("Invalid value assigned.");
            }

            varFind.Value = value;
            return null;
        }

        public override object VisitArrayIndexAccess(Modula2Parser.ArrayIndexAccessContext context)
        {
            var varName = context.ident().GetText();

            // Dohledání proměnné v aktuálním kontextu
            Variable varFind = null;
            foreach (var table in symbolTables)
            {
                varFind = table.GetVariable(varName);
                if (varFind != null)
                    break;
            }

            // Pro případ, když se jedná o pole
            if (varFind?.DataType == DataType.ARRAY)
            {
                List<int> arrayIndexes = new List<int>();

                foreach (var index in context.arrayAccess())
                {
                    arrayIndexes.Add((int)VisitArrayAccess(index));
                }

                if (arrayIndexes.Count > 1)
                {
                    Variable finalFound = varFind;

                    for (int i = 0; i < arrayIndexes.Count - 1; i++)
                    {
                        finalFound = (Variable)finalFound.ArrayValues[arrayIndexes[i]];
                    }

                    return finalFound.ArrayValues[arrayIndexes.Last()];
                }
                else
                {
                    return varFind.ArrayValues[arrayIndexes[0]];
                }
            }

            throw new Exception($"{varName} is not an array.");
        }

        public override object VisitType([NotNull] Modula2Parser.TypeContext context)
        {
            string dataTypeText = context.GetText();
            switch (dataTypeText)
            {
                case "INTEGER":
                    return DataType.INTEGER;
                case "REAL":
                    return DataType.REAL;
                case "CHAR":
                    return DataType.CHAR;
                default:
                    break;
            }

            if (dataTypeText.Substring(0, 5) == "ARRAY")
            {
                return DataType.ARRAY;
            }
            return base.VisitType(context);
        }

        public override object VisitExpression([NotNull] Modula2Parser.ExpressionContext context)
        {
            object result = VisitTerm(context.term(0)); // Zpracuje první term

            //pokud existuje mínus před termem, provedeme negaci
            if (context.negateOp() != null)
            {
                result = (int)result * -1;
            }

            for (int i = 0; i < context.addOp().Length; i++)
            {
                string op = context.addOp(i).GetText(); // Získá operátor plus nebo minus

                if (op == "+")
                {
                    result = Add(result, VisitTerm(context.term(i + 1))); // Přida další term
                }
                else if (op == "-")
                {
                    result = Subtract(result, VisitTerm(context.term(i + 1))); // Odečte další term
                }
            }
            return result;
        }

        public override object VisitTerm([NotNull] Modula2Parser.TermContext context)
        {
            object result = VisitFactor(context.factor(0)); // Zpracuje první factor

            for (int i = 0; i < context.multOp().Length; i++)
            {
                string op = context.multOp(i).GetText(); // Získá operátor násobení nebo dělení

                if (op == "*")
                {
                    result = Multiply(result, VisitFactor(context.factor(i + 1))); // vynásobí s  dalším factorem
                }
                else if (op == "/")
                {
                    result = Divide(result, VisitFactor(context.factor(i + 1))); // vydělí s dalším factorem
                }
            }
            return result;

            //return base.VisitTerm(context);
        }

        public override object VisitFactor([NotNull] Modula2Parser.FactorContext context)
        {
            // Zde zkontrolujte, zda je ve contextu expression hodnota nastavena
            if (context.expression() != null)
            {
                //vrátím hodnotu z expression
                return VisitExpression(context.expression());
            }

            // Pokud expression není nastaveno, zpracujte Factor normálně
            return base.VisitFactor(context);
        }

        private object? Divide(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return l / r;
            }
            if (left is float lf && right is float rf)
            {
                return lf / rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt / rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat / rInt;
            }
            throw new Exception("unable to divide " + left + " and " + right);
        }
        private object? Multiply(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return l * r;
            }
            if (left is float lf && right is float rf)
            {
                return lf * rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt * rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat * rInt;
            }
            throw new Exception("unable to multiply " + left + " and " + right);
        }
        private object? Negate(object? result)
        {
            if (result is int rInt)
            {
                return rInt * -1;
            }
            if (result is float rFloat)
            {
                return rFloat * -1;
            }

            throw new Exception("unable to negate " + result);
        }
        private object? Subtract(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return l - r;
            }
            if (left is float lf && right is float rf)
            {
                return lf - rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt - rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat - rInt;
            }

            // Pokud je levá strana null, zkusíme provést negaci pravé strany - dočasný workaround
            if (left == null && right is int rnInt)
            {
                try
                {
                    return 0 - rnInt ;
                }
                catch (Exception)
                {
                    throw new Exception("unable to subtract " + left + " and " + right);
                }
            }
            if (left == null && right is float rnFloat)
            {
                try
                {
                    return 0 - rnFloat;
                }
                catch (Exception)
                {
                    throw new Exception("unable to subtract " + left + " and " + right);
                }
            }
            throw new Exception("unable to subtract " + left + " and " + right);
        }
        private object? Add(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return l + r;
            }
            if (left is float lf && right is float rf)
            {
                return lf + rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt + rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat + rInt;
            }

            //Console.WriteLine("typ: " + left.GetType());
            throw new Exception("unable to add " + left + " and " + right);
        }
        public override object VisitIdent(Modula2Parser.IdentContext context)
        {
            var varName = context.GetText();
            // Dohledání proměnné v aktuálním kontextu
            Variable variableFind = null;
            foreach (var table in symbolTables)
            {
                variableFind = table.GetVariable(varName);
                if (variableFind != null)
                    break;
            }

            if (variableFind == null)
            {
                throw new Exception("Variable " + varName + " is not defined.");
            }

            return variableFind.Value;

        }

        public override object VisitNumber([NotNull] Modula2Parser.NumberContext context)
        {
            return int.Parse(context.GetText());
        }

        public override object VisitCharacter([NotNull] Modula2Parser.CharacterContext context)
        {
            //tato metoda odstraní uvozovky a vrátí znak
            string text = context.GetText();
            int startIndex = text.IndexOf('"') + 1; // Najdi pozici první uvozovky a přidej 1
            int endIndex = text.LastIndexOf('"'); // Najdi pozici poslední uvozovky
            string extractedText = text.Substring(startIndex, endIndex - startIndex);
            return extractedText;
        }
        public override object VisitAddOp([NotNull] Modula2Parser.AddOpContext context)
        {
            return context.GetText();
        }

        public override object VisitRealNumber([NotNull] Modula2Parser.RealNumberContext context)
        {
            //nastavení kultury pro tečku
            CultureInfo culture = CultureInfo.InvariantCulture;
            float number = float.Parse(context.GetText(), NumberStyles.Float, culture);
            return number;
        }

        //forStatement: 'FOR' ident ':=' expression 'TO' expression ('BY' expression)? 'DO' (statement ';')+ 'END';
        public override object VisitForStatement(Modula2Parser.ForStatementContext context)
        {
            // Zjistím název první proměnné
            var ident = context.ident().GetText();

            // Dohledání proměnné v aktuálním kontextu
            Variable variable = null;
            foreach (var table in symbolTables)
            {
                variable = table.GetVariable(ident);
                if (variable != null)
                    break;
            }

            // Pokud proměnná neexistuje, vytvoříme a přidáme do aktuálního kontextu 
            if (variable == null)
            {
                variable = new Variable();
                variable.Name = ident;
                variable.DataType = DataType.INTEGER;
                // Přidání proměnné do aktuálního kontextu SymbolTable
                symbolTables.Peek().AddVariable(variable);
            }

            // Kontrola, zda existující proměnná je typu INTEGER
            if (variable.DataType != DataType.INTEGER)
            {
                throw new Exception("Existing variable '" + variable.Name + "' is not type INTEGER");
            }

            // Přiřadíme hodnotu
            variable.Value = VisitExpression(context.expression(0));

            int rangeFrom = (int)variable.Value;
            // Uložíme si pravou krajní mez FOR cyklu
            int rangeTo = (int)VisitExpression(context.expression(1));
            // Výchozí hodnota inkrementace bude jedna, není-li specifikováno "BY" jinak
            int rangeBy = 1;
            // Pokud existuje třetí výraz v definici, budeme předpokládat, že se jedná o "BY"
            if (context.expression(2) != null)
            {
                rangeBy = (int)VisitExpression(context.expression(2));
            }

            // Uložení původní hodnoty proměnné pro pozdější obnovení
            int originalValue = (int)variable.Value;

            for (int i = rangeFrom; i < rangeTo; i += rangeBy)
            {
                // Vytvoření nového kontextu SymbolTable pro nový blok
                symbolTables.Push(new SymbolTable());
                foreach (var statement in context.statement())
                {
                    // Nastavení aktuální hodnoty proměnné pro iteraci
                    variable.Value = i;
                    VisitStatement(statement);
                }
                // Odstranění kontextu SymbolTable pro nový blok
                symbolTables.Pop();
            }
            // Obnovení původní hodnoty proměnné
            variable.Value = originalValue;

            return null;
        }

        //ifStatement: 'IF' condition 'THEN' (statement ';')+ ('ELSIF' condition 'THEN' (statement ';')+)* ('ELSE' (statement ';')+)? 'END';
        public override object VisitIfStatement([NotNull] Modula2Parser.IfStatementContext context)
        {
            bool condition = (bool)VisitCondition(context.condition(0));
            //zkontrolujeme první IF, pokud je TRUE, provedeme všechny příkazy
            if (condition)
            {
                //vložíme nový kontext do zásobníku
                symbolTables.Push(new SymbolTable());

                //Console.WriteLine("---DEBUG: Condition is TRUE");
                Visit(context.ifBlock());

                //odstraníme kontext ze zásobníku
                symbolTables.Pop();
                return null;
            }
            else if (!condition)
            {
                //Console.WriteLine("---DEBUG: MAIN Condition is FALSE");
                for (int i = 1; i < context.condition().Length; i++)
                {
                    bool conditionElseResult = (bool)VisitCondition(context.condition(i));
                    if (conditionElseResult)
                    {
                        //vložíme nový kontext do zásobníku
                        symbolTables.Push(new SymbolTable());
                        // minus jedna protože první je IF
                        Visit(context.elseIfBlock(i - 1));
                        //odstraníme kontext ze zásobníku
                        symbolTables.Pop();
                        return null;
                    }
                }
                if (context.elseBlock() != null)
                {
                    //vložíme nový kontext do zásobníku
                    symbolTables.Push(new SymbolTable());
                    Visit(context.elseBlock());
                    symbolTables.Pop();
                }
            }
            return null;
        }

        //condition: expression ('>=' | '<=' | '>' | '<' | '=' | '#') expression | ident | ('1' | '0');
        public override object VisitCondition(Modula2Parser.ConditionContext context)
        {
            string text = context.GetText();

            // True nebo false check ('1' | '0')
            if (text == "1")
            {
                return true;
            }
            else if (text == "0")
            {
                return false;
            }

            // Check ('>=' | '<=' | '>' | '<' | '=' | '#')
            if (text.Contains(">="))
            {
                object left = VisitExpression(context.expression(0));
                object right = VisitExpression(context.expression(1));
                bool result = GreaterThanOrEqual(left, right);
                return result;
            }

            if (text.Contains("<="))
            {
                object left = VisitExpression(context.expression(0));
                object right = VisitExpression(context.expression(1));
                bool result = LessThanOrEqual(left, right);
                return result;
            }

            if (text.Contains(">"))
            {
                object left = VisitExpression(context.expression(0));
                object right = VisitExpression(context.expression(1));
                bool result = GreaterThan(left, right);
                return result;
            }

            if (text.Contains("<"))
            {
                object left = VisitExpression(context.expression(0));
                object right = VisitExpression(context.expression(1));
                bool result = LessThan(left, right);
                return result;
            }

            if (text.Contains("="))
            {
                object left = VisitExpression(context.expression(0));
                object right = VisitExpression(context.expression(1));
                bool result = IsEqual(left, right);
                return result;
            }

            if (text.Contains("#"))
            {
                object left = VisitExpression(context.expression(0));
                object right = VisitExpression(context.expression(1));
                bool result = IsNotEqual(left, right);
                return result;
            }

            // Check ident
            if (context.ident() != null)
            {
                var ident = context.ident().GetText();

                // Dohledání proměnné v aktuálním kontextu
                Variable variable = null;
                foreach (var table in symbolTables)
                {
                    variable = table.GetVariable(ident);
                    if (variable != null)
                        break;
                }

                if (variable == null)
                {
                    throw new Exception("Variable " + ident + " is not defined.");
                }
                return ConvertVariableValueToBool(variable);
            }

            return null;
        }

        private bool ConvertVariableValueToBool(Variable variable)
        {
            if (variable.Value == "1" || variable.Value == "true")
            {
                return true;
            }
            else if (variable.Value == "0" || variable.Value == "false")
            {
                return false;
            }
            else if (variable.DataType == DataType.INTEGER)
            {
                return (int)variable.Value == 1;
            }
            else if (variable.DataType == DataType.REAL)
            {
                return (float)variable.Value == 1;
            }

            return false;
        }
        private bool GreaterThan(object left, object right)
        {
            if (left is int l && right is int r)
            {
                return l > r;
            }
            if (left is float lf && right is float rf)
            {
                return lf > rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt > rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat > rInt;
            }
            throw new Exception("unable to compare " + left + " and " + right);
        }
        private bool GreaterThanOrEqual(object left, object right)
        {
            if (left is int l && right is int r)
            {
                return l >= r;
            }
            if (left is float lf && right is float rf)
            {
                return lf >= rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt >= rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat >= rInt;
            }
            throw new Exception("unable to compare " + left + " and " + right);
        }
        private bool LessThanOrEqual(object left, object right)
        {
            if (left is int l && right is int r)
            {
                return l <= r;
            }
            if (left is float lf && right is float rf)
            {
                return lf <= rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt <= rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat <= rInt;
            }
            throw new Exception("unable to compare " + left + " and " + right);
        }
        private bool LessThan(object? left, object? right)
        {
            if (left is int l && right is int r)
            {
                return l < r;
            }
            if (left is float lf && right is float rf)
            {
                return lf < rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt < rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat < rInt;
            }
            throw new Exception("unable to compare " + left + " and " + right);
        }
        private bool IsEqual(object left, object right)
        {
            if (left is int l && right is int r)
            {
                return l == r;
            }
            if (left is float lf && right is float rf)
            {
                return lf == rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt == rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat == rInt;
            }

            if (left is string lString && right is string rString)
            {
                return lString == rString;
            }
            throw new Exception("unable to compare " + left + " and " + right);
        }
        private bool IsNotEqual(object left, object right)
        {
            if (left is int l && right is int r)
            {
                return l != r;
            }
            if (left is float lf && right is float rf)
            {
                return lf != rf;
            }
            if (left is int lInt && right is float rFloat)
            {
                return lInt != rFloat;
            }
            if (left is float lFloat && right is int rInt)
            {
                return lFloat != rInt;
            }
            throw new Exception("unable to compare " + left + " and " + right);
        }

        //procedureDeclaration: 'PROCEDURE' ident ( '(' varFormal (';' varFormal)* ')' )? (':' type)? ';' 'BEGIN' (statement ';')+ ('RETURN' expression ';')? 'END' ident;

        public override object VisitProcedureDeclaration([NotNull] Modula2Parser.ProcedureDeclarationContext context)
        {
            //zjistíme název procedury
            string procedureName = context.ident(0).GetText();

            //vytváříme list pro parametry
            List<VarFormal> varFormals = new List<VarFormal>();
            //pokud existují parametry, načteme je
            foreach (var varFormal in context.varFormal())
            {
                object varFormalObject = VisitVarFormal(varFormal);
                if (varFormalObject != null)
                {
                    varFormals.Add((VarFormal)varFormalObject);
                }
                //Console.WriteLine("---DEBUG: varFormal: " + varFormal.GetText());
            }

            //zjistíme návratový typ pokud existuje, jinak null
            DataType returnType = DataType.NULL;
            if (context.type() != null)
            {
                returnType = (DataType)VisitType(context.type());
            }

            //zjistíme tělo procedury
            List<object?> procedureBody = new List<object?>();
            foreach (var statement in context.statement())
            {
                if (statement != null)
                {
                    procedureBody.Add(statement);
                }
            }
            //zjistíme návratovou hodnotu pokud existuje, jinak null
            object? returnExpression = null;
            if (context.expression() != null)
            {
                //returnExpression = VisitExpression(context.expression());
                returnExpression = context.expression();
            }

            //vytvoříme proceduru
            Procedure procedure = new Procedure();
            procedure.Name = procedureName;
            procedure.VarFormals = varFormals;
            procedure.ReturnType = returnType;
            procedure.Body = procedureBody;
            procedure.ReturnExpression = returnExpression;

            //zkontrolujeme jestli procedura již neexistuje v tabulce symbolů na spodu zásobníku(globální kontext)
            var symbolTablesList = symbolTables.ToList();
            if (symbolTablesList.Last().ProcedureExists(procedureName))
            {
                throw new Exception("Procedure " + procedureName + " already exists.");
            }

            //zkontrolujeme jestli se nejedná o předdefinovanou proceduru
            if (builtInProcedures.Contains(procedureName))
            {
                throw new Exception("Procedure " + procedureName + " is built-in.");
            }

            //přidáme proceduru do zásobníku
            symbolTables.Peek().AddProcedure(procedure);
            return null;
        }

        //procedureCall: (ident '.')* ident '(' (expression (',' expression)*)? ')';
        public override object VisitProcedureCall([NotNull] Modula2Parser.ProcedureCallContext context)
        {
            //vytvoříme nový kontext pro proceduru
            symbolTables.Push(new SymbolTable());

            //zjistíme název procedury(veškeré identy oddělené tečkou)
            List<string> procedureNames = new List<string>();
            for (int i = 0; i < context.ident().Length; i++)
            {
                procedureNames.Add(context.ident(i).GetText());
            }

            //list pro tabulky symbolů, aby sme mohli sáhnout do globálního kontextu
            List<SymbolTable> symbolTablesList = symbolTables.ToList();

            //zkontrolujeme jestli procedura existuje v tabulce symbolů na spodu zásobníku(globální kontext)
            if (!symbolTablesList.Last().ProcedureExists(procedureNames[context.ident().Length - 1]))
            {
                if (!builtInProcedures.Contains(procedureNames[context.ident().Length - 1]))
                {
                    throw new Exception("Procedure " + procedureNames[context.ident().Length - 1] + " is not defined.");
                }
            }

            //pokud se jedná o předdefinovanou proceduru WriteOut, vypíšeme hodnotu na jednu řádku
            if (procedureNames[context.ident().Length - 1] == "WriteOut")
            {
                foreach (var parameter in context.expression())
                {
                    Console.Write(VisitExpression(parameter));
                }
                symbolTables.Pop();
                return null;
            }
            //pokud se jedná o předdefinovanou proceduru WriteLine, vypíšeme hodnotu na jednu řádku
            if (procedureNames[context.ident().Length - 1] == "WriteLine")
            {
                Console.WriteLine();
                symbolTables.Pop();
                return null;
            }
            //pokud se jedná o předdefinovanou proceduru ReadIn, načteme hodnotu
            if (procedureNames[context.ident().Length - 1] == "ReadIn")
            {
                string input = Console.ReadLine();
                symbolTables.Pop();
                return input;
            }
            //pokud se jedná o předdefinovanou proceduru random, vygenerujeme náhodné číslo
            if (procedureNames[context.ident().Length - 1] == "Rand")
            {
                //vytvoříme seznam pro parametry 
                List<object?> parametersRand = new List<object?>();
                foreach (var parameter in context.expression())
                {
                    parametersRand.Add(VisitExpression(parameter));
                }
                //pokud má dva parametry, vygenerujeme náhodné číslo v rozsahu
                if (parametersRand.Count == 2)
                {
                    //pokud oba parametry nejsou typu int, vyhodíme chybu
                    if ((parametersRand[0] is int && parametersRand[1] is int))
                    {
                        //pokud je první parametr větší než druhý, vyhodíme chybu
                        if ((int)parametersRand[0] > (int)parametersRand[1])
                        {
                            throw new Exception("Invalid range of random numbers.");
                        }
                        else
                        {
                            Random random = new Random();
                            int randomNumber = random.Next((int)parametersRand[0], (int)parametersRand[1]);
                            symbolTables.Pop();
                            return randomNumber;
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid type of parameters.");
                    }
                }
                else
                {
                    throw new Exception("Invalid number of parameters.");
                }
            }

            //zjistíme proceduru(aktuálně udělané tak, že se bere pouze poslední identifikátor)
            Procedure procedure = symbolTablesList.Last().GetProcedure(procedureNames[context.ident().Length - 1]);

            //zjistíme parametry
            List<object?> parameters = new List<object?>();

            //předělám foreach na for abych mohl získat index a přistupovat tak k procedure.VarFormals
            for (int i = 0; i < context.expression().Length; i++)
            {
                Variable parameter = new Variable();
                parameter.Value = VisitExpression(context.expression(i));
                parameter.Name = procedure.VarFormals[i].Name;
                //pokud parametr existuje jako proměnná v globálním kontextu, přidáme lokálně
                if (symbolTablesList.Last().VariableExists(parameter.Name))
                {
                    symbolTables.Peek().AddVariable(symbolTablesList.Last().GetVariable(parameter.Name));
                }
                else
                {
                    //přidáme parametr do tabulky symbolů
                    symbolTables.Peek().AddVariable(parameter);
                }
                parameters.Add(parameter);
            }

            if (procedure != null)
            {
                //zjistíme jestli máme stejný počet parametrů
                if (procedure.VarFormals.Count == parameters.Count)
                {
                    //zjistíme jestli jsou parametry stejného typu
                    for (int i = 0; i < procedure.VarFormals.Count; i++)
                    {
                        //zjistíme typ parametru
                        DataType parameterType = procedure.VarFormals[i].Type;
                        //zjistíme typ hodnoty
                        Variable parameter = symbolTables.Peek().GetVariable(procedure.VarFormals[i].Name);

                        Type? valueType = null;

                        if (parameter.Value == null)
                        {
                            if (parameter.DataType == DataType.ARRAY && parameterType == DataType.ARRAY)
                            {
                                break;
                            }
                        }
                        else
                        {
                            valueType = parameter.Value.GetType();
                        }
                        //pokud se typy nerovnají, vyhodíme chybu
                        if (
                            !((valueType == typeof(int) && parameterType == DataType.INTEGER)
                            || (valueType == typeof(String) && parameterType == DataType.CHAR)
                            || (valueType == typeof(float) && parameterType == DataType.REAL)
                            || (valueType == typeof(int) && parameterType == DataType.REAL)
                            )
                            )
                        {
                            throw new Exception("Invalid value assigned(probably different data type).");
                        }
                    }
                }
                else
                {
                    throw new Exception("Invalid number of parameters.");
                }
            }
            else
            {
                throw new Exception("Procedure " + procedureNames[context.ident().Length - 1] + " is not defined.");
            }
            //pokud je vše v pořádku, provedeme proceduru
            foreach (var statement in procedure.Body)
            {
                VisitStatement((Modula2Parser.StatementContext)statement);
            }

            //pokud je návratový typ NULL, vracíme NULL
            if (procedure.ReturnType == DataType.NULL)
            {
                symbolTables.Pop();
                return null;
            }
            //pokud není NULL, vracíme hodnotu
            else
            {
                object? returnValue = VisitExpression((Modula2Parser.ExpressionContext)procedure.ReturnExpression);
                symbolTables.Pop();
                return returnValue;
            }
        }

        public override object VisitVarFormal([NotNull] Modula2Parser.VarFormalContext context)
        {
            string varFormalName = context.ident().GetText();
            DataType varFormalDataType = (DataType)VisitType(context.type());
            VarFormal varFormal = new VarFormal();
            varFormal.Name = varFormalName;
            varFormal.Type = varFormalDataType;
            return varFormal;
        }
    }
}
