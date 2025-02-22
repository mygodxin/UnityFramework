using System;

class Node
{
    public string Value;
    public Node Left;
    public Node Right;

    public Node(string value)
    {
        this.Value = value;
        this.Left = null;
        this.Right = null;
    }

    public bool IsOperator()
    {
        return Value == "+" || Value == "-" || Value == "*" || Value == "/";
    }
}

/// <summary>
/// 二叉树表达式计算
/// </summary>
public class ExpressionTree
{
    private Node root;

    public ExpressionTree(string expression)
    {
        root = BuildExpressionTree(expression);
    }

    private Node BuildExpressionTree(string expression)
    {
        if (string.IsNullOrEmpty(expression))
            return null;

        expression = expression.Replace(" ", "");

        if (expression[0] == '(' && expression[expression.Length - 1] == ')')
        {
            int count = 0;
            bool flag = false;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(')
                {
                    count++;
                    flag = true;
                }
                else if (expression[i] == ')')
                    count--;

                if (count == 0 && flag && i != expression.Length - 1)
                {
                    flag = false;
                    break;
                }
            }

            if (flag)
                return BuildExpressionTree(expression.Substring(1, expression.Length - 2));
        }

        int index = GetLowestPriorityOperatorIndex(expression);
        if (index != -1)
        {
            Node node = new Node(expression[index].ToString());
            node.Left = BuildExpressionTree(expression.Substring(0, index));
            node.Right = BuildExpressionTree(expression.Substring(index + 1));
            return node;
        }
        else
        {
            return new Node(expression);
        }
    }

    private int GetLowestPriorityOperatorIndex(string expression)
    {
        int count = 0;
        int lowestPriority = int.MaxValue;
        int lowestPriorityIndex = -1;

        for (int i = expression.Length - 1; i >= 0; i--)
        {
            if (expression[i] == ')')
                count++;
            else if (expression[i] == '(')
                count--;
            else if (count == 0 && (expression[i] == '+' || expression[i] == '-' || expression[i] == '*' || expression[i] == '/'))
            {
                int priority = GetOperatorPriority(expression[i]);
                if (priority < lowestPriority)
                {
                    lowestPriority = priority;
                    lowestPriorityIndex = i;
                }
            }
        }

        return lowestPriorityIndex;
    }

    private int GetOperatorPriority(char op)
    {
        switch (op)
        {
            case '+':
            case '-':
                return 1;
            case '*':
            case '/':
                return 2;
            default:
                return 0;
        }
    }

    private double EvaluateExpression(Node node)
    {
        if (node.IsOperator())
        {
            double leftValue = EvaluateExpression(node.Left);
            double rightValue = EvaluateExpression(node.Right);

            switch (node.Value)
            {
                case "+":
                    return leftValue + rightValue;
                case "-":
                    return leftValue - rightValue;
                case "*":
                    return leftValue * rightValue;
                case "/":
                    return leftValue / rightValue;
                default:
                    throw new Exception("Invalid operator");
            }
        }
        else
        {
            try
            {
                return double.Parse(node.Value);
            }
            catch
            {
                throw new Exception("Find unreplaced char:" + node.Value + ",you should use Replace to replace");
            }
        }
    }

    /// <summary>
    /// 计算结果
    /// </summary>
    public double Evaluate()
    {
        return EvaluateExpression(root);
    }

    private void ReplaceNodeValue(Node node, string[] replaces)
    {
        if (node == null)
            return;
        var value = node.Value;
        for (int i = 0; i < replaces.Length; i += 2)
        {
            if (replaces[i] == value)
            {
                node.Value = replaces[i + 1];
            }
        }
        ReplaceNodeValue(node.Left, replaces);
        ReplaceNodeValue(node.Right, replaces);
    }

    /// <summary>
    /// 替换指定字符串 ["XX","5","YYY","6"]替换后所有XX=5,YYY=6
    /// </summary>
    public void Replace(params string[] replaces)
    {
        if (replaces.Length % 2 != 0)
            throw new Exception("Params must be Multiple of two.");
        ReplaceNodeValue(root, replaces);
    }
}