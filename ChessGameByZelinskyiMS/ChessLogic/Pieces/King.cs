namespace ChessLogic
{
    public class King : pieceChess
    {
        public override pieceType Type => pieceType.King;
        public override playerChess Color { get; }

        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West,
            Direction.NorthWest,
            Direction.NorthEast,
            Direction.SouthWest,
            Direction.SouthEast
        };

        public King(playerChess color)
        {
            Color = color;
        }

        private static bool IsUnmovedRook(positionChess pos, boardChess board)
        {
            if (board.isEmpty(pos))
            {
                return false;
            }

            pieceChess piece = board[pos];
            return piece.Type == pieceType.Rook && !piece.HasMoved;
        }

        private static bool AllEmpty(IEnumerable<positionChess> positions, boardChess board)
        {
            return positions.All(pos => board.isEmpty(pos));
        }

        private bool CanCastleKingSide(positionChess from, boardChess board)
        {
            if (HasMoved)
            {
                return false;
            }

            positionChess rookPos = new positionChess(from.Row, 7);
            positionChess[] betweenPositions = new positionChess[] { new(from.Row, 5), new(from.Row, 6) };

            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        private bool CanCastleQueenSide(positionChess from, boardChess board)
        {
            if (HasMoved)
            {
                return false;
            }

            positionChess rookPos = new positionChess(from.Row, 0);
            positionChess[] betweenPositions = new positionChess[] { new(from.Row, 1), new(from.Row, 2), new(from.Row, 3) };

            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        public override pieceChess Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private IEnumerable<positionChess> MovePositions(positionChess from, boardChess board)
        {
            foreach (Direction dir in dirs)
            {
                positionChess to = from + dir;

                if (!boardChess.isInside(to))
                {
                    continue;
                }

                if (board.isEmpty(to) || board[to].Color != Color)
                {
                    yield return to;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(positionChess from, boardChess board)
        {
            foreach (positionChess to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);
            }

            if (CanCastleKingSide(from, board))
            {
                yield return new Castle(moveType.CastleKS, from);
            }

            if (CanCastleQueenSide(from, board))
            {
                yield return new Castle(moveType.CastleQS, from);
            }
        }

        public override bool CanCaptureOpponentKing(positionChess from, boardChess board)
        {
            return MovePositions(from, board).Any(to =>
            {
                pieceChess piece = board[to];
                return piece != null && piece.Type == pieceType.King;
            });
        }
    }
}
