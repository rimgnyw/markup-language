abstract class ParseTree
{
    abstract public String process();
}

class Bolded : ParseTree
{
    ParseTree child;
    public Bolded(ParseTree child)
    {
        this.child = child;
    }
    public override String process()
    {
        return "<b>" + child.process() + "</b>";
    }
}

class Italics : ParseTree
{
    ParseTree child;
    public Italics(ParseTree child)
    {
        this.child = child;
    }
    public override String process()
    {
        return "<em>" + child.process() + "</em>";
    }
}

class Str : ParseTree
{
    String data;
    public Str(String data)
    {
        this.data = data;
    }
    public override string process()
    {
        return data;
    }
}
