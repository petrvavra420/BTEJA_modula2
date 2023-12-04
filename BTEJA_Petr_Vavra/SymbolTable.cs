using BTEJA_Petr_Vavra;

public class SymbolTable
{
    private Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
    private Dictionary<string, Procedure> procedures = new Dictionary<string, Procedure>();

    public void AddVariable(Variable variable)
    {
        variables.Add(variable.Name, variable);
    }

    public Variable GetVariable(string name)
    {
        return variables.ContainsKey(name) ? variables[name] : null;
    }

    public void AddProcedure(Procedure procedure)
    {
        procedures.Add(procedure.Name, procedure);
    }

    public Procedure GetProcedure(string name)
    {
        return procedures.ContainsKey(name) ? procedures[name] : null;
    }

    // Další metody pro správu symbolů můžete přidat podle potřeby.
}