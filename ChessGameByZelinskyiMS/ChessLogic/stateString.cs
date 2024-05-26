using System.Text;

namespace ChessLogic
{
    public class stateString
    {
        private readonly StringBuilder sb = new StringBuilder();

        public stateString(playerChess currentPlayer, boardChess board)
        {
            AddPiecePlacement(board);
            sb.Append(' ');
            AddCurrentPlayer(currentPlayer);
            sb.Append(' ');
            AddCastlingRights(board);
            sb.Append(' ');
            AddEnPassant(board, currentPlayer);
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        private static char PieceChar(pieceChess piece)
        {
            char c = piece.Type switch
            {
                pieceType.Pawn => 'p',
                pieceType.Knight => 'n',
                pieceType.Rook => 'r',
                pieceType.Bishop => 'b',
                pieceType.Queen => 'q',
                pieceType.King => 'k',
                _ => ' '
            };

            if (piece.Color == playerChess.White)
            {
                return char.ToUpper(c);
            }

            return c;
        }

        private void AddRowData(boardChess board, int row)
        {
            int empty = 0;

            for (int c = 0; c < 8; c++)
            {
                if (board[row, c] == null)
                {
                    empty++;
                    continue;
                }

                if (empty > 0)
                {
                    sb.Append(empty);
                    empty = 0;
                }

                sb.Append(PieceChar(board[row, c]));
            }

            if (empty > 0)
            {
                sb.Append(empty);
            }
        }

        private void AddPiecePlacement(boardChess board)
        {
            for (int r = 0; r < 8; r++)
            {
                if (r != 0)
                {
                    sb.Append('/');
                }

                AddRowData(board, r);
            }
        }

        private void AddCurrentPlayer(playerChess currentPlayer)
        {
            if (currentPlayer == playerChess.White)
            {
                sb.Append('w');
            }
            else
            {
                sb.Append('b');
            }
        }

        private void AddCastlingRights(boardChess board)
        {
            bool castleWKS = board.CastleRightKS(playerChess.White);
            bool castleWQS = board.CastleRightQS(playerChess.White);
            bool castleBKS = board.CastleRightKS(playerChess.Black);
            bool castleBQS = board.CastleRightQS(playerChess.Black);

            if (!(castleWKS || castleWQS || castleBKS || castleBQS))
            {
                sb.Append('-');
                return;
            }

            if (castleWKS)
            {
                sb.Append('K');
            }
            if (castleWQS)
            {
                sb.Append('Q');
            }
            if (castleBKS)
            {
                sb.Append('k');
            }
            if (castleBQS)
            {
                sb.Append('q');
            }
        }

        private void AddEnPassant(boardChess board, playerChess currentPlayer)
        {
            if (!board.CanCaptureEnPassant(currentPlayer))
            {
                sb.Append('-');
                return;
            }

            positionChess pos = board.getPawnSkipPosition(currentPlayer.Contender());
            char file = (char)('a' + pos.Column);
            int rank = 8 - pos.Row;
            sb.Append(file);
            sb.Append(rank);
        }
    }
}
