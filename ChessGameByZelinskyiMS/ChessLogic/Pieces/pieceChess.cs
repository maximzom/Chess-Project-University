namespace ChessLogic
{
    public abstract class pieceChess
    {
        public abstract pieceType Type { get; }
        public abstract playerChess Color { get; }
        public bool HasMoved { get; set; } = false;

        public abstract pieceChess Copy();

        public abstract IEnumerable<Move> GetMoves(positionChess from, boardChess board);

        protected IEnumerable<positionChess> MovePositionsInDir(positionChess from, boardChess board, Direction dir)
        {
            for (positionChess pos = from + dir; boardChess.isInside(pos); pos += dir)
            {
                if (board.isEmpty(pos))
                {
                    yield return pos;
                    continue;
                }

                pieceChess piece = board[pos];

                if (piece.Color != Color)
                {
                    yield return pos;
                }

                yield break;
            }
        }

        protected IEnumerable<positionChess> MovePositionsInDirs(positionChess from, boardChess board, Direction[] dirs)
        {
            return dirs.SelectMany(dir => MovePositionsInDir(from, board, dir));
        }

        public virtual bool CanCaptureOpponentKing(positionChess from, boardChess board)
        {
            return GetMoves(from, board).Any(move =>
            {
                pieceChess piece = board[move.ToPos];
                return piece != null && piece.Type == pieceType.King;
            });
        }
    }
}
