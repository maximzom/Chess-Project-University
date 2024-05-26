namespace ChessLogic
{
    public class DoublePawn : Move
    {
        public override moveType Type => moveType.DoublePawn;
        public override positionChess FromPos { get; }
        public override positionChess ToPos { get; }

        private readonly positionChess skippedPos;

        public DoublePawn(positionChess from, positionChess to)
        {
            FromPos = from;
            ToPos = to;
            skippedPos = new positionChess((from.Row + to.Row) / 2, from.Column);
        }

        public override bool Execute(boardChess board)
        {
            playerChess player = board[FromPos].Color;
            board.setPawnSkipPosition(player, skippedPos);
            new NormalMove(FromPos, ToPos).Execute(board);

            return true;
        }
    }
}
