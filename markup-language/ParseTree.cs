/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

abstract class ParseTree {
    abstract public String process();
}
// generic node, used for tieing other nodes together to allow arbitrarily long documents
class Node : ParseTree {
    ParseTree left, right;
    public Node(ParseTree left, ParseTree right) {
        this.left = left;
        this.right = right;
    }
    public override string process() {
        return left.process() + right.process();
    }
}

class Paragraph : ParseTree {
    ParseTree content;
    public Paragraph(ParseTree content) {
        this.content = content;
    }
    public override string process() {
        return "<p>" + content.process() + "</p>\n";
    }
}

class ParagraphNode : ParseTree {
    ParseTree left, right;
    public ParagraphNode(ParseTree left, ParseTree right) {
        this.left = left;
        this.right = right;
    }
    public override string process() {
        return left.process() + right.process();
    }
}

class Text : ParseTree {
    string text;
    public Text(string text) {
        this.text = text;
    }
    public override string process() {

        // Text is an end-node and only returns the text it contains
        return text;
    }
}

class Header1 : ParseTree {
    ParseTree content;
    public Header1(ParseTree content) {
        this.content = content;
    }
    public override string process() {
        return "<h1>" + content.process() + "</h1>\n";
    }
}

class Italic : ParseTree {
    ParseTree content;
    public Italic(ParseTree content) {
        this.content = content;
    }
    public override string process() {
        return "<i>" + content.process() + "</i>";
    }
}
class Bold : ParseTree {
    ParseTree content;
    public Bold(ParseTree content) {
        this.content = content;
    }
    public override string process() {
        return "<b>" + content.process() + "</b>";
    }
}

// Node for parsing erroneous segments without crashing the program
class ErrorNode : ParseTree {

    string text;
    public ErrorNode() {
        this.text = "ERROR";
    }
    public override string process() {

        return text;
    }

}