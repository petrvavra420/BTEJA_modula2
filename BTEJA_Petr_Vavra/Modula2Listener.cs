//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Modula2.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="Modula2Parser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface IModula2Listener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProgram([NotNull] Modula2Parser.ProgramContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProgram([NotNull] Modula2Parser.ProgramContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] Modula2Parser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] Modula2Parser.StatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.varStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVarStatement([NotNull] Modula2Parser.VarStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.varStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVarStatement([NotNull] Modula2Parser.VarStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignment([NotNull] Modula2Parser.AssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignment([NotNull] Modula2Parser.AssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfStatement([NotNull] Modula2Parser.IfStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfStatement([NotNull] Modula2Parser.IfStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.ifBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfBlock([NotNull] Modula2Parser.IfBlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.ifBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfBlock([NotNull] Modula2Parser.IfBlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.elseIfBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterElseIfBlock([NotNull] Modula2Parser.ElseIfBlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.elseIfBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitElseIfBlock([NotNull] Modula2Parser.ElseIfBlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.elseBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterElseBlock([NotNull] Modula2Parser.ElseBlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.elseBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitElseBlock([NotNull] Modula2Parser.ElseBlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.forStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterForStatement([NotNull] Modula2Parser.ForStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.forStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitForStatement([NotNull] Modula2Parser.ForStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.procedureDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProcedureDeclaration([NotNull] Modula2Parser.ProcedureDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.procedureDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProcedureDeclaration([NotNull] Modula2Parser.ProcedureDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.procedureCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProcedureCall([NotNull] Modula2Parser.ProcedureCallContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.procedureCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProcedureCall([NotNull] Modula2Parser.ProcedureCallContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.varFormal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVarFormal([NotNull] Modula2Parser.VarFormalContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.varFormal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVarFormal([NotNull] Modula2Parser.VarFormalContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.condition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCondition([NotNull] Modula2Parser.ConditionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.condition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCondition([NotNull] Modula2Parser.ConditionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpression([NotNull] Modula2Parser.ExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpression([NotNull] Modula2Parser.ExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTerm([NotNull] Modula2Parser.TermContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTerm([NotNull] Modula2Parser.TermContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.factor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFactor([NotNull] Modula2Parser.FactorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.factor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFactor([NotNull] Modula2Parser.FactorContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.arrayAccess"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArrayAccess([NotNull] Modula2Parser.ArrayAccessContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.arrayAccess"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArrayAccess([NotNull] Modula2Parser.ArrayAccessContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.arrayIndexAccess"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArrayIndexAccess([NotNull] Modula2Parser.ArrayIndexAccessContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.arrayIndexAccess"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArrayIndexAccess([NotNull] Modula2Parser.ArrayIndexAccessContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.ident"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdent([NotNull] Modula2Parser.IdentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.ident"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdent([NotNull] Modula2Parser.IdentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterType([NotNull] Modula2Parser.TypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitType([NotNull] Modula2Parser.TypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.array"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArray([NotNull] Modula2Parser.ArrayContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.array"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArray([NotNull] Modula2Parser.ArrayContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.realNumber"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRealNumber([NotNull] Modula2Parser.RealNumberContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.realNumber"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRealNumber([NotNull] Modula2Parser.RealNumberContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNumber([NotNull] Modula2Parser.NumberContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNumber([NotNull] Modula2Parser.NumberContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.addOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAddOp([NotNull] Modula2Parser.AddOpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.addOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAddOp([NotNull] Modula2Parser.AddOpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.multOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMultOp([NotNull] Modula2Parser.MultOpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.multOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMultOp([NotNull] Modula2Parser.MultOpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.character"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCharacter([NotNull] Modula2Parser.CharacterContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.character"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCharacter([NotNull] Modula2Parser.CharacterContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Modula2Parser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterString([NotNull] Modula2Parser.StringContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Modula2Parser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitString([NotNull] Modula2Parser.StringContext context);
}
