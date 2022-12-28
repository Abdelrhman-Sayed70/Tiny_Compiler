using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Token_Class
{
    Begin, Call, Declare, End, Do, Else, EndIf, EndUntil, EndWhile, If, Integer,
    Parameters, Procedure, Program, Read, Real, Set, Then, Until, While, Write,
    Dot, Semicolon, Comma, LParanthesis, RParanthesis, EqualOp, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,
    Idenifier, Constant
}
namespace JASON_Compiler
{
    

    public class Token
    {
       public string lex;
       public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("begin", Token_Class.Begin);
            ReservedWords.Add("call", Token_Class.Call);
            ReservedWords.Add("declare", Token_Class.Declare);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("do", Token_Class.Do);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("endif", Token_Class.EndIf);
            ReservedWords.Add("enduntil", Token_Class.EndUntil);
            ReservedWords.Add("endwhile", Token_Class.EndWhile);
            ReservedWords.Add("integer", Token_Class.Integer);
            ReservedWords.Add("parameters", Token_Class.Parameters);
            ReservedWords.Add("procedure", Token_Class.Procedure);
            ReservedWords.Add("program", Token_Class.Program);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("real", Token_Class.Real);
            ReservedWords.Add("set", Token_Class.Set);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("while", Token_Class.While);
            ReservedWords.Add("write", Token_Class.Write);

            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<",Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("!", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            


        }

    public void StartScanning(string SourceCode)
        {
            for(int i=0; i<SourceCode.Length;i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

                if (CurrentChar >= 'A' && CurrentChar <= 'z') //if you read a character
                {
                   j = i + 1;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];

                        while ((CurrentChar >= 'A' && CurrentChar <= 'z') || CurrentChar >= '0' && CurrentChar <= '9')
                        {

                            CurrentLexeme = CurrentLexeme + CurrentChar.ToString();

                            j++;
                            CurrentChar = SourceCode[j];

                        }
                    }
                    FindTokenClass(CurrentLexeme);

                    i = j-1;
                }

                else if(CurrentChar >= '0' && CurrentChar <= '9')
                {
                    j = i + 1;
                    //CurrentLexeme = CurrentLexeme + CurrentChar.ToString();
                    CurrentChar = SourceCode[j];

                    while ((CurrentChar >= '0' && CurrentChar <= '9') || CurrentChar.Equals('.'))
                    {
                        CurrentLexeme = CurrentLexeme + CurrentChar.ToString();

                        j++;
                       if (j<SourceCode.Length)
                        CurrentChar = SourceCode[j];
                       
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j-1;
                }
                else if(CurrentChar == '{')
                {
                    j++;
                    CurrentChar = SourceCode[j];
                    while(CurrentChar != '}')
                    {
                        j++;
                        CurrentChar = SourceCode[j];
                    }
                    i = j;
                }
                else
                {
                    FindTokenClass(CurrentLexeme);
                }
            }
            
            JASON_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                TC = ReservedWords[Lex];
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }
            //Is it an identifier?
            else if(isIdentifier(Lex))
            {
                TC = Token_Class.Idenifier;
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }

            else if (Operators.ContainsKey(Lex))
            {
                TC = Operators[Lex];
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }

            //Is it a Constant?
            else if(isConstant(Lex))
            {
                TC = Token_Class.Constant;
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }
            //Is it an operator?

            else
            {
                Errors.Error_List.Add("Unidentified Token "+ Lex );
            }


        }

    

        bool isIdentifier(string lex)
        {
            bool isValid=true;
            if (!(lex[0] >= 'A' && lex[0] <= 'z'))
            { isValid = false; }

            else
            {
                for (int i = 1; i < lex.Length; i++)
                {
                    if(!(lex[i] >= 'A' && lex[i] <= 'z')
                        || (lex[i] >= '0' && lex[i] <= '9'))
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = true;
          
                for (int i = 0; i < lex.Length; i++)
                {
                    if (!((lex[i] >= '0' && lex[i] <= '9') || lex[i]=='.'))
                    {
                        isValid = false;
                    }
                }
            return isValid;
        }
    }
}
