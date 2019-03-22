using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KataCSharp.Calculator
{
    public class Operation
    {
        private InvokeOperation _Method;
        public string StringRepresentation { get; }
        public int Priority { get; }

        public Operation(InvokeOperation invokeMethod, int priority, string sign)
        {
            this.StringRepresentation = sign;
            this.Priority = priority;
            this._Method = invokeMethod;
        }

        public delegate double InvokeOperation(Stack<double> stack);

        public double Invoke(Stack<double> stack)
        {
            return this._Method.Invoke(stack);
        }
    }

    public class StackCalculator
    {
        private IDictionary<string, Operation> _operationsList;
        private Stack<double> _val;
        private Stack<string> _op;
        public char FloatPointSign { get; set; }

        public static string ReplaceUnarMinus(string input, string replacer)
        {
            string output = "";
            int i_len = 0;
            for (int i = 0; i < input.Length;)
            {
                i_len = 0;
                while (i + i_len < input.Length && input[i + i_len] != '-')
                    i_len++;
                output += input.Substring(i, i_len);
                i += i_len;
                if (i >= input.Length)
                    break;
                if (i != 0 && (Char.IsDigit(input[i - 1]) || input[i - 1] == ')'))
                {
                    output += input[i];
                }
                else
                {
                    output += replacer;
                }
                i++;
            }
            return output;
        }
        public double Calculate(string expression)
        {
            string input = ReplaceUnarMinus(expression, "~");
            int i = 0;
            int i_len = 0;
            while (i < input.Length)
            {
                i_len = 0;
                if (Char.IsDigit(input[i]))
                {
                    while (i + i_len < input.Length && (Char.IsDigit(input[i + i_len]) || input[i + i_len] == this.FloatPointSign))
                        i_len++;
                    if (double.TryParse(input.Substring(i, i_len), out double number))
                    {
                        this._val.Push(number);
                        i += i_len;
                        continue;
                    }
                    else
                    {
                        throw new CalculationException($"Cant parse value {input.Substring(i, i_len)} as double");
                    }

                }
                //if brackets
                if (input[i] == ')')
                {
                    CalculateStack();
                    i++;
                    continue;
                }

                if (input[i] == '(')
                {
                    _op.Push("(");
                    i++;
                    continue;
                }

                i_len = 1;

                while (i + i_len < input.Length && 0 < _operationsList.Keys.Count(x => x.StartsWith(input.Substring(i, i_len))))
                {
                    if (_operationsList.Keys.Count(x => x == input.Substring(i, i_len)) == 1)
                    {
                        break;
                    }
                    i_len++;
                }
                var opStr = input.Substring(i, i_len);
                var matched = _operationsList.Where(x => x.Key == input.Substring(i, i_len));
                if (matched.Count() == 1)
                {
                    string topStack;
                    if (_op.Count != 0 && (topStack = _op.Peek()) != "(" && _operationsList[topStack].Priority >= _operationsList[opStr].Priority)
                    {
                        CalculateStack();
                    }
                    _op.Push(opStr);
                    i += i_len;
                    continue;
                }
                throw new CalculationException($"Cannot define operation starting from '{input.Substring(i)}");
            }
            if (_op.Count > 0) CalculateStack();
            if (_val.Count == 1) return _val.Pop();
            throw new CalculationException("fasdfasd");
        }

        private void CalculateStack()
        {
            while (_op.Count > 0 && _op.Peek() != "(")
            {
                //there is operation for sure
                //try to parse operatin and get Operation instance
                var operation = _operationsList[_op.Pop()];
                //if we here we have instance of Operation
                //calculate and push result back to the stack
                _val.Push(operation.Invoke(_val));
            }
            //if we are here then one of while statements is true
            //if it no operation left then everthing is fine
            if (_op.Count == 0) return;

            //if next operation is '(' then pop it out and exit
            if (_op.Peek() == "(")
            {
                _op.Pop(); return;
            }
            //if we are here then something went wrong
            throw new CalculationException("Corrupted stack exception occured");
        }

        public StackCalculator()
        {
            this.FloatPointSign = ',';
            this._val = new Stack<double>();
            this._op = new Stack<string>();
            Populate();
        }

        //public StackCalculator AddOperation(Func<Operation> func)
        //{
        //    Operation opToAdd = func(this._val);
        //    this._operationsList.Add(opToAdd.StringRepresentation,opToAdd);
        //    return this;
        //}

        private void Populate()
        {
            _operationsList = new Dictionary<string, Operation> {
                {"~" ,new Operation(x=>-x.Pop(),25,"~")},
                {"-" ,new Operation(x=>-x.Pop()+x.Pop(),10,"-")},
                {"+" ,new Operation(x=>x.Pop()+x.Pop(),10,"~")},
                {"*" ,new Operation(x=>x.Pop()*x.Pop(),50,"~")},
                {"/" ,new Operation(x=>1/x.Pop()*x.Pop(),50,"~")},
                {"^" ,new Operation(x=>{
                    double pow=x.Pop();
                    return Math.Pow(x.Pop(),pow);
                },100,"~")},
                { "sqr" ,new Operation(x=>{
                    double sqrt=x.Pop();
                    return sqrt*sqrt;
                },100,"sqr")}
                //{"~" ,new Operation((x,y)=>-x,10,"~")},
                //{"~" ,new Operation((x,y)=>-x,10,"~")},
                //{"~" ,new Operation((x,y)=>-x,10,"~")},
                //{"-" ,434},
                //{"+" ,434},
                //{"/" ,434},
                //{"^" ,434},
                //{"423" ,434},
                //{"423" ,434},
                //{"423" ,434},
                //{"423" ,434},
                //{"423" ,434},
                //{"423" ,434},
                //{"423" ,434},
                //{"423" ,434},
                //{"423" ,434},
                //{"423" ,434},
                //{"423" ,434},
                //{"423" ,434},
                //{"423" ,434},

            };
        }
        public Operation this[string key]
        {
            get
            {
                if (_operationsList.ContainsKey(key.ToLower()))
                    return _operationsList[key.ToLower()];
                throw new CalculationException($"Operation with key {key.ToLower()} was not found in dictionary");
            }
            private set
            {
                _operationsList[key.ToLower()] = value;
            }
        }

        public class CalculationException : Exception
        {
            public CalculationException(string message) : base(message)
            {

            }
        }
    }
}
