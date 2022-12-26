using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Token_Class
{
    Main, Int, Float, String, Read, Write, Repeat, Until, If, Elseif, Else, Then, Return, Endl,End,
    Colon, And, Or, Dot, Semicolon, Comma, LParanthesis, RParanthesis, AssignmentOp, NotEqualOp, ConditionEqualOp, LessThanOp, GreaterThanOp,
    PlusOp, MinusOp, MultiplyOp, DivideOp, Idenifier, IntNumber, FloatNumber, Number, LPracket, RPracket
}
namespace Tiny_Compiler
{


    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>(); // list will be displayed
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("main", Token_Class.Main);
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add(";", Token_Class.Semicolon);


            Operators.Add(".", Token_Class.Dot);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LPracket);
            Operators.Add("}", Token_Class.RPracket);
            Operators.Add("=", Token_Class.ConditionEqualOp);
            Operators.Add(":=", Token_Class.AssignmentOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("&&", Token_Class.And);
            Operators.Add("||", Token_Class.Or);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);

        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[j];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n') { continue; }

                //------------------------------- IDENTIFIER & RESERVED -------------------------------//
                if ((CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= 'a' && CurrentChar <= 'z')) // in case(_res -> _:error , res:id)
                //if (CurrentChar >= 'A' && CurrentChar <= 'z')
                {
                    while (true)
                    {
                        j++;
                        if (j == SourceCode.Length) { i = j; break; }

                        CurrentChar = SourceCode[j];
                        if ((CurrentChar >= 'A' && CurrentChar <= 'z') || (CurrentChar >= '0' && CurrentChar <= '9'))
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        else
                        {
                            i = j - 1; break;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                }

                // ------------------------------- NUMBER -------------------------------// 
                else if ((CurrentChar >= '0' && CurrentChar <= '9'))
                {
                    while (true)
                    {
                        j++;
                        if (j == SourceCode.Length) { i = j; break; }
                        CurrentChar = SourceCode[j];

                        if (CurrentChar == ' ' || CurrentChar == '+' || CurrentChar == ':' || CurrentChar == '=' ||
                            CurrentChar == '>' || CurrentChar == '<' || CurrentChar == '*' || CurrentChar == '-' ||
                            CurrentChar == '/' || CurrentChar == ';' || CurrentChar == ')' || CurrentChar == ',' || CurrentChar == '\n' || CurrentChar == '\r')
                        {
                            i = j - 1; break;
                        }
                        CurrentLexeme += CurrentChar.ToString();
                    }
                    FindTokenClass(CurrentLexeme);
                }

                //------------------------------- COMMENT -------------------------------//
                else if (CurrentChar == '/' && j + 1 < SourceCode.Length && SourceCode[j + 1] == '*')
                {

                    while ((CurrentChar != '*' || SourceCode[j + 1] != '/'))
                    {
                        j++;
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();

                        if (j == SourceCode.Length - 1)
                        {
                            FindTokenClass(CurrentLexeme);
                            break;
                        }
                    }
                    i = j + 1;
                }

                //------------------------------- STRING -------------------------------//
                else if (CurrentChar == '\"')
                {
                    // String
                    j++;
                    CurrentChar = SourceCode[j];
                    CurrentLexeme += CurrentChar.ToString();
                    while (CurrentChar != '\"')
                    {
                        // "abc"
                        if (j == SourceCode.Length - 1)
                        {
                            FindTokenClass(CurrentLexeme);
                            break;
                        }
                        j++;
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                    }
                    i = j;
                }

                //------------------------------- BOOLEAN_OPERATOR [&&] -------------------------------//
                else if (CurrentChar == '&')
                {
                    bool ok = true;
                    j++;
                    if (j == SourceCode.Length) { FindTokenClass(CurrentLexeme); ; i = j; ok = false; }
                    if (ok)
                    {
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '&')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                }

                //------------------------------- BOOLEAN_OPERATOR[:=] --------------------------------------//
                else if (CurrentChar == '|')
                {
                    bool ok = true;
                    j++;
                    if (j == SourceCode.Length) { FindTokenClass(CurrentLexeme); i = j; ok = false; }
                    if (ok)
                    {
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '|')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                }

                //------------------------------- CONDITION_OPERATORS [<> <=] -------------------------------//
                else if (CurrentChar == '<')
                {
                    bool ok = true;
                    j++;
                    if (j == SourceCode.Length) { FindTokenClass(CurrentLexeme); i = j; ok = false; }
                    if (ok)
                    {
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '=')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        else if (CurrentChar == '>')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                }

                //------------------------------- . ----------------------------//
                else if (CurrentChar == '.')
                {
                    while (true)
                    {
                        j++;
                        if (j == SourceCode.Length) { i = j; break; }

                        CurrentChar = SourceCode[j];
                        if ((CurrentChar >= '0' && CurrentChar <= '9'))
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        else
                        {
                            i = j - 1; break;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                }

                //------------------------------- ASSIGNMENT_OPERATOR [:=] --------------------------------------//
                else if (CurrentChar == ':')
                {
                    bool ok = true;
                    j++;
                    if (j == SourceCode.Length) { i = j; ok = false; }
                    if (ok)
                    {
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '=')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                }
                //------------------------------- ERROR, ARITHMATIC_OPERATOR, CONDITION_OPERATOR -------------------------------//
                else
                {
                    // check for [Error, Arithmatic_Operator, Condition_Operator ]
                    FindTokenClass(CurrentLexeme);
                }
            }
            Tiny_Compiler.TokenStream = Tokens;
        }

        void FindTokenClass(string Lex)
        {

            Token_Class TC; // enum contains tokens
            Token Tok = new Token(); // has 2 values lex and token type. we now search for token type for the entered lexeme 
            Tok.lex = Lex;

            // Is it a reserved word? 
            if (ReservedWords.ContainsKey(Lex))
            {
                // [key][value]
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }

            // Is it an operator?
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }

            // Is it an identifier?
            else if (isIdentifier(Lex))
            {
                // Search in REGEX
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }


            // Is it a Number?
            else if (isNumber(Lex))
            {
                // Search in REGEX
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
            }


            // Is it an Int Number?
            else if (isInt(Lex))
            {
                Tok.token_type = Token_Class.IntNumber;
                Tokens.Add(Tok);
            }

            // Is it a Float Number?
            else if (isFloat(Lex))
            {
                Tok.token_type = Token_Class.FloatNumber;
                Tokens.Add(Tok);
            }

         

            // Is it an undefined?
            else
            {
                Errors.Error_List.Add(Lex);
            }
        }

        bool isIdentifier(string lex)
        {
            var regexId = new Regex(@"^([a-z]|[A-Z])([a-z]|[A-Z]|[0-9])*$", RegexOptions.Compiled);
            bool isValid = regexId.IsMatch(lex);
            return isValid;
        }

        bool isNumber(string lex)
        {
            var regexNumber = new Regex(@"^[0-9]+(\.[0-9]+)?$", RegexOptions.Compiled);
            bool isValid = regexNumber.IsMatch(lex);
            return isValid;
        }

        bool isInt(string lex)
        {
            var regexInt = new Regex(@"^[0-9]+$", RegexOptions.Compiled);
            bool isValid = regexInt.IsMatch(lex);
            return isValid;
        }

        bool isFloat(string lex)
        {
            var regexFloat = new Regex(@"^[0-9]+(\.[0-9]+)$", RegexOptions.Compiled);
            bool isValid = regexFloat.IsMatch(lex);
            return isValid;
        }
    }
}