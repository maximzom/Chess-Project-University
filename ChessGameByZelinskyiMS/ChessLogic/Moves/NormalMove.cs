namespace ChessLogic
{
    public class NormalMove : Move
    {
        public override moveType Type => moveType.Normal;
        public override positionChess FromPos { get; }
        public override positionChess ToPos { get; }

        public NormalMove(positionChess from, positionChess to)
        {
            FromPos = from;
            ToPos = to;
        }

        public override bool Execute(boardChess board)
        {
            pieceChess piece = board[FromPos];
            bool capture = !board.isEmpty(ToPos);
            board[ToPos] = piece;
            board[FromPos] = null;
            piece.HasMoved = true;

            return capture || piece.Type == pieceType.Pawn;
        }
    }
}
