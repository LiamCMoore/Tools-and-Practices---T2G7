
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Diagnostics;
using SwinGameSDK;

/// <summary>
/// The DeploymentController controls the players actions
/// during the deployment phase.
/// </summary>
static class DeploymentController
{
	private const int _shipsTop = 98;
	private const int _shipsLeft = 20;
	private const int _shipsHeight = 90;

	private const int _shipsWidth = 300;
	private const int _topButtonsTop = 72;

	private const int _topButtonsHeight = 46;
	private const int _playButtonLeft = 693;

	private const int _playButtonWidth = 80;
	private const int _upDownButtonLeft = 410;

	private const int _leftRightButtonLeft = 350;
	private const int _randomButtonLeft = 547;

	private const int _randomButtonWidth = 51;

	private const int _dirButtonsWidth = 47;

	private const int _textOffSet = 5;
	private static Direction _currentDirection = Direction.UpDown;

	private static ShipName _selectedShip = ShipName.Tug;
	/// <summary>
	/// Handles user input for the Deployment phase of the game.
	/// </summary>
	/// <remarks>
	/// Involves selecting the ships, deloying ships, changing the direction
	/// of the ships to add, randomising deployment, end then ending
	/// deployment
	/// </remarks>
	public static void HandleDeploymentInput()
	{
		if (SwinGame.KeyTyped(KeyCode.EscapeKey)) {
			GameController.AddNewState(GameState.ViewingGameMenu);
		}

		if (SwinGame.KeyTyped(KeyCode.UpKey) | SwinGame.KeyTyped(KeyCode.DownKey)) {
			_currentDirection = Direction.UpDown; 
		}
		if (SwinGame.KeyTyped(KeyCode.LeftKey) | SwinGame.KeyTyped(KeyCode.RightKey)) {
			_currentDirection = Direction.LeftRight;
		}

		if (SwinGame.KeyTyped(KeyCode.RKey)) {
			GameController.HumanPlayer.RandomizeDeployment();
		}

		if (SwinGame.MouseClicked(MouseButton.LeftButton)) {
			ShipName selected = default(ShipName);
			selected = GetShipMouseIsOver();
			if (selected != ShipName.None) {
				_selectedShip = selected;
			} else {
				DoDeployClick();
			}

			if (GameController.HumanPlayer.ReadyToDeploy & UtilityFunctions.IsMouseInRectangle(_playButtonLeft, _topButtonsTop, _playButtonWidth, _topButtonsHeight)) {
				GameController.EndDeployment();
			} else if (UtilityFunctions.IsMouseInRectangle(_upDownButtonLeft, _topButtonsTop, _dirButtonsWidth, _topButtonsHeight)) {
				_currentDirection = Direction.UpDown;
			} else if (UtilityFunctions.IsMouseInRectangle(_leftRightButtonLeft, _topButtonsTop, _dirButtonsWidth, _topButtonsHeight)) {
				_currentDirection = Direction.LeftRight;
			} else if (UtilityFunctions.IsMouseInRectangle(_randomButtonLeft, _topButtonsTop, _randomButtonWidth, _topButtonsHeight)) {
				GameController.HumanPlayer.RandomizeDeployment();
			}
		}
	}

	/// <summary>
	/// The user has clicked somewhere on the screen, check if its is a deployment and deploy
	/// the current ship if that is the case.
	/// </summary>
	/// <remarks>
	/// If the click is in the grid it deploys to the selected location
	/// with the indicated direction
	/// </remarks>
	private static void DoDeployClick()
	{
		var mouse = default(Point2D);

		mouse = SwinGame.MousePosition();

		//Calculate the row/col clicked
		int row = 0;
		int col = 0;
		row = Convert.ToInt32(Math.Floor((mouse.Y - UtilityFunctions._fieldTop) / (UtilityFunctions._cellHeight + UtilityFunctions._cellGap)));
		col = Convert.ToInt32(Math.Floor((mouse.X - UtilityFunctions._fieldLeft) / (UtilityFunctions._cellWidth + UtilityFunctions._cellGap)));

		if (row >= 0 & row < GameController.HumanPlayer.PlayerGrid.Height) {
			if (col >= 0 & col < GameController.HumanPlayer.PlayerGrid.Width) {
				//if in the area try to deploy
				try {
					GameController.HumanPlayer.PlayerGrid.MoveShip(row, col, _selectedShip, _currentDirection);
                    UtilityFunctions.Message = "You have placed a ship on the grid";
                } catch (Exception ex) {
					Audio.PlaySoundEffect(GameResources.GameSound("Error"));
					UtilityFunctions.Message = ex.Message;
				}
			}
		}
	}

	/// <summary>
	/// Draws the deployment screen showing the field and the ships
	/// that the player can deploy.
	/// </summary>
	public static void DrawDeployment()
	{
		UtilityFunctions.DrawField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer, true);

		//Draw the Left/Right and Up/Down buttons
		if (_currentDirection == Direction.LeftRight) {
			SwinGame.DrawBitmap(GameResources.GameImage("LeftRightButton"), _leftRightButtonLeft, _topButtonsTop);
            //two lines bellow are only for debugging
            //SwinGame.DrawText("U/D", Color.Gray, GameFont("Menu"), UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP)
            //SwinGame.DrawText("L/R", Color.White, GameFont("Menu"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP)
        }
        else {
			SwinGame.DrawBitmap(GameResources.GameImage("UpDownButton"), _leftRightButtonLeft, _topButtonsTop);
            //two lines bellow are only for debugging
            //SwinGame.DrawText("U/D", Color.White, GameFont("Menu"), UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP)
            //SwinGame.DrawText("L/R", Color.Gray, GameFont("Menu"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP)
        }

        //DrawShips
        foreach (ShipName sn in Enum.GetValues(typeof(ShipName))) {
			int i = 0;
			i = ((int)sn) - 1;
			if (i >= 0) {
				if (sn == _selectedShip) {
					SwinGame.DrawBitmap(GameResources.GameImage("SelectedShip"), _shipsLeft, _shipsTop + i * _shipsHeight);
                    //three lines bellow are only for debugging
                    //    SwinGame.FillRectangle(Color.LightBlue, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                    //Else
                    //    SwinGame.FillRectangle(Color.Gray, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                }

                //uncommenting two lines bellow does not caused an error
                //SwinGame.DrawRectangle(Color.Black, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                //SwinGame.DrawText(sn.ToString(), Color.Black, GameFont("Courier"), SHIPS_LEFT + TEXT_OFFSET, SHIPS_TOP + i * SHIPS_HEIGHT)

            }
        }

		if (GameController.HumanPlayer.ReadyToDeploy) {
			SwinGame.DrawBitmap(GameResources.GameImage("PlayButton"), _playButtonLeft, _topButtonsTop);
            //uncommenting two lines bellow caused an error
            //SwinGame.FillRectangle(Color.LightBlue, PLAY_BUTTON_LEFT, PLAY_BUTTON_TOP, PLAY__buttonWidth, PLAY_BUTTON_HEIGHT)
            //SwinGame.DrawText("PLAY", Color.Black, GameFont("Courier"), PLAY_BUTTON_LEFT + TEXT_OFFSET, PLAY_BUTTON_TOP)
        }

		SwinGame.DrawBitmap(GameResources.GameImage("RandomButton"), _randomButtonLeft, _topButtonsTop);

		UtilityFunctions.DrawMessage();
	}

	/// <summary>
	/// Gets the ship that the mouse is currently over in the selection panel.
	/// </summary>
	/// <returns>The ship selected or none</returns>
	private static ShipName GetShipMouseIsOver()
	{
		foreach (ShipName sn in Enum.GetValues(typeof(ShipName))) {
			int i = 0;
			i =((int)sn) - 1;

			if (UtilityFunctions.IsMouseInRectangle(_shipsLeft, _shipsTop + i * _shipsHeight, _shipsWidth, _shipsHeight)) {
				return sn;
			}
		}

		return ShipName.None;
	}
}
