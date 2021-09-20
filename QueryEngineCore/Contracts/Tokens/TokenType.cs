namespace QueryEngineCore.Contracts.Tokens
{
    public enum TokenType
    {
        From,
        Where,
        Select,
        Id,
        String,
        Number,
        LogOp,
        RelOp,
        Comma,
        RParen,
        LParen,
        Ws,
    }
}