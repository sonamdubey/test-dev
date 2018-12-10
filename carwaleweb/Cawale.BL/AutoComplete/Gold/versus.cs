
using System;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.goldparser.lalr;
using com.calitha.commons;
using com.calitha.goldparser;

namespace com.calitha.versussuggest
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
        SYMBOL_CARPART = 5, // <carpart>
        SYMBOL_START = 6, // <start>
        SYMBOL_VERSUSSUGGEST = 7  // <versussuggest>
    };

    enum RuleConstants : int
    {
        RULE_START = 0, // <start> ::= <versussuggest>
        RULE_VERSUSSUGGEST_VERSUS = 1, // <versussuggest> ::= <carpart> Versus
        RULE_CARPART_IDENTIFIER = 2, // <carpart> ::= Identifier
        RULE_CARPART_IDENTIFIER2 = 3  // <carpart> ::= <carpart> Identifier
    };

    public class VersusSuggestParser
    {
        private LALRParser parser;

        public VersusSuggestParser(string filename)
        {
            FileStream stream = new FileStream(filename,
                                               FileMode.Open,
                                               FileAccess.Read,
                                               FileShare.Read);
            Init(stream);
            stream.Close();
        }

        public VersusSuggestParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public VersusSuggestParser(Stream stream)
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

            if (arr[arr.Length - 1] == "vs")
            {
                for(int i=0;i<arr.Length-1;i++)
                {
                    result += arr[i] + " ";
                    if (arr[i] == "vs")
                    {
                        return null;
                    }
                }

            }
            else 
            {
                return null;
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
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_ERROR:
                    //(Error)
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE:
                    //Whitespace
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER:
                    //Identifier
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_VERSUS:
                    //Versus
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_CARPART:
                    //<carpart>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_START:
                    //<start>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_VERSUSSUGGEST:
                    //<versussuggest>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

            }
            throw new SymbolException("Unknown symbol");
        }

        public string CreateObjectFromNonterminal(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_START:
                    //<start> ::= <versussuggest>
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_VERSUSSUGGEST_VERSUS:
                    //<versussuggest> ::= <carpart> Versus
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_CARPART_IDENTIFIER:
                    //<carpart> ::= Identifier
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_CARPART_IDENTIFIER2:
                    //<carpart> ::= <carpart> Identifier
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
