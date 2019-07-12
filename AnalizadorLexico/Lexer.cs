using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico
{
    class Lexer
    {
        private char peek;
        private string programafonte;
        private int i;

        private int id;
        string[] reservadas = { "program", "int", "string", "double", "float", "if",
                                "else", "while", "for", "end" };
        string[] operadores = { "=", "==", "<", ">", "<=", ">=", "!", "+", "-", "/",
                                "*", "%", "++", "--"};
        //private bool valorEsperado = false;
        private Simbolo simbol = new Simbolo();
        public List<Simbolo> listaSimbolos = new List<Simbolo>();

        public Lexer (string programa)
        {
            programafonte = programa;
            init();
        }

        public int getI
        {
            get { return i; }
        }

        private void init()
        {
            i = 0;
            id = 1;
            nextChar();
        }

        private void nextChar()
        {
            if (i < programafonte.Length)
            {
                peek = programafonte[i];
                i++;
            }
            else
            {
                peek = programafonte[programafonte.Length - 1];     
            }
        }


        public String scan()
        {
            string token = "";
            while(peek == ' ' || peek == '\t' || peek == '\n')
                nextChar();


            //Número
            if (Char.IsDigit(peek))
            {
                int parteInteira = 0;
                int divisor = 1;
                float pontoFlutuante = 0;
                

                do
                {
                    parteInteira = (10 * parteInteira) + int.Parse(peek.ToString());
                    nextChar();
                } while (Char.IsDigit(peek));

                if (peek.Equals(','))
                {
                    nextChar();
                    do
                    {                     
                        pontoFlutuante = (10 * pontoFlutuante) + int.Parse(peek.ToString());
                        nextChar();
                        //divisor *= 10;
                    } while (Char.IsDigit(peek));

                    //pontoFlutuante /= divisor;
                    
                    token = "<NUM, " + 
                        parteInteira.ToString() + ',' + pontoFlutuante.ToString() + ">";
                    return token;
                }
                //Por enquanto só se aceita números inteiros
                token = "<NUM, " + parteInteira.ToString() + ">";
                return token;
            }


            //Literal
            if (peek.Equals('"'))
            {
                string lexema = "";
                nextChar();

                while (!peek.Equals('"')) 
                {             
                    lexema = String.Concat(lexema, peek.ToString());
                    nextChar();
                }
                nextChar();

                token = "<LITERAL, " + lexema + ">";
                return token;
            }


            //Identificador ou palavra reservada
            if (Char.IsLetter(peek))
            {
                string lexema = "";
                do
                {
                    lexema = String.Concat(lexema, peek.ToString());
                    /*
                    if (lexema.Equals("end"))
                    {
                        token = "<" + lexema + ">";
                        return token;
                    }
                    */
                    
                    //lexema += peek.ToString();
                    nextChar();
                } while (Char.IsLetterOrDigit(peek));

                if (reservadas.Contains(lexema))
                {
                    token = "<" + lexema + ">";
                    return token;
                }
                else
                {
                    Simbolo S = listaSimbolos.Find(s => s.Nome == lexema);

                    if (S != null)
                        simbol = S;
                    else
                    {
                        simbol = new Simbolo();
                        simbol.Id = id;
                        simbol.Nome = lexema;
                        listaSimbolos.Add(simbol);
                        id++;
                    }

                    token = "<ID, " + simbol.Id + ">";
                    return token;
                }
            }


            //Operadores
            if (("=+-<>/%&|!*&~").Contains(peek))
            {
                string lexema = "";
                while (("=+-<>/%&|!*&~").Contains(peek))
                {
                    lexema = String.Concat(lexema, peek.ToString());
                    nextChar();
                }

                if (operadores.Contains(lexema))
                {
                    token = "<RELOP, " + lexema + ">";
                    return token;
                }
            }


            //Delimitadores
            if (("{}();").Contains(peek.ToString()))
            {
                token = "<DELIM, " + peek + ">";
                nextChar();
                return token;
            }

            token = peek.ToString();
            return token;
            
        }
    }
}
