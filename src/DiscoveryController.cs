
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Diagnostics;
using SwinGameSDK;

/// <summary>
/// The battle phase is handled by the DiscoveryController.
/// </summary>
static class DiscoveryController
{

	/// <summary>
	/// Handles input during the discovery phase of the game.
	/// </summary>
	/// <remarks>
	/// Escape opens the game menu. Clicking the mouse will
	/// attack a location.
	/// </remarks>
	public static void HandleDiscoveryInput()
	{
		if (SwinGame.KeyTyped(KeyCode.EscapeKey)) {
			GameController.AddNewState(GameState.ViewingGameMenu);
		}

		if (SwinGame.MouseClicked(MouseButton.LeftButton)) {
			DoAttack();
		}
	}

	/// <summary>
	/// Attack the location that the mouse if over.
	/// </summary>
	private static void DoAttack()
	{
		var mouse = default(Point2D);

		mouse = SwinGame.MousePosition();

		//Calculate the row/col clicked
		int row = 0;
		int col = 0;
		row = Convert.ToInt32(Math.Floor((mouse.Y - UtilityFunctions._fieldTop) / (UtilityFunctions._cellHeight + UtilityFunctions._cellGap)));
		col = Convert.ToInt32(Math.Floor((mouse.X - UtilityFunctions._fieldLeft) / (UtilityFunctions._cellWidth + UtilityFunctions._cellGap)));

		if (row >= 0 & row < GameController.HumanPlayer.EnemyGrid.Height) {
			if (col >= 0 & col < GameController.HumanPlayer.EnemyGrid.Width) {
				GameController.Attack(row, col);
			}
		}
	}

	/// <summary>
	/// Draws the game during the attack phase.
	/// </summary>s
	public static void DrawDiscovery()
	{
		const int _scoresLeft = 172;
		const int _shotsTop = 157;
		const int _hitsTop = 206;
		const int _splashTop = 256;

		if ((SwinGame.KeyDown(KeyCode.LeftShiftKey) | SwinGame.KeyDown(KeyCode.RightShiftKey)) & SwinGame.KeyDown(KeyCode.CKey)) {
			UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, true);
		} else {
			UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, false);
		}

		UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
		UtilityFunctions.DrawMessage();

		SwinGame.DrawText(GameController.HumanPlayer.Shots.ToString(), Color.White, GameResources.GameFont("Menu"), _scoresLeft, _shotsTop);
		SwinGame.DrawText(GameController.HumanPlayer.Hits.ToString(), Color.White, GameResources.GameFont("Menu"), _scoresLeft, _hitsTop);
		SwinGame.DrawText(GameController.HumanPlayer.Missed.ToString(), Color.White, GameResources.GameFont("Menu"), _scoresLeft, _splashTop);
	}

}
