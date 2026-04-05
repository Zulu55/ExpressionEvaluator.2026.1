namespace ExpressionEvaluator.Core;

public class Evaluator
{
    public static double Evaluate(string infix)
    {

        infix = infix.Trim();

        var operators = new HashSet<char> { '+', '-', '*', '/', '^' };

        if (!infix.Any(operators.Contains))
            return double.Parse(infix);

        var parts = ExtractParts(infix);
        var postfix = InfixToPostfix(parts);
        return EvaluatePostfix(postfix);
    }

    private const string PI_VALUE = "3.141592653589793238";
    private const string E_VALUE = "2.718281828459045235";

    private static List<string> ExtractParts(string infix)
    {
        var parts = new List<string>();
        var number = "";

        foreach (var x in infix)
        {

            if (x == 'p')
            {
                parts.Add(PI_VALUE);
                continue;
            }

            if (x == 'e')
            {
                parts.Add(E_VALUE);
                continue;
            }

            if (char.IsDigit(x) || x == '.')
            {
                number += x;
            }
            else if (IsOperator(x.ToString()) || x == '(' || x == ')')
            {
                if (number != "")
                {
                    parts.Add(number);
                    number = "";
                }

                parts.Add(x.ToString());
            }
            else if (x == ' ')
            {
                continue;
            }
            else
            {
                throw new Exception("Digit Invalid");
            }
        }

        if (number != "")
        {
            parts.Add(number);
        }
        return parts;
    }

    private static List<string> InfixToPostfix(List<string> parts)
    {
        var postFix = new List<string>();
        var stack = new Stack<string>();
        foreach (var part in parts)
        {
            if (part == "(")
            {
                stack.Push(part);
            }
            else if (part == ")")
            {
                while (stack.Peek() != "(")
                    postFix.Add(stack.Pop());

                stack.Pop();
            }
            else if (IsOperator(part))
            {
                while (stack.Count > 0 && PriorityInfix(part) <= PriorityStack(stack.Peek()))
                {
                    postFix.Add(stack.Pop());
                }

                stack.Push(part);
            }
            else
            {
                postFix.Add(part);
            }

        }
        while (stack.Count > 0)
        {
            postFix.Add(stack.Pop());
        }
        return postFix;
    }

    private static int PriorityStack(string item) => item switch
    {
        "^" => 3,
        "*" => 2,
        "/" => 2,
        "+" => 1,
        "-" => 1,
        "(" => 0,
        _ => throw new Exception("Sintax error."),
    };

    private static int PriorityInfix(string item) => item switch
    {
        "^" => 4,
        "*" => 2,
        "/" => 2,
        "+" => 1,
        "-" => 1,
        "(" => 5,
        _ => throw new Exception("Sintax error."),
    };

    private static double EvaluatePostfix(List<string>postfix)
    {
        var stack = new Stack<double>();
        foreach (var part in postfix)
        {
            if (IsOperator(part))
            {
                var b = stack.Pop();
                var a = stack.Pop();
                stack.Push(part switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => a / b,
                    "^" => Math.Pow(a, b),
                    _ => throw new Exception("Sintax error."),
                });
            }
            else
            {
                stack.Push(double.Parse(part, System.Globalization.CultureInfo.InvariantCulture));
            }
        }
        return stack.Pop();
    }

    private static bool IsOperator(string item) => "+-*/^".Contains(item);
}