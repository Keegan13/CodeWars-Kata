namespace KataCSharp.Calculator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class Evaluator
    {
        public class Operation
        {
            public Operation(char op, Action<Stack<double>> func, int priority)
            {
                this.Operator = op;
                this.Involve = func;
                this.Priority = priority;
            }
            public readonly char Operator;
            public readonly Action<Stack<double>> Involve;
            public readonly int Priority;
        }

        private readonly Stack<double> _valuesStack = new Stack<double>();
        private readonly Stack<Operation> _operationsStack = new Stack<Operation>();
        public readonly Operation[] Operations;

        public Evaluator() : this(DefalutOperations()) { }
        public Evaluator(IEnumerable<Operation> operations)
        {
            this.Operations = operations.ToArray();
            this._valuesStack = new Stack<double>();
            this._operationsStack = new Stack<Operation>();
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        }

        public void Reset()
        {
            this._valuesStack.Clear();
            this._operationsStack.Clear();
        }
        public double Evaluate(string expression)
        {
            expression = ReplaceUnaryMinus(RemoveWhiteSpaces(expression));
            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsDigit(expression[i])) PushNumber(expression, ref i);
                else
                {
                    HandleOperation(expression, ref i);
                }
            }
            EvaluateStack();
            var result = _valuesStack.Count() == 1 ? _valuesStack.Pop() : 0;
            Reset();
            return result;
        }
        protected virtual void EvaluateStack(bool tillBracket = true)
        {
            while (_operationsStack.Count > 0)
            {
                var op = _operationsStack.Pop();
                if (op.Operator == '(')
                {
                    if (tillBracket) _operationsStack.Push(op);
                    break;
                }
                else op.Involve(_valuesStack);
            }
        }
        protected virtual void PushNumber(string expression, ref int i)
        {
            int k = 0;
            while (i + k < expression.Length && (char.IsDigit(expression[i + k]) || expression[i + k] == '.')) k++;
            _valuesStack.Push(double.Parse(expression.Substring(i, k)));
            i += k - 1;
        }
        protected virtual void HandleOperation(string expression, ref int i)
        {
            int index = i;
            if (Operations.Single(x => x.Operator == expression[index]) is Operation op)
            {
                if (_operationsStack.Count > 0 && (_operationsStack.Peek().Priority >= op.Priority || op.Operator == ')'))
                {
                    EvaluateStack(op.Operator != ')');
                }
                if (op.Operator != ')')
                    _operationsStack.Push(op);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(expression), $"Operation with operator {expression[i].ToString()} was not found in current instance of Evaluator");
            }
            i = index;
        }

        public static IEnumerable<Operation> DefalutOperations()
        {
            var defOp = new Operation[7];
            defOp[0] = new Operation('+', x => x.Push(x.Pop() + x.Pop()), 10);
            defOp[1] = new Operation('-', x => x.Push(-x.Pop() + x.Pop()), 10);
            defOp[2] = new Operation('*', x => x.Push(x.Pop() * x.Pop()), 20);
            defOp[3] = new Operation('/', x =>
            {
                var d = x.Pop();
                x.Push(x.Pop() / d);
            }, 20);
            defOp[4] = new Operation('~', x => x.Push(-x.Pop()), 25);
            defOp[5] = new Operation(')', x => { }, 200);
            defOp[6] = new Operation('(', x => { }, 200);
            return defOp;
        }
        public static string ReplaceUnaryMinus(string expression)
        {
            string output = "";
            //ToDo do with RegExp
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '-' && (i == 0 || (!char.IsDigit(expression[i - 1]) && expression[i - 1] != ')'))) output += '~';
                else output += expression[i];
            }
            return output;
        }
        public static string RemoveWhiteSpaces(string input)
        {
            string output = "";
            foreach (var character in input)
            {
                if (character != ' ') output += character.ToString();
            }
            return output;
        }
    }
}
