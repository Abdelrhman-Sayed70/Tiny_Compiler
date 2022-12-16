using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
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
        int InputPointer = 0; // keep trake of token stream 
        List<Token> TokenStream;
        public  Node root; // parse tree
        
        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream; // implemented in phase 1 
            root = new Node("Program");
            root.Children.Add(Program()); // starting production rule
            return root;
        }
        // for each procedure add its mo3adla 
        // terminal [tokens] -> Add(mathc(token)) 
        // non terminal [procedure] -> Add(function)
        // or -> check of the prefix of each statement using if statement using TokenStream[InputPointer]
        // in case of epslon return null 
        Node Program()
        {
            Node program = new Node("Program"); // name shown in parse tree 
            // right hand side  
            program.Children.Add(Header());
            program.Children.Add(DeclSec());
            program.Children.Add(Block());
            program.Children.Add(match(Token_Class.Dot)); 
            // end right hans side  
            MessageBox.Show("Success");
            return program;
        }
        
        // Implement your logic here [my hand start] 
        Node Header()
        {
            Node header = new Node("Header");
            // write your code here to check the header sructure
            header.Children.Add(match(Token_Class.Program)); 
            header.Children.Add(match(Token_Class.Idenifier));
            
            MessageBox.Show("Success");
            return header;
        }
        // not implemneted 
        Node DeclSec()
        {
            Node declsec = new Node("DeclSec");
            // write your code here to check atleast the declare sturcure 
            // without adding procedures
            return declsec;
        }
        Node Block()
        {
            Node block = new Node("block");
            // write your code here to match statements
            // right hand side 
            block.Children.Add(match(Token_Class.Begin));
            return block;
        }
        
        Node Statements()
        {
            Node statements = new Node("Statements");
            statements.Children.Add(Statement());
            statements.Children.Add(State());
            return statements; 
        }
        Node Statement()
        {
            Node statement = new Node("Statement");

            if (TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                statement.Children.Add(match(Token_Class.Read));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                statement.Children.Add(match(Token_Class.Write));

            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Set)
            {
                statement.Children.Add(match(Token_Class.Set));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Call)
            {
                statement.Children.Add(match(Token_Class.Call));
            }
            return statement;
        }

        Node State()
        {
            Node state = new Node("state");
            if (TokenStream[InputPointer].token_type == Token_Class.Semicolon)
            {
                state.Children.Add(match(Token_Class.Semicolon));
                state.Children.Add(Statement());
                state.Children.Add(State());
                return state;
            }
            return null; // epsilon 
        }

        Node Expression()
        {
            // completed
            Node expression = new Node("expression");
            expression.Children.Add(Term());
            expression.Children.Add(Exp());
            return expression;
        }

        
        Node Term()
        {
            // completed 
            Node term = new Node("term");
            term.Children.Add(Factor());
            term.Children.Add(Ter());
            return term;
        }

        Node Ter()
        {
            // completed 
            Node ter = new Node("ter");
            if (TokenStream[InputPointer].token_type == Token_Class.DivideOp || TokenStream[InputPointer].token_type == Token_Class.DivideOp)
            {
                ter.Children.Add(MultOp());
                ter.Children.Add(Factor());
                ter.Children.Add(Ter());
                return ter;
            }
            return null;

        }
        Node MultOp()
        {
            // completed
            Node multop = new Node("multop");
            if (TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
            {
                multop.Children.Add(match(Token_Class.MultiplyOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.DivideOp)
            {
                multop.Children.Add(match(Token_Class.DivideOp));
            }
            return multop;
        }
        Node AddOp()
        {
            // completed
            Node addop = new Node("addop");
            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp)
            {
                addop.Children.Add(match(Token_Class.PlusOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.MinusOp)
            {
               addop.Children.Add(match(Token_Class.MinusOp));
            }
            return addop;
        }

        Node Exp()
        {
            // completed
            Node exp = new Node("exp");
            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp)
            {
                exp.Children.Add(AddOp());
                exp.Children.Add(Term());
                exp.Children.Add(Exp());
                return exp;
            }
            return null;
        }



        Node Factor()
        {
            // completed
            Node factor = new Node("factor");
            if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                factor.Children.Add(match(Token_Class.Idenifier));

            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Constant)
            {
                factor.Children.Add(match(Token_Class.Constant));
            }
            return factor;
        }

        Node RelOp()
        {
            // completed
            Node relop = new Node("relop");
            if (TokenStream[InputPointer].token_type == Token_Class.EqualOp)
            {
                relop.Children.Add(match(Token_Class.EqualOp));
            }

            else if (TokenStream[InputPointer].token_type == Token_Class.NotEqualOp)
            {
                relop.Children.Add(match(Token_Class.NotEqualOp));
            }

            else if (TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp)
            {
                relop.Children.Add(match(Token_Class.GreaterThanOp));
            }

            else if (TokenStream[InputPointer].token_type == Token_Class.LessThanOp)
            {
                relop.Children.Add(match(Token_Class.LessThanOp));
            }
            return relop;
        }

        Node ArgList()
        {
            // completed
            Node arglist = new Node("arglist");
            if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                arglist.Children.Add(match(Token_Class.LParanthesis));
                arglist.Children.Add(Arguments());
                arglist.Children.Add(match(Token_Class.RParanthesis));
                return arglist; 
            }
            return null;
        }

        Node Arguments()
        {
            // completed
            Node argument = new Node("argument");
            argument.Children.Add(match(Token_Class.Idenifier));
            argument.Children.Add(Arg());
            return argument;
        }

        Node Arg()
        {
            // completed
            Node arg = new Node("arg");
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                arg.Children.Add(match(Token_Class.Comma));
                arg.Children.Add(match(Token_Class.Idenifier));
                arg.Children.Add(Arg());
                return arg;
            }

            return null;
        }


        // end of my hand ==========================================================================
        public Node match(Token_Class ExpectedToken)
        {
            // check of token stream
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
                // x = 
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString()  + "\r\n");
                InputPointer++;
                return null;
            }
        }

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
