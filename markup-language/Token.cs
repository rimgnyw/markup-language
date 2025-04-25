/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

class Token {
    public readonly TokenType type;
    public readonly string literal;
    public readonly int line;

    public Token(TokenType type, string literal, int line) {
        this.type = type;
        this.literal = literal;
        this.line = line;

    }
    public TokenType getType() { return type; }
    public string getText() { return literal.ToString(); }
}