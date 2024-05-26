namespace ChessLogic
{
    public class Bishop : pieceChess
    {
        public override pieceType Type => pieceType.Bishop;
        public override playerChess Color { get; }

        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.NorthWest,
            Direction.NorthEast,
            Direction.SouthWest,
            Direction.SouthEast
        };

        public Bishop(playerChess color)
        {
            Color = color;
        }

        public override pieceChess Copy()
        {
            Bishop copy = new Bishop(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(positionChess from, boardChess board)
        {
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
