namespace ChessLogic
{
    public class Pawn : pieceChess
    {
        public override pieceType Type => pieceType.Pawn;
        public override playerChess Color { get; }

        private readonly Direction forward;

        public Pawn(playerChess color)
        {
            Color = color;

            if (color == playerChess.White)
            {
                forward = Direction.North;
            }
            else if (color == playerChess.Black)
            {
                forward = Direction.South;
            }
        }

        public override pieceChess Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private static bool CanMoveTo(positionChess pos, boardChess board)
        {
            return boardChess.isInside(pos) && board.isEmpty(pos);
        }

        private bool CanCaptureAt(positionChess pos, boardChess board)
        {
            if (!boardChess.isInside(pos) || board.isEmpty(pos))
            {
                return false;
            }

            return board[pos].Color != Color;
        }

        private static IEnumerable<Move> PromotionMoves(positionChess from, positionChess to)
        {
            yield return new PawnPromotion(from, to, pieceType.Knight);
            yield return new PawnPromotion(from, to, pieceType.Bishop);
            yield return new PawnPromotion(from, to, pieceType.Rook);
            yield return new PawnPromotion(from, to, pieceType.Queen);
        }

        private IEnumerable<Move> ForwardMoves(positionChess from, boardChess board)
        {
            positionChess oneMovePos = from + forward;

            if (CanMoveTo(oneMovePos, board))
            {
                if (oneMovePos.Row == 0 || oneMovePos.Row == 7)
                {
                    foreach (Move promMove in PromotionMoves(from, oneMovePos))
                    {
                        yield return promMove;
                    }
                }
                else
                {
                    yield return new NormalMove(from, oneMovePos);
                }

                positionChess twoMovesPos = oneMovePos + forward;

                if (!HasMoved && CanMoveTo(twoMovesPos, board))
                {
                    yield return new DoublePawn(from, twoMovesPos);
                }
            }
        }

        private IEnumerable<Move> DiagonalMoves(positionChess from, boardChess board)
        {
            foreach (Direction dir in new Direction[] { Direction.West, Direction.East })
            {
                positionChess to = from + forward + dir;

                if (to == board.getPawnSkipPosition(Color.Contender()))
                {
                    yield return new EnPassant(from, to);
                }
                else if (CanCaptureAt(to, board))
                {
                    if (to.Row == 0 || to.Row == 7)
                    {
                        foreach (Move promMove in PromotionMoves(from, to))
                        {
                            yield return promMove;
                        }
                    }
                    else
                    {
                        yield return new NormalMove(from, to);
                    }
                }
            }
        }

        public override IEnumerable<Move> GetMoves(positionChess from, boardChess board)
        {
            return ForwardMoves(from, board).Concat(DiagonalMoves(from, board));
        }

        public override bool CanCaptureOpponentKing(positionChess from, boardChess board)
        {
            return DiagonalMoves(from, board).Any(move =>
            {
                pieceChess piece = board[move.ToPos];
                return piece != null && piece.Type == pieceType.King;
            });
        }
    }
}
