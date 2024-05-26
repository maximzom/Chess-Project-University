namespace ChessLogic
{
    public class resultChess
    {
        public playerChess Winner { get; }
        public endReason Reason { get; }

        public resultChess(playerChess winner, endReason reason)
        {
            Winner = winner;
            Reason = reason;
        }

        public static resultChess Win(playerChess winner)
        {
            return new resultChess(winner, endReason.Checkmate);
        }

        public static resultChess Draw(endReason reason)
        {
            return new resultChess(playerChess.None, reason);
        }
    }
}
