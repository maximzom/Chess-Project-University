namespace ChessLogic
{
    public class EnPassant : Move
    {
        public override moveType Type => moveType.EnPassant;
        public override positionChess FromPos { get; }
        public override positionChess ToPos { get; }

        private readonly positionChess capturePos;

        public EnPassant(positionChess from, positionChess to)
        {
            FromPos = from;
            ToPos = to;
            capturePos = new positionChess(from.Row, to.Column);
        }

        public override bool Execute(boardChess board)
        {
            new NormalMove(FromPos, ToPos).Execute(board);
            board[capturePos] = null;

            return true;
        }
    }
}
