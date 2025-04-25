/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using static TokenType;
using System.Text.RegularExpressions;

class Lexer {
    private int start = 0;
    private int current = 0;
    private int line = 1;
    private String source;
    private readonly List<Token> tokens = new List<Token>();

    public Lexer(String source) {
        this.source = source.Replace("\r\n", "\n"); // sanitise input to avoid windows garbash
    }

    public List<Token> scanTokens() {
        while (!endOfLine()) {
            start = current;
            scanToken();
        }
        tokens.Add(new Token(EOF, "", line));
        return tokens;
    }

    private void scanToken() {
        char c = advance();

        // anything that isn't indentified as a token is read as text, including parts of tokens
        switch (c) {
            case '*':
                if (match('*')) addToken(ITALIC);
                else text(); break;
            case '\'':
                if (match('\'')) addToken(BOLD);
                else text(); break;
            case '#':
                if (match('#')) header();
                else text(); break;
            case '\n':
                line++;
                addToken(NL); break;
            default:
                text();
                break;

        }
    }

    private void text() {
        Regex rg = new Regex(@"(\*\*)|(\'\')");
        // continue through the text token until it's broken by a different token
        while (!rg.IsMatch(peek().ToString() + peekNext().ToString()) && peek().ToString() != "\n" && !endOfLine()) {
            advance();
        }

        // remove trailing symbol(s) to get text token content
        String value = source.Substring(start, current - start);

        addToken(TEXT, value);

    }

    private void header() {
        while (peek().ToString() != "\n" && !endOfLine()) {
            advance();
        }

        // remove trailing symbol(s) to get text token content
        String value = source.Substring(start + 2, current - start - 2);

        addToken(H1, value);

    }

    private bool match(char expected) {
        if (endOfLine()) return false;
        if (source[current] != expected) return false;
        current++;
        return true;
    }

    private char peek() {
        if (endOfLine()) return '\0';
        return source[current];
    }
    private char peekNext() {
        if (current + 1 > source.Length - 1)
            return '\0';
        return source[current + 1];
    }

    private bool endOfLine() {
        return current >= source.Length;
    }

    private char advance() {
        return source[current++];
    }
    private void addToken(TokenType type) {
        addToken(type, "");
    }
    private void addToken(TokenType type, string literal) {

        tokens.Add(new Token(type, literal, line));
    }
}