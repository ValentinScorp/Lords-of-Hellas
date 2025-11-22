public abstract class Token
{
    public TokenType Type { get; private set; }

    protected Token(TokenType type) {
        Type = type;
    }
}