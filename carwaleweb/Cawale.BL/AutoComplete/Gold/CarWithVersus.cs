
using System;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.goldparser.lalr;
using com.calitha.commons;
using com.calitha.goldparser;

namespace com.calitha.carversussuggest
{

    [Serializable()]
    public class SymbolException : System.Exception
    {
        public SymbolException(string message) : base(message)
        {
        }

        public SymbolException(string message,
            Exception inner) : base(message, inner)
        {
        }

        protected SymbolException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

    }

    [Serializable()]
    public class RuleException : System.Exception
    {

        public RuleException(string message) : base(message)
        {
        }

        public RuleException(string message,
                             Exception inner) : base(message, inner)
        {
        }

        protected RuleException(SerializationInfo info,
                                StreamingContext context) : base(info, context)
        {
        }

    }

    enum SymbolConstants : int
    {
        SYMBOL_EOF          = 0, // (EOF)
        SYMBOL_ERROR        = 1, // (Error)
        SYMBOL_WHITESPACE   = 2, // Whitespace
        SYMBOL_IDENTIFIER   = 3, // Identifier
        SYMBOL_VERSUS       = 4, // Versus
        SYMBOL_FIRSTCAR     = 5, // <firstcar>
        SYMBOL_SECONDCAR    = 6, // <secondcar>
        SYMBOL_START        = 7, // <start>
        SYMBOL_VERSUSSECOND = 8  // <versusSecond>
    };

    enum RuleConstants : int
    {
        RULE_START                 = 0, // <start> ::= <secondcar>
        RULE_SECONDCAR_IDENTIFIER  = 1, // <secondcar> ::= <versusSecond> Identifier
        RULE_SECONDCAR_IDENTIFIER2 = 2, // <secondcar> ::= <secondcar> Identifier
        RULE_VERSUSSECOND_VERSUS   = 3, // <versusSecond> ::= <firstcar> Versus
        RULE_FIRSTCAR_IDENTIFIER   = 4, // <firstcar> ::= Identifier
        RULE_FIRSTCAR_IDENTIFIER2  = 5  // <firstcar> ::= <firstcar> Identifier
    };

    public class CarVersusSuggestParser
    {
        private LALRParser parser;

        public CarVersusSuggestParser(string filename)
        {
            FileStream stream = new FileStream(filename,
                                               FileMode.Open, 
                                               FileAccess.Read, 
                                               FileShare.Read);
            Init(stream);
            stream.Close();
        }

        public CarVersusSuggestParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public CarVersusSuggestParser(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            CGTReader reader = new CGTReader(stream);
            parser = reader.CreateNewParser();
            parser.TrimReductions = false;
            parser.StoreTokens = LALRParser.StoreTokensMode.NoUserObject;

            parser.OnTokenError += new LALRParser.TokenErrorHandler(TokenErrorEvent);
            parser.OnParseError += new LALRParser.ParseErrorHandler(ParseErrorEvent);
        }

        public Tuple<string,string> Parse(string source)
        {
            string[] arr = source.Trim().Split(null);
            string car = "", car1 = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == "vs")
                {
                    if (car == "" || car1 != "")
                    {
                        return null;
                    }
                    car1 = car;
                    car = "";
                    continue;
                }
                car += arr[i] + " ";
            }
            if (car == "" || car1 == "")
            {
                return null;
            }

            return new Tuple<string, string>(car1.Trim(), car.Trim());
        }

        private Tuple<string, string> CreateObject(Token token)
        {
            if (token is TerminalToken)
                return new Tuple<string,string>(CreateObjectFromTerminal((TerminalToken)token),null);
            else
                return CreateObjectFromNonterminal((NonterminalToken)token);
        }

        private string CreateObjectFromTerminal(TerminalToken token)
        {
            switch (token.Symbol.Id)
            {
                case (int)SymbolConstants.SYMBOL_EOF :
                //(EOF)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ERROR :
                //(Error)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //Whitespace
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER :
                //Identifier
                return token.Text;

                case (int)SymbolConstants.SYMBOL_VERSUS :
                //Versus
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FIRSTCAR :
                //<firstcar>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SECONDCAR :
                //<secondcar>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_START :
                //<start>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VERSUSSECOND :
                //<versusSecond>
                //todo: Create a new object that corresponds to the symbol
                return null;

            }
            throw new SymbolException("Unknown symbol");
        }

        public Tuple<string, string> CreateObjectFromNonterminal(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_START :
                //<start> ::= <secondcar>
                return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_SECONDCAR_IDENTIFIER :
                //<secondcar> ::= <versusSecond> Identifier
                return new Tuple<string, string>(CreateObject(token.Tokens[0]).Item1, CreateObjectFromTerminal((TerminalToken)token.Tokens[1]));

                case (int)RuleConstants.RULE_SECONDCAR_IDENTIFIER2 :
                //<secondcar> ::= <secondcar> Identifier
                    Tuple<string, string> t1 = CreateObject(token.Tokens[0]);
                    return new Tuple<string, string>(t1.Item1, t1.Item2 + " " + CreateObjectFromTerminal((TerminalToken)token.Tokens[1]));

                case (int)RuleConstants.RULE_VERSUSSECOND_VERSUS :
                //<versusSecond> ::= <firstcar> Versus
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_FIRSTCAR_IDENTIFIER :
                //<firstcar> ::= Identifier
                    return new Tuple<string,string>(CreateObjectFromTerminal((TerminalToken)token.Tokens[0]),null);

                case (int)RuleConstants.RULE_FIRSTCAR_IDENTIFIER2 :
                //<firstcar> ::= <firstcar> Identifier
                    Tuple<string, string> t2 = CreateObject(token.Tokens[0]);
                    return new Tuple<string, string>(t2.Item1 + " " + CreateObjectFromTerminal((TerminalToken)token.Tokens[1]), null);

            }
            throw new RuleException("Unknown rule");
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            string message = "Token error with input: '"+args.Token.ToString()+"'";
            //todo: Report message to UI?
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {
            string message = "Parse error caused by token: '"+args.UnexpectedToken.ToString()+"'";
            //todo: Report message to UI?
        }

    }
}
