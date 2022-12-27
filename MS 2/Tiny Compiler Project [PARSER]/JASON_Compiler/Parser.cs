using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Compiler
{

    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }



    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            Flag.isOfr = false;
            this.TokenStream = TokenStream;
            // starting sympol
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }


        


        // ================================================================================================================
        // Implement your production rules here.
        /*
            RULES :
            - start with starting symbol.
            - create node function for each non terminal [Procedure].
            - for each procedure implement its R.H.S.
            - if you find terminal with small letter [Token] then add match function. 
            - else if you find NON terminal with capital lette [Procedure] then add this function.
            - in case of the function contains or then check for prefix of each segement using if statement.
                    if (TokenStream[InputPointer].token_type == Token_Class.)
            - in case of epslon return null.

            RULES for clean code : 
            - each node start with capital letter. 
                Node Program(){}
            - the created object inside node start with small letter and the passed name is the same as node function name.
                Node program = new Node("Program");
        */


        // ============================================== Gaber start =======================================================




        //=================================================Helper Functions begin================================================



        public bool checkStatment()
        {



            Token_Class tmp = TokenStream[InputPointer].token_type;

            Token_Class tmp2 = TokenStream[InputPointer + 1].token_type;
            if (tmp == Token_Class.Read || tmp == Token_Class.Write ||
                tmp2 == Token_Class.AssignmentOp || tmp == Token_Class.Int ||
                tmp == Token_Class.Float || tmp == Token_Class.String || tmp == Token_Class.If || tmp == Token_Class.Repeat || tmp2 == Token_Class.LPracket)
            {
                return true;
            }


            return false;
        }

        public bool checkTerm()
        {

            Token_Class tmp = TokenStream[InputPointer].token_type;



            if (tmp == Token_Class.Number || (tmp == Token_Class.Idenifier && true && TokenStream[InputPointer + 1].token_type == Token_Class.LParanthesis) ||
                tmp == Token_Class.Idenifier)
            {
                return true;
            }




            return false;
        }

        public bool checkEquation()
        {

            Token_Class tmp = TokenStream[InputPointer].token_type;


            if (tmp == Token_Class.LParanthesis || checkTerm() == true)
            {
                return true;
            }



            return false;

        }


        public class Flag
        {
            public static bool isOfr = false; // out of range

            public static bool isSpecial = false;
        }

        //================================================Helper Functions End====================================================
        Node Program()
        {
            // completed
            // Program → Function_statements Main_function

            if (Flag.isOfr == true) { return null; }


            Node program = new Node("Program");
            program.Children.Add(Function_statements());
            program.Children.Add(Main_function());
            return program;
        }

        Node Function_statements()
        {
            // completed
            // Function_statements → Function_statement Function_statementsDash

            if (Flag.isOfr == true) { return null; }


            Node function_statements = new Node("Function_statements");
            function_statements.Children.Add(Function_statement());
            function_statements.Children.Add(Function_statements_Dash());
            return function_statements;
        }



        Node Function_statement()
        {
            // completed
            // Function_statement → Function_declaration Function_body | 𝜀

            if (Flag.isOfr == true) { return null; }
           


            Node function_statement = new Node("Function_statement");
            
            

            if ((InputPointer + 1 >= TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float) || TokenStream[InputPointer].token_type == Token_Class.String) || (TokenStream[InputPointer].token_type == Token_Class.Int && TokenStream[InputPointer + 1].token_type != Token_Class.Main) || (TokenStream[InputPointer].token_type == Token_Class.Float && TokenStream[InputPointer + 1].token_type != Token_Class.Main) || (TokenStream[InputPointer].token_type == Token_Class.String && TokenStream[InputPointer + 1].token_type != Token_Class.Main))
            {



                if (InputPointer + 1 >= TokenStream.Count && ( TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float) || TokenStream[InputPointer].token_type == Token_Class.String) { Flag.isSpecial = true; }


                function_statement.Children.Add(Function_declaration());
                function_statement.Children.Add(Function_body());
                return function_statement;
            }
            else
            {
                return null;
            }

        }

        Node Function_statements_Dash()
        {
            // completed
            // Function_statements_Dash → Function_statement Function_statements’ | 𝜀

            if (Flag.isOfr == true) { return null; }


            Node function_statements_Dash = new Node("Function_statements_Dash");
            if ((TokenStream[InputPointer].token_type == Token_Class.Int && TokenStream[InputPointer + 1].token_type != Token_Class.Main) || (TokenStream[InputPointer].token_type == Token_Class.Float && TokenStream[InputPointer + 1].token_type != Token_Class.Main) || (TokenStream[InputPointer].token_type == Token_Class.String && TokenStream[InputPointer + 1].token_type != Token_Class.Main))
            {
                function_statements_Dash.Children.Add(Function_statement());
                function_statements_Dash.Children.Add(Function_statements_Dash());
                return function_statements_Dash;
            }
            return null;
        }

        Node Function_declaration()
        {
            // completed
            // Function _declaration → Datatype identifier (Parameters)

            if (Flag.isOfr == true) { return null; }


            Node function_declaration = new Node("Function_declaration");
            function_declaration.Children.Add(Datatype());
            function_declaration.Children.Add(match(Token_Class.Idenifier));
            function_declaration.Children.Add(match(Token_Class.LParanthesis));
            function_declaration.Children.Add(Parameters());
            function_declaration.Children.Add(match(Token_Class.RParanthesis));
            return function_declaration;
        }

        Node Function_body()
        {
            // completed
            // Function _body → { Statements Return_statement }

            if (Flag.isOfr == true) { return null; }


            Node function_body = new Node("Function_body");
            function_body.Children.Add(match(Token_Class.LPracket));
            function_body.Children.Add(Statements());
            function_body.Children.Add(Return_statement());
            function_body.Children.Add(match(Token_Class.RPracket));
            return function_body;
        }


        Node Main_function()
        {
            // completed
            // Main_function → Datatype main() Function_body

            if (Flag.isOfr == true) { return null; }


            Node main_function = new Node("Main_function");
            main_function.Children.Add(Datatype());
            main_function.Children.Add(match(Token_Class.Main));
            main_function.Children.Add(match(Token_Class.LParanthesis));
            main_function.Children.Add(match(Token_Class.RParanthesis));
            main_function.Children.Add(Function_body());
            return main_function;
        }

        Node Datatype()
        {
            // Completed
            // Datatype → int | float | string

            if (Flag.isOfr == true) { return null; }


            Node datatype = new Node("Datatype");
            if (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String)
            {
                datatype.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            return datatype;
        }

        Node Ops()
        {
            Node ops = new Node("Ops");

            if (Flag.isOfr == true) { return null; }


            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp || TokenStream[InputPointer].token_type == Token_Class.DivideOp || TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
            {
                ops.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            return ops;
        }

        Node Statements()
        {
            // completed
            // Statements → Statement Statements’

            if (Flag.isOfr == true) { return null; }


            Node statements = new Node("Statements");
            statements.Children.Add(Statement());
            statements.Children.Add(StatementsDash());
            return statements;
        }

        Node Statement()
        {
            // Statement → Write_statement | Read_statement | Assignment_statement | Declaration_statement | If_statement |Repeat_statement | Function_call


            if (Flag.isOfr == true) { return null; }



            Node statement = new Node("Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                statement.Children.Add(Write_statement());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                statement.Children.Add(Read_statement());
            }
            else if (TokenStream[InputPointer + 1].token_type == Token_Class.AssignmentOp)
            {
                statement.Children.Add(Assignment_statement());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String)
            {
                statement.Children.Add(Declaration_statement());

            }
            else if (TokenStream[InputPointer].token_type == Token_Class.If)
            {
                statement.Children.Add(If_statement());

            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Repeat)
            {
                statement.Children.Add(Repeat_statement());

            }
            else if (TokenStream[InputPointer + 1].token_type == Token_Class.LPracket) // ***
            {
                statement.Children.Add(Function_call());

            }

            return statement;
        }

        Node StatementsDash()
        {
            // Statements’ → Statement Statements’ | 𝜀



            if (Flag.isOfr == true) { return null; }


            Node statementsDash = new Node("StatementsDash");

            if (checkStatment() == true)
            {

                statementsDash.Children.Add(Statement());
                statementsDash.Children.Add(StatementsDash());

                return statementsDash;
            }

            return null;
        }

        Node Return_statement()
        {
            // completed    
            // Return_statement → return Expression ;


            if (Flag.isOfr == true) { return null; }



            Node return_statement = new Node("Return_statement");
            return_statement.Children.Add(match(Token_Class.Return));
            return_statement.Children.Add(Expression());
            return_statement.Children.Add(match(Token_Class.Semicolon));
            return return_statement;
        }

        Node Ret_statement()
        {
            // completed
            // Ret_statement → Return_statement | 𝜀


            if (Flag.isOfr == true) { return null; }


            Node ret_statement = new Node("Ret_statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Return)
            {
                ret_statement.Children.Add(Return_statement());
                return ret_statement;
            }
            else
            {
                return null;
            }
        }
        Node Write_statement()
        {
            // completed    
            // Write_statement → write Write_statement_Dash


            if (Flag.isOfr == true) { return null; }


            Node write_statement = new Node("Write_statement");
            write_statement.Children.Add(match(Token_Class.Write));
            write_statement.Children.Add(Write_statement_Dash());
            return write_statement;
        }

        Node Read_statement()
        {
            // completed
            // Read_statement → read identifier ;


            if (Flag.isOfr == true) { return null; }


            Node read_statement = new Node("Read_statement");
            read_statement.Children.Add(match(Token_Class.Read));
            read_statement.Children.Add(match(Token_Class.Idenifier));
            read_statement.Children.Add(match(Token_Class.Semicolon));
            return read_statement;
        }

        Node Write_statement_Dash()
        {
            // completed
            // Write_statement’ → Expression ; | endl ;



            if (Flag.isOfr == true) { return null; }


            Node write_statement_dash = new Node("Write_statement_Dash");
            if (TokenStream[InputPointer].token_type == Token_Class.StringLine || checkEquation() == true)
            {
                write_statement_dash.Children.Add(Expression());
                write_statement_dash.Children.Add(match(Token_Class.Semicolon));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Endl)
            {
                write_statement_dash.Children.Add(match(Token_Class.Endl));
                write_statement_dash.Children.Add(match(Token_Class.Semicolon));
            }
            return write_statement_dash;
        }

        Node Expression()
        {
            // Expression → stringLine | Term | Equation

            if (Flag.isOfr == true) { return null; }



            Node expression = new Node("Expression");

            if (TokenStream[InputPointer].token_type == Token_Class.StringLine)
            {
                expression.Children.Add(match(Token_Class.StringLine));

            }
            else if (checkEquation() == true)
            {
                expression.Children.Add(Equation());
            }
            else if (checkTerm() == true)
            {
                expression.Children.Add(Term());
            }

            return expression;
        }

        Node Term()
        {
            // completed
            // Term → number | identifier | Function_call


            if (Flag.isOfr == true) { return null; }


            Node term = new Node("Term");
            if (TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                term.Children.Add(match(Token_Class.Number));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier && true/*check for i*/ && TokenStream[InputPointer + 1].token_type == Token_Class.LParanthesis)
            {
                term.Children.Add(Function_call());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                term.Children.Add(match(Token_Class.Idenifier));
            }
            return term;
        }

        Node Function_call()
        {
            // completed
            // Function_call → identifier (Identifier_list | 𝜀) 

            if (Flag.isOfr == true) { return null; }


            Node function_call = new Node("Function_call");
            function_call.Children.Add(match(Token_Class.Idenifier));
            function_call.Children.Add(match(Token_Class.LParanthesis));
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                function_call.Children.Add(match(Token_Class.Idenifier));
            }
            else
            {
                function_call.Children.Add(match(Token_Class.RParanthesis));
                return function_call;
            }
            function_call.Children.Add(match(Token_Class.RParanthesis));
            return function_call;
        }

        Node Identifier_list()
        {
            // completed
            // Identifier_list → Id Identifier_list_Dash


            if (Flag.isOfr == true) { return null; }


            Node identifier_list = new Node("Identifier_list");
            identifier_list.Children.Add(Id());
            identifier_list.Children.Add(Identifier_list_Dash());
            return identifier_list;
        }

        Node Identifier_list_Dash()
        {
            // completed
            // Identifier_list’ → , Id Identifier_list’ | eplson


            if (Flag.isOfr == true) { return null; }



            Node identifier_list_Dash = new Node("Identifier_list_Dash");
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                identifier_list_Dash.Children.Add(match(Token_Class.Comma));
                identifier_list_Dash.Children.Add(Id());
                identifier_list_Dash.Children.Add(Identifier_list_Dash());
                return identifier_list_Dash;
            }
            else
            {
                return null;
            }
        }

        Node ConditionOp()
        {
            // completed
            // ConditionOp → notEqualOp | equalOp | lessThanOp |greaterThanOp


            if (Flag.isOfr == true) { return null; }



            Node conditionOp = new Node("ConditionOp");
            if (TokenStream[InputPointer].token_type == Token_Class.NotEqualOp)
            {
                conditionOp.Children.Add(match(Token_Class.NotEqualOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.ConditionEqualOp)
            {
                conditionOp.Children.Add(match(Token_Class.ConditionEqualOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.LessThanOp)
            {
                conditionOp.Children.Add(match(Token_Class.LessThanOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp)
            {
                conditionOp.Children.Add(match(Token_Class.GreaterThanOp));
            }
            return conditionOp;
        }

        Node BooleanOp()
        {
            // completed
            //  BooleanOp → andOp | orOp


            if (Flag.isOfr == true) { return null; }



            Node booleanOp = new Node("BooleanOp");
            if (TokenStream[InputPointer].token_type == Token_Class.And || TokenStream[InputPointer].token_type == Token_Class.Or)
            {
                booleanOp.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            return booleanOp;
        }


        Node Equation()
        {


            if (Flag.isOfr == true) { return null; }


            Node equation = new Node("Equation");

            if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                equation.Children.Add(match(Token_Class.LParanthesis));
                equation.Children.Add(Equation());

                equation.Children.Add(match(Token_Class.RParanthesis));
                equation.Children.Add(Equation_Dash());
            }
            else
            {
                equation.Children.Add(Term());
                equation.Children.Add(Equation_Dash());

            }

            return equation;
        }

        Node Equation_Dash()
        {

            if (Flag.isOfr == true) { return null; }


            Node equation_dash = new Node("Equation_Dash");

            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp || TokenStream[InputPointer].token_type == Token_Class.DivideOp || TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
            {

                equation_dash.Children.Add(Ops());
                equation_dash.Children.Add(Equation());
                equation_dash.Children.Add(Equation_Dash());


                return equation_dash;

            }


            return null;
        }

        Node Condition()
        {
            // completed
            // Condition → identifier ConditionOp Term


            if (Flag.isOfr == true) { return null; }



            Node condition = new Node("Condition");
            condition.Children.Add(match(Token_Class.Idenifier));
            condition.Children.Add(ConditionOp());
            condition.Children.Add(Term());
            return condition;
        }

        Node Condition_statement()
        {
            // completed
            // Condition_statement → Condition Condition_statement_Dash


            if (Flag.isOfr == true) { return null; }



            Node condition_statement = new Node("Condition_statement");
            condition_statement.Children.Add(Condition());
            condition_statement.Children.Add(Condition_statement_Dash());
            return condition_statement;
        }

        Node Condition_statement_Dash()
        {
            // completed
            // Condition_statement’ → BooleanOp Condition Condition_statement’| 𝜀


            if (Flag.isOfr == true) { return null; }



            Node condition_statement_Dash = new Node("Condition_statement_Dash");
            if (TokenStream[InputPointer].token_type == Token_Class.And || TokenStream[InputPointer].token_type == Token_Class.Or)
            {
                condition_statement_Dash.Children.Add(BooleanOp());
                condition_statement_Dash.Children.Add(Condition());
                condition_statement_Dash.Children.Add(Condition_statement_Dash());
                return condition_statement_Dash;
            }
            return null;
        }

        Node Id()
        {
            // completed
            // Id → identifier Id’


            if (Flag.isOfr == true) { return null; }


            Node id = new Node("Id");
            id.Children.Add(match(Token_Class.Idenifier));
            id.Children.Add(Id_Dash());
            return id;
        }

        Node Id_Dash()
        {
            // completed
            // Id_Dash → 𝜀 | assignmentOp Expression



            if (Flag.isOfr == true) { return null; }


            Node id_Dash = new Node("Id_Dash");
            if (TokenStream[InputPointer].token_type == Token_Class.AssignmentOp)
            {
                id_Dash.Children.Add(match(Token_Class.AssignmentOp));
                id_Dash.Children.Add(Expression());
                return id_Dash;
            }
            else
            {
                return null;
            }
        }
        // ============================================== Gaber end =======================================================


        // ============================================== Nour start =======================================================

        Node Assignment_statement()
        {
            // completed
            // Assignment_statement → identifier assignmentOp Expression ;


            if (Flag.isOfr == true) { return null; }


            Node assignment_statement = new Node("Assignment_statement");

            assignment_statement.Children.Add(match(Token_Class.Idenifier));
            assignment_statement.Children.Add(match(Token_Class.AssignmentOp));
            assignment_statement.Children.Add(Expression());
            assignment_statement.Children.Add(match(Token_Class.Semicolon));
            return assignment_statement;
        }

        Node Declaration_statement()
        {
            // completed
            // Declaration_statement → Datatype Identifier_list ;


            if (Flag.isOfr == true) { return null; }


            Node declaration_statement = new Node("Declaration_statement");
            declaration_statement.Children.Add(Datatype());
            declaration_statement.Children.Add(Identifier_list());
            declaration_statement.Children.Add(match(Token_Class.Semicolon));
            return declaration_statement;
        }

        Node Else_statement()
        {
            // completed
            // Else_statement → else Statements end | 𝜀


            if (Flag.isOfr == true) { return null; }


            Node else_statement = new Node("Else_statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                else_statement.Children.Add(match(Token_Class.Else));
                else_statement.Children.Add(Statements());
                else_statement.Children.Add(match(Token_Class.End));
                return else_statement;
            }
            else
            {
                return null;
            }
        }

        Node Else_if_statement()
        {
            // completed
            // Else_if_statement → elseif Condition_statement then Statements Ret_statement Else_statement end | �


            if (Flag.isOfr == true) { return null; }


            Node else_if_statement = new Node("Else_if_statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Elseif)
            {
                else_if_statement.Children.Add(match(Token_Class.Elseif));
                else_if_statement.Children.Add(Condition_statement());
                else_if_statement.Children.Add(match(Token_Class.Then));
                else_if_statement.Children.Add(Statements());
                else_if_statement.Children.Add(Ret_statement());
                else_if_statement.Children.Add(Else_statement());
                else_if_statement.Children.Add(match(Token_Class.End));
                return else_if_statement;
            }
            else
            {
                return null;
            }
        }

        Node If_statement()
        {
            // completed
            // If_statement → if Condition_statement then Statements Ret_statement Else_if_statement Else_statement end


            if (Flag.isOfr == true) { return null; }


            Node if_statement = new Node("If_statement");
            if_statement.Children.Add(match(Token_Class.If));
            if_statement.Children.Add(Condition_statement());
            if_statement.Children.Add(match(Token_Class.Then));
            if_statement.Children.Add(Statements());
            if_statement.Children.Add(Ret_statement());
            if_statement.Children.Add(Else_if_statement());
            if_statement.Children.Add(Else_statement());
            if_statement.Children.Add(match(Token_Class.End));
            return if_statement;
        }

        Node Repeat_statement()
        {
            // completed
            // Repeat_statement → repeat Statements until Condition_statement


            if (Flag.isOfr == true) { return null; }


            Node repeat_statement = new Node("Repeat_statement");
            repeat_statement.Children.Add(match(Token_Class.Repeat));
            repeat_statement.Children.Add(Statements());
            repeat_statement.Children.Add(match(Token_Class.Until));
            repeat_statement.Children.Add(Condition_statement());
            return repeat_statement;
        }

        // ============================================== Nour end =======================================================

        // ============================================== Ruq start =======================================================
        Node Parameter()
        {

            if (Flag.isOfr == true) { return null; }


            Node parameter = new Node("Parameter");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String)
                {
                    parameter.Children.Add(Datatype());
                    parameter.Children.Add(match(Token_Class.Idenifier));
                    return parameter;
                }
                else
                    return null;
            }
            else
            {
                // to be handeld 
                //Errors.Error_List.Add("Parsing Error: Expected "
                //+ ExpectedToken.ToString() + "\r\n");
                //InputPointer++;
                return null;
            }

        }
        Node Parameter_Dash()
        {

            if (Flag.isOfr == true) { return null; }



            Node parameter_Dash = new Node("Parameter_Dash");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Comma)
                {
                    parameter_Dash.Children.Add(match(Token_Class.Comma));
                    parameter_Dash.Children.Add(Parameter());
                    parameter_Dash.Children.Add(Parameter_Dash());
                }
                return null;
            }
            else
            {
                // to be handeld 
                //Errors.Error_List.Add("Parsing Error: Expected "
                //+ ExpectedToken.ToString() + "\r\n");
                //InputPointer++;
                return null;
            }
        }
        Node Parameters()
        {

            if (Flag.isOfr == true) { return null; }



            Node parameters = new Node("Parameters");
            parameters.Children.Add(Parameter());
            parameters.Children.Add(Parameter_Dash());
            return parameters;
        }
        // ============================================== Ruq end =========================================================
        // ================================================================================================================
        // match : deal with tokens 
        public Node match(Token_Class ExpectedToken)
        {

            if (Flag.isOfr == true && Flag.isSpecial == false) { return null; }

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;

                    if (InputPointer >= TokenStream.Count)
                    {
                        Flag.isOfr = true;
                    }


                    Node newNode = new Node(ExpectedToken.ToString());
                    return newNode;
                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;

                    if (InputPointer >= TokenStream.Count)
                    {
                       Flag.isOfr = true;

                    }

                   

                    

                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;

              

                if (Flag.isSpecial == true) { Flag.isSpecial = false; }

                return null;
            }
        }

        // ================================================================================================================
        // Print Parse Tree
        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
