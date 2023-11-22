grammar Modula2; 

program: (statement ';')+ EOF;

statement: varStatement | assignment | ifStatement | forStatement | procedureDeclaration | procedureCall;

varStatement: 'VAR' ident ':' type (':=' (expression | procedureCall))?;

assignment: ident ('[' expression ']')? ':=' (expression | procedureCall);

ifStatement: 'IF' condition 'THEN' ifBlock ('ELSIF' condition 'THEN' elseIfBlock)* ('ELSE' elseBlock)? 'END';

ifBlock:  (statement ';')+;
elseIfBLock: (statement ';')+;
elseBlock: (statement ';')+;

forStatement: 'FOR' ident ':=' expression 'TO' expression ('BY' expression)? 'DO' (statement ';')+ 'END';

procedureDeclaration: 'PROCEDURE' ident ( '(' varFormal (';' varFormal)* ')' )? (':' type)? ';' 'BEGIN' (statement ';')+ ('RETURN' expression ';')? 'END' ident;

procedureCall: (ident '.')* ident '(' (expression (',' expression)*)? ')';

varFormal: ident ':' type;

condition: expression ('>=' | '<=' | '>' | '<' | '=' | '#') expression | ident | ('1' | '0');

expression: term ((addOp) term)*;

term: factor ((multOp) factor)*;

factor: procedureCall | string | character | realNumber | number | ident ('[' expression ']')? | '(' expression ')';

ident: IDENTIFIER+;  
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;

type: 'INTEGER' | 'REAL' | 'CHAR' | array;

array: 'ARRAY' 'OF' type | 'ARRAY' expression 'OF' type;

realNumber: number '.' number;

number: '1' | '0' | ('-' | '+')? DIGIT;
DIGIT: '0'..'9'+;


addOp: '+' | '-';
multOp: '*' | '/';



character: '"' ~('"')* '"';

string: '\'' ~'\''* '\'';
WS : [ \t\r\n]+ -> skip ;
