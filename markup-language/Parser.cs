/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using static TokenType;
class Parser {
    private int current = 0;
    private readonly List<Token> tokens = new List<Token>();

    public Parser(List<Token> tokens) {
        this.tokens = tokens;
    }

    public ParseTree parse() {
        ParseTree result = S();
        return result;
    }

    public ParseTree S() {
        ParseTree result = paragraph();

        // Tail
        while (peekToken().getType() != EOF) {
            ParseTree tail = paragraph();
            result = new ParagraphNode(result, tail);
        }

        return result;
    }
    public ParseTree paragraph() {
        ParseTree result;
        Token token = peekToken();
        if (token.getType() == H1) {
            nextToken(); // consume header
            ParseTree content = textSegment();
            // result = new Header1(content);
            while (peekToken().getType() != NL) {
                ParseTree next = textSegment();
                content = new Node(content, next);
            }
            nextToken(); // consume newline
            return new Header1(content);
            // return result;
        }
        else {
            result = textSegment();
        }
        // MoreText
        while (peekToken().getType() != EOF) {
            if (peekToken().getType() == NL) {
                nextToken(); // consume the new line
                return new Paragraph(result);
            }
            else {
                ParseTree next = textSegment();
                result = new Node(result, next);
            }
        }
        return new Paragraph(result);
    }
    public ParseTree textSegment() {
        try {
            ParseTree result;
            Token token = nextToken();
            if (token.getType() == TEXT)
                result = new Text(token.getText());
            else if (token.getType() == ITALIC || token.getType() == BOLD)
                result = formattedText();
            else {
                throw Program.error(token.line, $"invalid token: {token.getType()}");
            }
            return result;
        }
        catch (ParseError) {
            // We want to allow the program to continue so we can find more possible syntax errors
            // but we don't want the program to output anything so we exit the program if we reach end of file with errors
            if (peekToken().getType() == EOF)
                Environment.Exit(1); // exit at EOF
            return new ErrorNode();
        }
    }

    public ParseTree formattedText() {
        ParseTree result;
        Token token = previousToken();
        if (token.getType() == ITALIC) {
            if (peekToken().getType() == ITALIC) throw Program.error(token.line, "Too many italics", "extra symbols");
            ParseTree content = textSegment();
            while (peekToken().getType() != ITALIC && peekToken().getType() != EOF) {
                if (peekToken().getType() == NL) {
                    nextToken(); // consume the new line
                    return content;
                    // ParseTree next = textSegment();
                    // content = new NewLine(content, next);
                }
                else {
                    ParseTree next = textSegment();
                    content = new Node(content, next);
                }
            }
            if (nextToken().getType() != ITALIC) throw Program.error(token.line, "Missing closing italics marker", "missing closing symbol");
            result = new Italic(content);
        }
        else if (token.getType() == BOLD) {
            if (peekToken().getType() == BOLD) throw Program.error(token.line, "Too many bolded", "extra symbols"); ParseTree content = textSegment();
            while (peekToken().getType() != BOLD && peekToken().getType() != EOF) {
                if (peekToken().getType() == NL) {
                    nextToken(); // consume the new line
                    return content;
                    // ParseTree next = textSegment();
                    // content = new NewLine(content, next);
                }
                else {
                    ParseTree next = textSegment();
                    content = new Node(content, next);
                }
            }
            if (nextToken().getType() != BOLD) throw Program.error(token.line, "Missing closing bolded marker", "missing closing symbol"); result = new Bold(content);
        }
        else {
            throw Program.error(token.line, "invalid token  " + token.getType());
        }
        return result;

    }

    public Token peekToken() {
        if (current > tokens.Count - 1) throw new Exception("Token pointer out of range"); // it should be impossible for a user to get this error
        return tokens[current];
    }
    public Token nextToken() {
        Token result = peekToken();
        current++;
        return result;
    }
    public Token previousToken() {
        if (current - 1 < 0) throw new Exception("No previous token"); // it should be impossible for a user to get this error
        return tokens[current - 1];
    }


}