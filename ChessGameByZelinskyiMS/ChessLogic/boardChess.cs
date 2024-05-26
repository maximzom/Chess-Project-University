namespace ChessLogic
{
    public class boardChess
    {
        private readonly pieceChess[,] pieces = new pieceChess[8, 8];

        private readonly Dictionary<playerChess, positionChess> pawnSkipPositions = new Dictionary<playerChess, positionChess>
        {
            { playerChess.White, null },
            { playerChess.Black, null }
        };

        public pieceChess this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        public pieceChess this[positionChess pos]
        {
            get { return this[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value; }
        }

        public positionChess getPawnSkipPosition(playerChess player)
        {
            return pawnSkipPositions[player];
        }

        public void setPawnSkipPosition(playerChess player, positionChess pos)
        {
            pawnSkipPositions[player] = pos;
        }

        public static boardChess Initial()
        {
            boardChess board = new boardChess();
            board.addStartPieces();
            return board;
        }

        private void addStartPieces()
        {
            this[0, 0] = new Rook(playerChess.Black);
            this[0, 1] = new Knight(playerChess.Black);
            this[0, 2] = new Bishop(playerChess.Black);
            this[0, 3] = new Queen(playerChess.Black);
            this[0, 4] = new King(playerChess.Black);
            this[0, 5] = new Bishop(playerChess.Black);
            this[0, 6] = new Knight(playerChess.Black);
            this[0, 7] = new Rook(playerChess.Black);

            this[7, 0] = new Rook(playerChess.White);
            this[7, 1] = new Knight(playerChess.White);
            this[7, 2] = new Bishop(playerChess.White);
            this[7, 3] = new Queen(playerChess.White);
            this[7, 4] = new King(playerChess.White);
            this[7, 5] = new Bishop(playerChess.White);
            this[7, 6] = new Knight(playerChess.White);
            this[7, 7] = new Rook(playerChess.White);

            for (int c = 0; c < 8; c++)
            {
                this[1, c] = new Pawn(playerChess.Black);
                this[6, c] = new Pawn(playerChess.White);
            }
        }

        public static bool isInside(positionChess pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        public bool isEmpty(positionChess pos)
        {
            return this[pos] == null;
        }

        public IEnumerable<positionChess> piecePositions()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    positionChess pos = new positionChess(r, c);

                    if (!isEmpty(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }

        public IEnumerable<positionChess> piecePositionsFor(playerChess player)
        {
            return piecePositions().Where(pos => this[pos].Color == player);
        }

        public bool IsInCheck(playerChess player)
        {
            return piecePositionsFor(player.Contender()).Any(pos =>
            {
                pieceChess piece = this[pos];
                return piece.CanCaptureOpponentKing(pos, this);
            });
        }

        public boardChess Copy()
        {
            boardChess copy = new boardChess();

            foreach (positionChess pos in piecePositions())
            {
                copy[pos] = this[pos].Copy();
            }

            return copy;
        }

        public countingChess CountPieces()
        {
            countingChess counting = new countingChess();

            foreach (positionChess pos in piecePositions())
            {
                pieceChess piece = this[pos];
                counting.Increment(piece.Color, piece.Type);
            }

            return counting;
        }

        public bool InsufficientMaterial()
        {
            countingChess counting = CountPieces();

            return IsKingVKing(counting) || IsKingBishopVKing(counting) ||
                   IsKingKnightVKing(counting) || IsKingBishopVKingBishop(counting);
        }

        private static bool IsKingVKing(countingChess counting)
        {
            return counting.TotalCount == 2;
        }

        private static bool IsKingBishopVKing(countingChess counting)
        {
            return counting.TotalCount == 3 && (counting.White(pieceType.Bishop) == 1 || counting.Black(pieceType.Bishop) == 1);
        }

        private static bool IsKingKnightVKing(countingChess counting)
        {
            return counting.TotalCount == 3 && (counting.White(pieceType.Knight) == 1 || counting.Black(pieceType.Knight) == 1);
        }

        private bool IsKingBishopVKingBishop(countingChess counting)
        {
            if (counting.TotalCount != 4)
            {
                return false;
            }

            if (counting.White(pieceType.Bishop) != 1 || counting.Black(pieceType.Bishop) != 1)
            {
                return false;
            }

            positionChess wBishopPos = FindPiece(playerChess.White, pieceType.Bishop);
            positionChess bBishopPos = FindPiece(playerChess.Black, pieceType.Bishop);

            return wBishopPos.SquareColor() == bBishopPos.SquareColor();
        }

        private positionChess FindPiece(playerChess color, pieceType type)
        {
            return piecePositionsFor(color).First(pos => this[pos].Type == type);
        }

        private bool IsUnmovedKingAndRook(positionChess kingPos, positionChess rookPos)
        {
            if (isEmpty(kingPos) || isEmpty(rookPos))
            {
                return false;
            }

            pieceChess king = this[kingPos];
            pieceChess rook = this[rookPos];

            return king.Type == pieceType.King && rook.Type == pieceType.Rook &&
                   !king.HasMoved && !rook.HasMoved;
        }

        public bool CastleRightKS(playerChess player)
        {
            return player switch
            {
                playerChess.White => IsUnmovedKingAndRook(new positionChess(7, 4), new positionChess(7, 7)),
                playerChess.Black => IsUnmovedKingAndRook(new positionChess(0, 4), new positionChess(0, 7)),
                _ => false
            };
        }

        public bool CastleRightQS(playerChess player)
        {
            return player switch
            {
                playerChess.White => IsUnmovedKingAndRook(new positionChess(7, 4), new positionChess(7, 0)),
                playerChess.Black => IsUnmovedKingAndRook(new positionChess(0, 4), new positionChess(0, 0)),
                _ => false
            };
        }

        private bool HasPawnInPosition(playerChess player, positionChess[] pawnPositions, positionChess skipPos)
        {
            foreach (positionChess pos in pawnPositions.Where(isInside))
            {
                pieceChess piece = this[pos];
                if (piece == null || piece.Color != player || piece.Type != pieceType.Pawn)
                {
                    continue;
                }

                EnPassant move = new EnPassant(pos, skipPos);
                if (move.IsLegal(this))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanCaptureEnPassant(playerChess player)
        {
            positionChess skipPos = getPawnSkipPosition(player.Contender());

            if (skipPos == null)
            {
                return false;
            }

            positionChess[] pawnPositions = player switch
            {
                playerChess.White => new positionChess[] { skipPos + Direction.SouthWest, skipPos + Direction.SouthEast },
                playerChess.Black => new positionChess[] { skipPos + Direction.NorthWest, skipPos + Direction.NorthEast },
                _ => Array.Empty<positionChess>()
            };

            return HasPawnInPosition(player, pawnPositions, skipPos);
        }
    }
}
