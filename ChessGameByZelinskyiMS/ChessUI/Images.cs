using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessLogic;

namespace ChessUI
{
    public static class Images
    {
        private static readonly Dictionary<pieceType, ImageSource> whiteSources = new()
        {
            { pieceType.Pawn, LoadImage("Assets/PawnW.png") },
            { pieceType.Bishop, LoadImage("Assets/BishopW.png") },
            { pieceType.Knight, LoadImage("Assets/KnightW.png") },
            { pieceType.Rook, LoadImage("Assets/RookW.png") },
            { pieceType.Queen, LoadImage("Assets/QueenW.png") },
            { pieceType.King, LoadImage("Assets/KingW.png") }
        };

        private static readonly Dictionary<pieceType, ImageSource> blackSources = new()
        {
            { pieceType.Pawn, LoadImage("Assets/PawnB.png") },
            { pieceType.Bishop, LoadImage("Assets/BishopB.png") },
            { pieceType.Knight, LoadImage("Assets/KnightB.png") },
            { pieceType.Rook, LoadImage("Assets/RookB.png") },
            { pieceType.Queen, LoadImage("Assets/QueenB.png") },
            { pieceType.King, LoadImage("Assets/KingB.png") }
        };

        private static ImageSource LoadImage(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }

        public static ImageSource GetImage(playerChess color, pieceType type)
        {
            return color switch
            {
                playerChess.White => whiteSources[type],
                playerChess.Black => blackSources[type],
                _ => null
            };
        }

        public static ImageSource GetImage(pieceChess piece)
        {
            if (piece == null)
            {
                return null;
            }

            return GetImage(piece.Color, piece.Type);
        }
    }
}
