namespace ChessLogic
{
    public class positionChess
    {
        public int Row { get; }
        public int Column { get; }

        public positionChess(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public playerChess SquareColor()
        {
            if ((Row + Column) % 2 == 0)
            {
                return playerChess.White;
            }

            return playerChess.Black;
        }

        public override bool Equals(object obj)
        {
            return obj is positionChess position &&
                   Row == position.Row &&
                   Column == position.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public static bool operator ==(positionChess left, positionChess right)
        {
            return EqualityComparer<positionChess>.Default.Equals(left, right);
        }

        public static bool operator !=(positionChess left, positionChess right)
        {
            return !(left == right);
        }

        public static positionChess operator +(positionChess pos, Direction dir)
        {
            return new positionChess(pos.Row + dir.RowDelta, pos.Column + dir.ColumnDelta);
        }
    }
}
