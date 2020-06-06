using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RubyComp;
using RubyComp.Scopes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RubyComp
{
    public class SemanticListener : RubBaseListener
    {
        public ParseTreeProperty<Scope> Scopes { get; set; } = new ParseTreeProperty<Scope>();
        public ParseTreeProperty<SymbolType> Types { get; } = new ParseTreeProperty<SymbolType>();
        private GlobalScope global;
        private GlobalScope globalVars;
        private Scope currScope;


        public override void EnterProg([NotNull] RubParser.ProgContext context)
        {
            global = new GlobalScope();
            globalVars = new GlobalScope();
            Scopes.Put(context, global);
            currScope = global;
            SymbolType.GlobalScope = global;
        }

        public override void EnterEveryRule(ParserRuleContext context)
        {
            context.GetType().ToString();
        }

        public override void ExitProg([NotNull] RubParser.ProgContext context)
        {
            Console.WriteLine($"{currScope.ScopeName} scope");
            foreach (KeyValuePair<string, ISymbol> item in currScope.Table)
            {
                if (item.Value.Type != null && item.Value.Type.Name == "Func")
                //if (true)
                {
                    Console.WriteLine(item.Key + ":" + item.Value.Type.Name);

                }
            }
        }

        public override void EnterAssignment([NotNull] RubParser.AssignmentContext context)
        {
            // Console.WriteLine("EnterAssignment");
        }

        public override void ExitInt_result([NotNull] RubParser.Int_resultContext context)
        {
            Types.Put(context, SymbolType.GetType("Integer"));
        }

        public override void ExitString_result([NotNull] RubParser.String_resultContext context)
        {
            Types.Put(context, SymbolType.GetType("String"));
        }

        public override void ExitFloat_result([NotNull] RubParser.Float_resultContext context)
        {
            Types.Put(context, SymbolType.GetType("Float"));
        }

        public override void EnterInt_assignment([NotNull] RubParser.Int_assignmentContext context)
        {
            var varId = context.var_id;
            string var = varId.GetText();

            switch (context.op.Type)
            {
                case RubParser.ASSIGN:

                    //var type = SymbolType.GetType(symbolType.Name)
                    if (!currScope.CheckSymbol(var))
                    {
                        VarSymbol var_ = new VarSymbol(var, SymbolType.GetType("Integer"));
                        currScope.AddSymbol(var_);
                        Types.Put(context, SymbolType.GetType("Integer"));
                    }
                    else
                    {
                        currScope.RemoveByName(var);
                        VarSymbol var_ = new VarSymbol(var, SymbolType.GetType("Integer"));
                        //Console.WriteLine($"Redefining {var}");
                        currScope.AddSymbol(var_);
                        Types.Put(context, SymbolType.GetType("Integer"));
                    }
                    break;
                default:
                    if (!currScope.CheckSymbol(var))
                    {
                        throw new SemanticException($"Undefined variable at {varId.start.Line}:{varId.start.Column}!");
                    }
                    break;
            }


            // Console.WriteLine("EnterInt_assignment");
            //conte
        }

        public override void ExitInt_assignment([NotNull] RubParser.Int_assignmentContext context)
        {
            // Console.WriteLine("ExitInt_assignment");
        }

        public override void EnterString_assignment([NotNull] RubParser.String_assignmentContext context)
        {
            var varId = context.var_id;
            string var = varId.GetText();

            switch (context.op.Type)
            {
                case RubParser.ASSIGN:

                    //var type = SymbolType.GetType(symbolType.Name)
                    if (!currScope.CheckSymbol(var))
                    {
                        VarSymbol var_ = new VarSymbol(var, SymbolType.GetType("String"));
                        currScope.AddSymbol(var_);
                        Types.Put(context, SymbolType.GetType("String"));
                    }
                    else
                    {
                        currScope.RemoveByName(var);
                        VarSymbol var_ = new VarSymbol(var, SymbolType.GetType("String"));
                        //Console.WriteLine($"Redefining {var}");
                        currScope.AddSymbol(var_);
                        Types.Put(context, SymbolType.GetType("String"));
                    }
                    break;
                default:
                    if (!currScope.CheckSymbol(var))
                    {
                        throw new SemanticException($"Undefined variable at {varId.start.Line}:{varId.start.Column}!");
                    }
                    break;
            }

        }


        public override void EnterDynamic_assignment([NotNull] RubParser.Dynamic_assignmentContext context)
        {
            var varId = context.var_id;
            string var = varId.GetText();

            switch (context.op.Type)
            {
                case RubParser.ASSIGN:

                    //var type = SymbolType.GetType(symbolType.Name)
                    if (!currScope.CheckSymbol(var))
                    {
                        VarSymbol var_ = new VarSymbol(var, SymbolType.GetType("Float"));
                        currScope.AddSymbol(var_);
                    }
                    break;
                default:
                    if (!currScope.CheckSymbol(var))
                    {
                        throw new SemanticException($"Undefined variable at {varId.start.Line}:{varId.start.Column}!");
                    }
                    break;
            }
        }

        public override void ExitDynamic_assignment([NotNull] RubParser.Dynamic_assignmentContext context)
        {
            //var varId = context.var_id;
            //string var = varId.GetText();

            //switch (context.op.Type)
            //{
            //    case RubParser.ASSIGN:

            //        //var type = SymbolType.GetType(symbolType.Name)
            //        if (!currScope.CheckSymbol(var))
            //        {
            //            VarSymbol var_ = new VarSymbol(var, SymbolType.GetType("Float"));
            //            currScope.AddSymbol(var_);
            //        }
            //        break;
            //    default:
            //        if (!currScope.CheckSymbol(var))
            //        {
            //            throw new SemanticException($"Undefined variable at {varId.start.Line}:{varId.start.Column}!");
            //        }
            //        break;
            //}
            var varId = context.var_id;
            string var = varId.GetText();

            switch (context.op.Type)
            {
                case RubParser.ASSIGN:

                    //var type = SymbolType.GetType(symbolType.Name)
                    if (!currScope.CheckSymbol(var))
                    {
                        VarSymbol var_ = new VarSymbol(var, Types.Get(context.GetChild(2)));
                        currScope.AddSymbol(var_);
                        Types.Put(context, Types.Get(context.GetChild(2)));
                    }
                    else
                    {
                        currScope.RemoveByName(var);
                        VarSymbol var_ = new VarSymbol(var, Types.Get(context.GetChild(2)));
                        currScope.AddSymbol(var_);
                        Types.Put(context, Types.Get(context.GetChild(2)));
                    }
                    break;
                default:
                    if (!currScope.CheckSymbol(var))
                    {
                        throw new SemanticException($"Undefined variable at {varId.start.Line}:{varId.start.Column}!");
                    }
                    break;
            }
        }

        

        public override void EnterFloat_assignment([NotNull] RubParser.Float_assignmentContext context)
        {
            var varId = context.var_id;
            string var = varId.GetText();

            switch (context.op.Type)
            {
                case RubParser.ASSIGN:

                    //var type = SymbolType.GetType(symbolType.Name)
                    if (!currScope.CheckSymbol(var))
                    {
                        VarSymbol var_ = new VarSymbol(var, SymbolType.GetType("Float"));
                        currScope.AddSymbol(var_);
                        Types.Put(context, SymbolType.GetType("Float"));
                    }
                    else
                    {
                        currScope.RemoveByName(var);
                        //Console.WriteLine($"Redefining {var}");
                        VarSymbol var_ = new VarSymbol(var, SymbolType.GetType("Float"));
                        currScope.AddSymbol(var_);
                        Types.Put(context, SymbolType.GetType("Float"));
                    }
                    break;
                default:
                    if (!currScope.CheckSymbol(var))
                    {
                        throw new SemanticException($"Undefined variable at {varId.start.Line}:{varId.start.Column}!");
                    }
                    else
                    {

                    }
                    break;
            }
        }

        public override void EnterInitial_array_assignment([NotNull] RubParser.Initial_array_assignmentContext context)
        {
            var varId = context.var_id;
            string var = varId.GetText();
            if (!currScope.CheckSymbol(var))
            {
                VarSymbol var_ = new VarSymbol(var, SymbolType.GetType("Array"));
                currScope.AddSymbol(var_);
            }
            else
            {
                currScope.RemoveByName(var);
                //Console.WriteLine($"Redefining {var} to new type");
                VarSymbol var_ = new VarSymbol(var, SymbolType.GetType("Array"));
                currScope.AddSymbol(var_);
            }

        }

        public override void ExitFloat_assignment([NotNull] RubParser.Float_assignmentContext context)
        {
            // Console.WriteLine("ExitFloat_assignment");

        }


        //for_statement : FOR id IN (int_result | dynamic_result) TD(int_result | dynamic_result) DO crlf statement_body END
        //      | FOR id IN dynamic_result DO crlf statement_body END
        //      | LEFT_RBRACKET dynamic_result RIGHT_RBRACKET DOT EACH DO BIT_OR id BIT_OR crlf statement_body END
        //      | dynamic_result DOT EACH DO BIT_OR id BIT_OR crlf statement_body END
        //      | LEFT_RBRACKET(int_result | dynamic_result) TD(int_result | dynamic_result) RIGHT_RBRACKET DOT EACH DO BIT_OR id BIT_OR crlf statement_body END
        //    ;

        public override void EnterFor_header([NotNull] RubParser.For_headerContext context)
        {
            
           var forstnmtindex = context.id();  

            var var_ = new VarSymbol(forstnmtindex.GetText(), SymbolType.GetType("Integer"));

            currScope.AddSymbol(var_);
        }

        public override void ExitFor_header([NotNull] RubParser.For_headerContext context)
        {
           
            if (context.GetChild(0).ToString() == "(")
            {
                if (context.GetChild(2).ToString() == "..")
                {
                    var lefttdtype = Types.Get(context.GetChild(1));
                    var righttype = Types.Get(context.GetChild(3));
                    if (lefttdtype.Name != "Integer" || righttype.Name != "Integer")
                    {
                        throw new SemanticException($"could not iterate over non-integer type {context.start.Line}:{context.start.Column}!");
                    }

                    Types.Put(context.id(), SymbolType.GetType("Integer"));
                }
                else
                {
                    var type = Types.Get(context.GetChild(1));
                    if (type.Name != "Array" && type.Name != "Unknown")
                    {
                        throw new SemanticException($"could not iterate over non-array type {context.start.Line}:{context.start.Column}!");
                    }

                    Types.Put(context.id(), SymbolType.GetType("Unknown"));
                }
            }
            else if (context.GetChild(0).ToString() == "for")
            {
                if (context.GetChild(4).ToString() == "..")
                {
                    var lefttdtype = Types.Get(context.GetChild(3));
                    var righttype = Types.Get(context.GetChild(5));

                    if (lefttdtype.Name != "Integer" || righttype.Name != "Integer")
                    {
                        throw new SemanticException($"could not iterate over non-integer type {context.start.Line}:{context.start.Column}!");
                    }
                    if (lefttdtype.Name != "Unknown" && righttype.Name != "Integer")
                    {
                        throw new SemanticException($"could not iterate over non-integer type {context.start.Line}:{context.start.Column}!");
                    }
                    if (lefttdtype.Name != "Integer" && righttype.Name != "Unknown")
                    {
                        throw new SemanticException($"could not iterate over non-integer type {context.start.Line}:{context.start.Column}!");
                    }

                    Types.Put(context.id(), SymbolType.GetType("Integer"));

                }
                else
                {
                    var type = Types.Get(context.GetChild(3));
                    if (type.Name != "Array" && type.Name != "Unknown")
                    {
                        throw new SemanticException($"could not iterate over non-array type {context.start.Line}:{context.start.Column}!");
                    }

                    Types.Put(context.id(), SymbolType.GetType("Unknown"));
                }

            }
            else
            {
                var type = Types.Get(context.GetChild(0));
                if (type.Name != "Array" && type.Name != "Unknown")
                {
                    throw new SemanticException($"could not iterate over non-array type {context.start.Line}:{context.start.Column}!");
                }

                Types.Put(context.id(), SymbolType.GetType("Unknown"));
            }

        }

        public override void ExitFor_statement([NotNull] RubParser.For_statementContext context)
        {
            
        }


        public override void ExitAll_result([NotNull] RubParser.All_resultContext context)
        {
            Types.Put(context, Types.Get(context.GetChild(0)));
        }

        public override void ExitComp_var([NotNull] RubParser.Comp_varContext context)
        {
            if (context.id() != null)
            {
                if (!(currScope.CheckSymbol(context.id().GetText())))
                {
                    throw new SemanticException($"Undefined variable at {context.start.Line}:{context.start.Column}!");
                }

                Types.Put(context, Types.Get(context.GetChild(0)));
            }
            else if (context.all_result() != null)
            {
                Types.Put(context, Types.Get(context.GetChild(0)));
            }
            else if (context.array_selector() != null)
            {
                Types.Put(context, SymbolType.GetType("Unknown"));
            }
        }


        public override void ExitDynamic_result([NotNull] RubParser.Dynamic_resultContext context)
        {
            if (context.ChildCount == 3)
            {
                var leftType = Types.Get(context.GetChild(0));
                var rightType = Types.Get(context.GetChild(2));


                if (context.GetChild(0).ToString() == "(")
                {
                    Types.Put(context, Types.Get(context.GetChild(1)));
                }
                else if (leftType.Name == "String" && rightType.Name == "String")
                {
                    Types.Put(context, SymbolType.GetType("String"));
                }
                else if (leftType.Name == "Integer" && rightType.Name == "Integer")
                {
                    Types.Put(context, SymbolType.GetType("Integer"));
                }
                else if (leftType.Name == "Float" && rightType.Name == "Float")
                {
                    Types.Put(context, SymbolType.GetType("Float"));
                }
                else if (leftType.Name == "String" || rightType.Name == "String")
                {
                    throw new SemanticException($"String can't be coerced {context.start.Line}:{context.start.Column}!");
                }
                else if (leftType.Name == "Unknown")
                {
                    Types.Put(context, SymbolType.GetType("Unknown"));
                }
                else if (rightType.Name == "Unknown")
                {
                    Types.Put(context, SymbolType.GetType("Unknown"));
                }
                else if (leftType.Name == "Integer" && rightType.Name == "Float")
                {
                    Types.Put(context, SymbolType.GetType("Float"));
                }
                else if (leftType.Name == "Float" && rightType.Name == "Integer")
                {
                    Types.Put(context, SymbolType.GetType("Float"));
                }
                else if (leftType.Name == "Unknown" || rightType.Name == "Unknown")
                {
                    Types.Put(context, SymbolType.GetType("Unknown"));
                }
                else
                {
                    Types.Put(context, SymbolType.GetType("Unknown"));
                }
            }
            else if (context.ChildCount == 1)
            {
                Types.Put(context, Types.Get(context.GetChild(0)));
            }
        }

        public override void ExitDynamic([NotNull] RubParser.DynamicContext context)
        {
            if (context.id() != null)
            {
                if (!(currScope.CheckSymbol(context.id().GetText())))
                {
                    throw new SemanticException($"Undefined variable at {context.start.Line}:{context.start.Column}!");
                }
                Types.Put(context, Types.Get(context.GetChild(0)));
            }
            else if (context.function_call_assignment() != null)
            {
                Types.Put(context, SymbolType.GetType("Unknown"));
            }
            else if (context.array_selector() != null)
            {
                Types.Put(context, SymbolType.GetType("Unknown"));
            }
        }

        public override void EnterId([NotNull] RubParser.IdContext context)
        {
            var var = context.GetText();

            if (!currScope.CheckSymbol(var) && var != currScope.ScopeName && var != "len" && var != "puts")
            {
                throw new SemanticException($"Undefined variable at {context.start.Line}:{context.start.Column}!");
            }
        }

        public override void ExitId([NotNull] RubParser.IdContext context)
        {
            var var = context.GetText();
            if (var != currScope.ScopeName && var != "puts" && var != "len")
            {
                var varSymbol =  currScope.GetSymbol(var);
                Types.Put(context, varSymbol.Type);
            }
            else
            {
                Types.Put(context, SymbolType.GetType("Func"));
            }


        }

        public override void ExitString_assignment([NotNull] RubParser.String_assignmentContext context)
        {
            // Console.WriteLine("ExitString_assignment");
        }

        public override void EnterRvalue([NotNull] RubParser.RvalueContext context)
        {
            // Console.WriteLine("EnterRvalue");
        }

        public override void ExitRvalue([NotNull] RubParser.RvalueContext context)
        {
            // Console.WriteLine("ExitRvalue");
        }

        public override void EnterLvalue([NotNull] RubParser.LvalueContext context)
        {
            // Console.WriteLine("EnterLvalue");
        }

        public override void ExitLvalue([NotNull] RubParser.LvalueContext context)
        {
            // Console.WriteLine("ExitLvalue");
        }


        public override void ExitReturn_statement([NotNull] RubParser.Return_statementContext context)
        {
            var returnType = Types.Get(context.GetChild(1));
            Console.WriteLine($"Returns {returnType.Name}");
        }

        public override void EnterFunction_definition_header([NotNull] RubParser.Function_definition_headerContext context)
        {
            var functionId = context.function_name();
            string name = functionId.GetText();

            var paramsId = context.function_definition_params();
            string paramsName;

            if (paramsId == null)
            {
                paramsName = "";
            }
            else
                paramsName = paramsId.GetText();


            if ((currScope.CheckSymbol(name)))
            {
                //Console.WriteLine($"Redefining {name} to new function");
                currScope.RemoveByName(name);
            }


            FunctionSymbol function_ = new FunctionSymbol(name, SymbolType.GetType("Func"), paramsName, currScope);
            Types.Put(context.GetChild(1), SymbolType.GetType("Func"));
            currScope.AddSymbol(function_);
            Scopes.Put(context, function_);
            currScope = function_;
            currScope.ScopeName = functionId.GetText();
            currScope.AddSymbol(function_);
            Console.WriteLine($"\nFunction {currScope.ScopeName} {paramsName}");


        }

        public override void EnterFunction_definition_param_id([NotNull] RubParser.Function_definition_param_idContext context)
        {
            var paramId = context.id();
            string name = paramId.GetText();

            VarSymbol var_ = new VarSymbol(name, SymbolType.GetType("Unknown"));

            currScope.AddSymbol(var_);
            Types.Put(context, SymbolType.GetType("Unknown"));

        }


        public override void ExitArray_selector([NotNull] RubParser.Array_selectorContext context)
        { 
            if (context.ChildCount == 4)
            {
                var index =  context.GetChild(2);
                var indexType = Types.Get(index).Name;
                if (indexType != "Integer")
                {
                    throw new SemanticException($"Integer expected {context.start.Line}:{context.start.Column}!");
                }
            }
            else if (context.ChildCount == 4)
            {
                var index = context.GetChild(2);
                var indexType = Types.Get(index).Name;
                if (indexType != "Integer")
                {
                    throw new SemanticException($"Integer expected {context.start.Line}:{context.start.Column}!");
                }
            }
        }

        public override void EnterFunction_call([NotNull] RubParser.Function_callContext context)
        {
            var funcId = context.name;
            string name = funcId.GetText();
            string parametrs;
            if (context.@params != null)
            {
                parametrs = context.@params.GetText();
            }
            else
            {
                parametrs = "";
            }

            var paramTokens = parametrs.Replace("(", "").Replace(")", "").Split(",");


            if (name != "len" && name != "puts")
            {
                if (!(currScope.CheckSymbol(name)))
                {
                    throw new SemanticException($"Undefined variable or function at {funcId.start.Line}:{funcId.start.Column}!");
                }
                else
                {
                    var symbol = currScope.GetSymbol(name);

                    if (symbol.Type.Name != "Func")
                    {
                        throw new SemanticException($"It is not a function at {funcId.start.Line}:{funcId.start.Column}!");
                    }

                    var decklParamTokens = symbol.Parameters.Replace("(", "").Replace(")", "").Split(",");

                    if (paramTokens.Length != decklParamTokens.Length)
                    {
                        throw new SemanticException($"Wrong count of arguments : {funcId.start.Line}:{funcId.start.Column}!");
                    }

                }
            }


        }




        public override void ExitFunction_definition([NotNull] RubParser.Function_definitionContext context)
        {
            //Console.WriteLine($"Function {currScope.ScopeName} ");
            foreach (KeyValuePair<string, ISymbol> item in currScope.Table)
            {
                if (item.Value.Type != null)
                {
                    Console.WriteLine(item.Key + ":" + item.Value.Type.Name);
                }
                else
                    Console.WriteLine(item.Key + ":");
            }

            currScope = currScope.Parent;
        }


        public override void ExitComparison([NotNull] RubParser.ComparisonContext context)
        {
            var left = context.left;
            var right = context.right;
            
            if (context.ChildCount == 3)
            {
                var leftType = Types.Get(context.GetChild(0));
                var rightType = Types.Get(context.GetChild(2));


                if (leftType.Name == "String" && rightType.Name != "String" && ((context.op.Text != "==") && context.op.Text != "!="))
                {
                    throw new SemanticException($"Could not compare String: {left.start.Line}:{left.start.Column}!");
                }
                else if (leftType.Name != "String" && rightType.Name == "String" && ((context.op.Text != "==") && context.op.Text != "!="))
                {
                    throw new SemanticException($"Could not compare String: {right.start.Line}:{right.start.Column}!");
                }

                Types.Put(context, SymbolType.GetType("Bool"));
            }

        }

        public override void EnterArray_definition_elements([NotNull] RubParser.Array_definition_elementsContext context)
        {
            
        }

    }
}