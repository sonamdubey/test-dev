
using System;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.goldparser.lalr;
using com.calitha.commons;
using com.calitha.goldparser;

namespace com.calitha.carsuggest
{

    [Serializable()]
    public class SymbolException : System.Exception
    {
        public SymbolException(string message)
            : base(message)
        {
        }

        public SymbolException(string message,
            Exception inner)
            : base(message, inner)
        {
        }

        protected SymbolException(SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

    }

    [Serializable()]
    public class RuleException : System.Exception
    {

        public RuleException(string message)
            : base(message)
        {
        }

        public RuleException(string message,
                             Exception inner)
            : base(message, inner)
        {
        }

        protected RuleException(SerializationInfo info,
                                StreamingContext context)
            : base(info, context)
        {
        }

    }

    enum SymbolConstants : int
    {
        SYMBOL_EOF = 0, // (EOF)
        SYMBOL_ERROR = 1, // (Error)
        SYMBOL_WHITESPACE = 2, // Whitespace
        SYMBOL_IDENTIFIER = 3, // Identifier
        SYMBOL_VERSUS = 4, // Versus
        SYMBOL_CAR = 5, // <car>
        SYMBOL_START = 6, // <start>
        SYMBOL_SUGGEST = 7, // <suggest>
        SYMBOL_SUGGESTNULL = 8, // <suggestnull>
        SYMBOL_SUGGESTWITHVERSUS = 9  // <suggestwithversus>
    };

    enum RuleConstants : int
    {
        RULE_START = 0, // <start> ::= <suggestnull>
        RULE_SUGGESTNULL = 1, // <suggestnull> ::= <suggest>
        RULE_SUGGESTNULL2 = 2, // <suggestnull> ::= 
        RULE_SUGGEST = 3, // <suggest> ::= <car>
        RULE_SUGGEST2 = 4, // <suggest> ::= <suggestwithversus> <car>
        RULE_SUGGESTWITHVERSUS_VERSUS = 5, // <suggestwithversus> ::= <car> Versus
        RULE_CAR_IDENTIFIER = 6, // <car> ::= Identifier
        RULE_CAR_IDENTIFIER2 = 7  // <car> ::= <car> Identifier
    };

    public class CarSuggestParser
    {
        private LALRParser parser;

        public CarSuggestParser(string filename)
        {
            FileStream stream = new FileStream(filename,
                                               FileMode.Open,
                                               FileAccess.Read,
                                               FileShare.Read);
            Init(stream);
            stream.Close();
        }

        public CarSuggestParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public CarSuggestParser(Stream stream)
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

        public string Parse(string source)
        {
            string[] arr = source.Trim().Split(null);
            string result = "";
            bool flag = false;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == "vs")
                {
                    if (flag || i == 0)
                    {
                        return null;
                    }
                    flag = true;
                    result = "";
                    continue;
                }
                result += arr[i] + " ";
            }

            return result.Trim();
        }

        private string CreateObject(Token token)
        {
            if (token is TerminalToken)
                return CreateObjectFromTerminal((TerminalToken)token);
            else
                return CreateObjectFromNonterminal((NonterminalToken)token);
        }

        private string CreateObjectFromTerminal(TerminalToken token)
        {
            switch (token.Symbol.Id)
            {
                case (int)SymbolConstants.SYMBOL_EOF:
                    //(EOF)
                    return null;

                case (int)SymbolConstants.SYMBOL_ERROR:
                    //(Error)
                    return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE:
                    //Whitespace
                    return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER:
                    //Identifier
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_VERSUS:
                    //Versus
                    return null;

                case (int)SymbolConstants.SYMBOL_CAR:
                    //<car>
                    return null;

                case (int)SymbolConstants.SYMBOL_START:
                    //<start>
                    return null;

                case (int)SymbolConstants.SYMBOL_SUGGEST:
                    //<suggest>
                    return null;

                case (int)SymbolConstants.SYMBOL_SUGGESTNULL:
                    //<suggestnull>
                    return null;

                case (int)SymbolConstants.SYMBOL_SUGGESTWITHVERSUS:
                    //<suggestwithversus>
                    return null;

            }
            throw new SymbolException("Unknown symbol");
        }

        public string CreateObjectFromNonterminal(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_START:
                    //<start> ::= <suggestnull>
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_SUGGESTNULL:
                    //<suggestnull> ::= <suggest>
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_SUGGESTNULL2:
                    //<suggestnull> ::= 
                    return null;

                case (int)RuleConstants.RULE_SUGGEST:
                    //<suggest> ::= <car>
                   return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_SUGGEST2:
                    //<suggest> ::= <suggestwithversus> <car>
                    return CreateObject(token.Tokens[1]);

                case (int)RuleConstants.RULE_SUGGESTWITHVERSUS_VERSUS:
                    //<suggestwithversus> ::= <car> Versus
                    return null;

                case (int)RuleConstants.RULE_CAR_IDENTIFIER:
                    //<car> ::= Identifier
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_CAR_IDENTIFIER2:
                    //<car> ::= <car> Identifier
                    return CreateObject(token.Tokens[0]) + " " + CreateObject(token.Tokens[1]);


            }
            throw new RuleException("Unknown rule");
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            string message = "Token error with input: '" + args.Token.ToString() + "'";
            //todo: Report message to UI?
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {
            string message = "Parse error caused by token: '" + args.UnexpectedToken.ToString() + "'";
            //todo: Report message to UI?
        }

    }
}
