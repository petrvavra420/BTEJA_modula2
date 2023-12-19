using BTEJA_Petr_Vavra;

public class SymbolTable
{
    private Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
    private Dictionary<string, Procedure> procedures = new Dictionary<string, Procedure>();

    public SymbolTable()
    {
    }

    // Nový konstruktor s parametrem pro nadřazenou tabulku symbolů
    public SymbolTable(SymbolTable parentTable)
    {
        variables = new Dictionary<string, Variable>();
        // Zkopírovat proměnné z nadřazené tabulky symbolů, pokud existují
        if (parentTable != null)
        {
            foreach (var variable in parentTable.variables)
            {
                variables.Add(variable.Key, variable.Value);
            }
        }
    }


    public void AddVariable(Variable variable)
    {
        variables.Add(variable.Name, variable);
    }

    public Variable GetVariable(string name)
    {
        return variables.ContainsKey(name) ? variables[name] : null;
    }

    public bool VariableExists(string name)
    {
        return variables.ContainsKey(name);
    }

    public bool ProcedureExists(string name)
    {
        return procedures.ContainsKey(name);
    }

    public void AddProcedure(Procedure procedure)
    {
        procedures.Add(procedure.Name, procedure);
    }

    public Procedure GetProcedure(string name)
    {
        return procedures.ContainsKey(name) ? procedures[name] : null;
    }

}