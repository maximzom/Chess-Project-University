using ChessLogic;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for PromotionMenu.xaml
    /// </summary>
    public partial class PromotionMenu : UserControl
    {
        public event Action<pieceType> PieceSelected;

        public PromotionMenu(playerChess player)
        {
            InitializeComponent();

            QueenImg.Source = Images.GetImage(player, pieceType.Queen);
            BishopImg.Source = Images.GetImage(player, pieceType.Bishop);
            RookImg.Source = Images.GetImage(player, pieceType.Rook);
            KnightImg.Source = Images.GetImage(player, pieceType.Knight);
        }

        private void QueenImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected?.Invoke(pieceType.Queen);
        }

        private void BishopImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected?.Invoke(pieceType.Bishop);
        }

        private void RookImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected?.Invoke(pieceType.Rook);
        }

        private void KnightImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected?.Invoke(pieceType.Knight);
        }
    }
}
