namespace ChessLogic
{
    public abstract class Move
    {
        public abstract moveType Type { get; }
        public abstract positionChess FromPos { get; }
        public abstract positionChess ToPos { get; }

        public abstract bool Execute(boardChess board);

        public virtual bool IsLegal(boardChess board)
        {
            playerChess player = board[FromPos].Color;
            boardChess boardCopy = board.Copy();
            Execute(boardCopy);
            return !boardCopy.IsInCheck(player);
        }
    }
}
