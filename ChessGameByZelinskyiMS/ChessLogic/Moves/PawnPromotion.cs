namespace ChessLogic
{
    public class PawnPromotion : Move
    {
        public override moveType Type => moveType.PawnPromotion;
        public override positionChess FromPos { get; }
        public override positionChess ToPos { get; }

        private readonly pieceType newType;

        public PawnPromotion(positionChess from, positionChess to, pieceType newType)
        {
            FromPos = from;
            ToPos = to;
            this.newType = newType;
        }

        private pieceChess CreatePromotionPiece(playerChess color)
        {
            return newType switch
            {
                pieceType.Knight => new Knight(color),
                pieceType.Bishop => new Bishop(color),
                pieceType.Rook => new Rook(color),
                _ => new Queen(color)
            };
        }

        public override bool Execute(boardChess board)
        {
            pieceChess pawn = board[FromPos];
            board[FromPos] = null;

            pieceChess promotionPiece = CreatePromotionPiece(pawn.Color);
            promotionPiece.HasMoved = true;
            board[ToPos] = promotionPiece;

            return true;
        }
    }
}
