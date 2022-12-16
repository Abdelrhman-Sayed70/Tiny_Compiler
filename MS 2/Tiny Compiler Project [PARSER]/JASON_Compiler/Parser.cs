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
            // Main_function → Datatype main() Function_body
            Node main_function = new Node("Main_function");

            // implement the function
            
            return main_function;
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
