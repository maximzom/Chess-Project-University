namespace ChessLogic
{
    public class gameStateChess
    {
        public boardChess Board { get; }
        public playerChess currentPlayer { get; private set; }
        public resultChess Result { get; private set; } = null;

        private int noCaptureOrPawnMoves = 0;
        private string stateString;

        private readonly Dictionary<string, int> stateHistory = new Dictionary<string, int>();

        public gameStateChess(playerChess player, boardChess board)
        {
            currentPlayer = player;
            Board = board;

            stateString = new stateString(currentPlayer, board).ToString();
            stateHistory[stateString] = 1;
        }

        public IEnumerable<Move> legalMovesForPiece(positionChess pos)
        {
            if (Board.isEmpty(pos) || Board[pos].Color != currentPlayer)
            {
                return Enumerable.Empty<Move>();
            }

            pieceChess piece = Board[pos];
            IEnumerable<Move> moveCandidates = piece.GetMoves(pos, Board);
            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        public void makeMove(Move move)
        {
            Board.setPawnSkipPosition(currentPlayer, null);
            bool captureOrPawn = move.Execute(Board);

            if (captureOrPawn)
            {
                noCaptureOrPawnMoves = 0;
                stateHistory.Clear();
            }
            else
            {
                noCaptureOrPawnMoves++;
            }

            currentPlayer = currentPlayer.Contender();
            UpdateStateString();
            CheckForGameOver();
        }

        public IEnumerable<Move> AllLegalMovesFor(playerChess player)
        {
            IEnumerable<Move> moveCandidates = Board.piecePositionsFor(player).SelectMany(pos =>
            {
                pieceChess piece = Board[pos];
                return piece.GetMoves(pos, Board);
            });

            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        private void CheckForGameOver()
        {
            if (!AllLegalMovesFor(currentPlayer).Any())
            {
                if (Board.IsInCheck(currentPlayer))
                {
                    Result = resultChess.Win(currentPlayer.Contender());
                }
                else
                {
                    Result = resultChess.Draw(endReason.Stalemate);
                }
            }
            else if (Board.InsufficientMaterial())
            {
                Result = resultChess.Draw(endReason.InsufficientMaterial);
            }
            else if (FiftyMoveRule())
            {
                Result = resultChess.Draw(endReason.FiftyMoveRule);
            }
            else if (ThreefoldRepetition())
            {
                Result = resultChess.Draw(endReason.ThreefoldRepetition);
            }
        }

        public bool IsGameOver()
        {
            return Result != null;
        }

        private bool FiftyMoveRule()
        {
            int fullMoves = noCaptureOrPawnMoves / 2;
            return fullMoves == 50;
        }

        private void UpdateStateString()
        {
            stateString = new stateString(currentPlayer, Board).ToString();

            if (!stateHistory.ContainsKey(stateString))
            {
                stateHistory[stateString] = 1;
            }
            else
            {
                stateHistory[stateString]++;
            }
        }

        private bool ThreefoldRepetition()
        {
            return stateHistory[stateString] == 3;
        }
    }
}
