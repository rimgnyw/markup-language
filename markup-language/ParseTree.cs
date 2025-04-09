abstract class ParseTree {
    abstract public String process();
}

class Line : ParseTree {
    ParseTree left, right;
    public Line(ParseTree left, ParseTree right) {
        this.left = left;
        this.right = right;
    }
    public override string process() {
        return left.process() + right.process();
    }
}

class Text : ParseTree {
    ParseTree left, center, right;
    public Text(ParseTree format, ParseTree content) {
        this.left = format;
        this.right = format;
        this.center = content;
    }
    public override string process() {
        return left.process() + center.process() + right.process();
    }
}

class Bolded : ParseTree {
    ParseTree child;
    public Bolded(ParseTree child) {
        this.child = child;
    }
    public override String process() {
        return "<b>" + child.process() + "</b>";
    }
}

class Italics : ParseTree {
    ParseTree child;
    public Italics(ParseTree child) {
        this.child = child;
    }
    public override String process() {
        return "<em>" + child.process() + "</em>";
    }
}

class Str : ParseTree {
    String data;
    public Str(String data) {
        this.data = data;
    }
    public override string process() {
        return data;
    }
}

class NewLine : ParseTree {
    ParseTree left, right;
    public NewLine(ParseTree left, ParseTree right) {
        this.left = left;
        this.right = right;
    }
    public override string process() {
        return left.process() + "<br>" + right.process();
    }
}