namespace ChessLogic
{
    public class Rook : pieceChess
    {
        public override pieceType Type => pieceType.Rook;
        public override playerChess Color { get; }

        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West
        };

        public Rook(playerChess color)
        {
            Color = color;
        }

        public override pieceChess Copy()
        {
            Rook copy = new Rook(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(positionChess from, boardChess board)
        {
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
