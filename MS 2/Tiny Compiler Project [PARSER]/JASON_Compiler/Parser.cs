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
        public  Node root;
        
        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
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

        Node Program()
        {
            // completed
            // Program → Function_statements Main_function
            Node program = new Node("Program");
            program.Children.Add(Function_statements());
            program.Children.Add(Main_function());
            return program;
        }
        
        Node Function_statements()
        {
            // Function_statements → Function_statement Function_statementsDash
            Node function_statements = new Node("Function_statements");
            // implement the function
            return function_statements;
        }

        Node Main_function()
        {
            // completed
            // Main_function → Datatype main() Function_body
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
            Node datatype = new Node("Datatype");
            if (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float|| TokenStream[InputPointer].token_type == Token_Class.String)
            {
                datatype.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            return datatype;
        }

        Node Function_body()
        {
            // completed
            // Function _body → { Statements Return_statement }
            Node function_body = new Node("Function_body");
            function_body.Children.Add(match(Token_Class.LPracket));
            function_body.Children.Add(Statements());
            function_body.Children.Add(Return_statement());
            function_body.Children.Add(match(Token_Class.RPracket));
            return function_body;
        }

        Node Return_statement()
        {
            Node return_statement = new Node("Return_statement");

            return return_statement;
        }

        Node Statements()
        {
            // completed
            // Statements → Statement Statements’
            Node statements = new Node("Statements");
            statements.Children.Add(Statement());
            statements.Children.Add(StatementsDash());
            return statements;
        }
        Node StatementsDash()
        {
            Node statementsDash = new Node("StatementsDash");

            return statementsDash;
        }
        Node Statement()
        {
            // Statement → Write_statement | Read_statement | Assignment_statement | Declaration_statement | If_statement |Repeat_statement | Function_call
            Node statement = new Node("Statement");
            
            return statement;
        }

        Node Write_statement()
        {
            // completed    
            // Write_statement → write Write_statement_Dash
            Node write_statement = new Node("Write_statement");
            write_statement.Children.Add(match(Token_Class.Write));
            write_statement.Children.Add(Write_statement_Dash());
            return write_statement;
        }

        Node Write_statement_Dash()
        {
            // completed
            // Write_statement’ → Expression ; | endl ;
            Node write_statement_dash = new Node("Write_statement_Dash");
            if (TokenStream[InputPointer].token_type == Token_Class.String)
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
            Node expression = new Node("Expression");
            if (TokenStream[InputPointer].token_type == Token_Class.String)
            {
                expression.Children.Add(match(Token_Class.String));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                expression.Children.Add(Term());
            }
            
            return expression;
        }

        Node Term()
        {
            // completed
            // Term → number | identifier | Function_call
            Node term = new Node("Term");
            if (TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                term.Children.Add(match(Token_Class.Number));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier && true/*check for i*/ && TokenStream[InputPointer+1].token_type == Token_Class.LParanthesis)
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
            // Identifier_list → Id Identifier_list’
            Node identifier_list = new Node("Identifier_list");
            identifier_list.Children.Add(match(Token_Class.Idenifier));
            identifier_list.Children.Add(Identifier_list_Dash());
            return identifier_list;
        }

        Node Identifier_list_Dash()
        {
            // Identifier_list’ → , Id Identifier_list’ | eplson
            Node identifier_list_Dash = new Node("Identifier_list_Dash");
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                identifier_list_Dash.Children.Add(match(Token_Class.Comma));
                identifier_list_Dash.Children.Add(match(Token_Class.Idenifier));
                identifier_list_Dash.Children.Add(Identifier_list_Dash());
                return identifier_list_Dash;
            }
            else
            {
                return null;
            }

        }
        // ================================================================================================================
        // match : deal with tokens 
        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
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
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString()  + "\r\n");
                InputPointer++;
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
