namespace ChessLogic
{
    public class Castle : Move
    {
        public override moveType Type { get; }
        public override positionChess FromPos { get; }
        public override positionChess ToPos { get; }

        private readonly Direction kingMoveDir;
        private readonly positionChess rookFromPos;
        private readonly positionChess rookToPos;

        public Castle(moveType type, positionChess kingPos)
        {
            Type = type;
            FromPos = kingPos;

            if (type == moveType.CastleKS)
            {
                kingMoveDir = Direction.East;
                ToPos = new positionChess(kingPos.Row, 6);
                rookFromPos = new positionChess(kingPos.Row, 7);
                rookToPos = new positionChess(kingPos.Row, 5);
            }
            else if (type == moveType.CastleQS)
            {
                kingMoveDir = Direction.West;
                ToPos = new positionChess(kingPos.Row, 2);
                rookFromPos = new positionChess(kingPos.Row, 0);
                rookToPos = new positionChess(kingPos.Row, 3);
            }
        }

        public override bool Execute(boardChess board)
        {
            new NormalMove(FromPos, ToPos).Execute(board);
            new NormalMove(rookFromPos, rookToPos).Execute(board);

            return false;
        }

        public override bool IsLegal(boardChess board)
        {
            playerChess player = board[FromPos].Color;

            if (board.IsInCheck(player))
            {
                return false;
            }

            boardChess copy = board.Copy();
            positionChess kingPosInCopy = FromPos;

            for (int i = 0; i < 2; i++)
            {
                new NormalMove(kingPosInCopy, kingPosInCopy + kingMoveDir).Execute(copy);
                kingPosInCopy += kingMoveDir;

                if (copy.IsInCheck(player))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
