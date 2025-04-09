abstract class ParseTree
{
    abstract public String process();
}

class Node : ParseTree
{
    ParseTree left, right;
    public Node(ParseTree left, ParseTree right)
    {
        this.left = left;
        this.right = right;
    }
    public override string process()
    {
        return left.process() + right.process();
    }
}

// Text is an end-node and only returns the text it contains
class Text : ParseTree
{
    string text;
    public Text(string text)
    {
        this.text = text;
    }
    public override string process()
    {

        return text;
    }
}
// TODO: italic and bold should return bolded and italic html
class Italic : ParseTree
{
    ParseTree content;
    public Italic(ParseTree content)
    {
        this.content = content;
    }
    public override string process()
    {
        return "<i>" + content.process() + "</i>";
    }
}
class Bold : ParseTree
{
    ParseTree content;
    public Bold(ParseTree content)
    {
        this.content = content;
    }
    public override string process()
    {
        return "<b>" + content.process() + "</b>";
    }
}

class NewLine : ParseTree
{
    ParseTree before, after;
    public NewLine(ParseTree before, ParseTree after)
    {
        this.before = before;
        this.after = after;
    }
    public override string process()
    {
        return before.process() + "<br>\n" + after.process();
    }
}