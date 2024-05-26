namespace ChessLogic
{
    public class countingChess
    {
        private readonly Dictionary<pieceType, int> whiteCount = new();
        private readonly Dictionary<pieceType, int> blackCount = new();

        public int TotalCount { get; private set; }

        public countingChess()
        {
            foreach (pieceType type in Enum.GetValues(typeof(pieceType)))
            {
                whiteCount[type] = 0;
                blackCount[type] = 0;
            }
        }

        public void Increment(playerChess color, pieceType type)
        {
            if (color == playerChess.White)
            {
                whiteCount[type]++;
            }
            else if (color == playerChess.Black)
            {
                blackCount[type]++;
            }

            TotalCount++;
        }

        public int White(pieceType type)
        {
            return whiteCount[type];
        }

        public int Black(pieceType type)
        {
            return blackCount[type];
        }
    }
}
