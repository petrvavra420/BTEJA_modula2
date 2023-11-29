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
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="Modula2Parser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface IModula2Visitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProgram([NotNull] Modula2Parser.ProgramContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement([NotNull] Modula2Parser.StatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.varStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarStatement([NotNull] Modula2Parser.VarStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignment([NotNull] Modula2Parser.AssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfStatement([NotNull] Modula2Parser.IfStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.ifBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfBlock([NotNull] Modula2Parser.IfBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.elseIfBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElseIfBlock([NotNull] Modula2Parser.ElseIfBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.elseBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElseBlock([NotNull] Modula2Parser.ElseBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.forStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitForStatement([NotNull] Modula2Parser.ForStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.procedureDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProcedureDeclaration([NotNull] Modula2Parser.ProcedureDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.procedureCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProcedureCall([NotNull] Modula2Parser.ProcedureCallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.varFormal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarFormal([NotNull] Modula2Parser.VarFormalContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.condition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCondition([NotNull] Modula2Parser.ConditionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] Modula2Parser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTerm([NotNull] Modula2Parser.TermContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.factor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFactor([NotNull] Modula2Parser.FactorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.arrayAccess"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArrayAccess([NotNull] Modula2Parser.ArrayAccessContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.arrayIndexAccess"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArrayIndexAccess([NotNull] Modula2Parser.ArrayIndexAccessContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.ident"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIdent([NotNull] Modula2Parser.IdentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType([NotNull] Modula2Parser.TypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.array"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArray([NotNull] Modula2Parser.ArrayContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.realNumber"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRealNumber([NotNull] Modula2Parser.RealNumberContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumber([NotNull] Modula2Parser.NumberContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.addOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAddOp([NotNull] Modula2Parser.AddOpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.multOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultOp([NotNull] Modula2Parser.MultOpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.character"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCharacter([NotNull] Modula2Parser.CharacterContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Modula2Parser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitString([NotNull] Modula2Parser.StringContext context);
}
