namespace ChessLogic
{
    public enum playerChess
    {
        None,
        White,
        Black
    }

    public static class PlayerExtensions
    {
        public static playerChess Contender(this playerChess player)
        {
            return player switch
            {
                playerChess.White => playerChess.Black,
                playerChess.Black => playerChess.White,
                _ => playerChess.None,
            };
        }
    }
}
