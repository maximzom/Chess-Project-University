using ChessLogic;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public event Action<Option> OptionSelected;

        public GameOverMenu(gameStateChess gameState)
        {
            InitializeComponent();

            resultChess result = gameState.Result;
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = GetReasonText(result.Reason, gameState.currentPlayer);
        }

        private static string GetWinnerText(playerChess winner)
        {
            return winner switch
            {
                playerChess.White => "БІЛІ ПЕРЕМОГЛИ!",
                playerChess.Black => "ЧОРНІ ПЕРЕМОГЛИ!",
                _ => "IT'S A DRAW"
            };
        }

        private static string PlayerString(playerChess player)
        {
            return player switch
            {
                playerChess.White => "У БІЛИХ",
                playerChess.Black => "У ЧОРНИХ",
                _ => ""
            };
        }

        private static string GetReasonText(endReason reason, playerChess currentPlayer)
        {
            return reason switch
            {
                endReason.Stalemate => $"ПАТ - {PlayerString(currentPlayer)} НЕМАЄ ХОДІВ.",
                endReason.Checkmate => $"МАТ - {PlayerString(currentPlayer)} НЕМАЄ ХОДІВ.",
                endReason.FiftyMoveRule => "ПРАВИЛО П'ЯТДЕСЯТИ ХОДІВ",
                endReason.InsufficientMaterial => "НЕДОСТАТНІЙ МАТЕРІАЛ",
                endReason.ThreefoldRepetition => "ТРИРАЗОВЕ ПОВТОРЕННЯ",
                _ => ""
            };
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Restart);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Exit);
        }
    }
}
