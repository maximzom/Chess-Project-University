namespace ChessLogic
{
    public class Knight : pieceChess
    {
        public override pieceType Type => pieceType.Knight;
        public override playerChess Color { get; }

        public Knight(playerChess color)
        {
            Color = color;
        }

        public override pieceChess Copy()
        {
            Knight copy = new Knight(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private static IEnumerable<positionChess> PotentialToPositions(positionChess from)
        {
            foreach (Direction vDir in new Direction[] { Direction.North, Direction.South })
            {
                foreach (Direction hDir in new Direction[] { Direction.West, Direction.East })
                {
                    yield return from + 2 * vDir + hDir;
                    yield return from + 2 * hDir + vDir;
                }
            }
        }

        private IEnumerable<positionChess> MovePositions(positionChess from, boardChess board)
        {
            return PotentialToPositions(from).Where(pos => boardChess.isInside(pos) 
                && (board.isEmpty(pos) || board[pos].Color != Color));
        }

        public override IEnumerable<Move> GetMoves(positionChess from, boardChess board)
        {
            return MovePositions(from, board).Select(to => new NormalMove(from, to));
        }
    }
}
