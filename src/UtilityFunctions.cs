
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Diagnostics;
using SwinGameSDK;
/// <summary>
/// This includes a number of utility methods for
/// drawing and interacting with the Mouse.
/// </summary>
static class UtilityFunctions
{
	public const int _fieldTop = 122;
	public const int _fieldLeft = 349;
	public const int _fieldWidth = 418;

	public const int _fieldHeight = 418;

	public const int _messageTop = 548;
	public const int _cellWidth = 40;
	public const int _cellHeight = 40;

	public const int _cellGap = 2;

	public const int _shipGap = 3;


	private static readonly Color _smallSea = SwinGame.RGBAColor(6, 60, 94, 255);
	private static readonly Color _smallShip = Color.Gray;
	private static readonly Color _smallMiss = SwinGame.RGBAColor(1, 147, 220, 255);


	private static readonly Color _smallHit = SwinGame.RGBAColor(169, 24, 37, 255);
	private static readonly Color _largeSea = SwinGame.RGBAColor(6, 60, 94, 255);
	private static readonly Color _largeShip = Color.Gray;
	private static readonly Color _largeMiss = SwinGame.RGBAColor(1, 147, 220, 255);

	private static readonly Color _largeHit = SwinGame.RGBAColor(252, 2, 3, 255);
	private static readonly Color _outlineColour = SwinGame.RGBAColor(5, 55, 88, 255);
	private static readonly Color _shipFillColor = Color.Gray;
	private static readonly Color _shipOutlineColor = Color.White;

	private static readonly Color _messageColor = SwinGame.RGBAColor(2, 167, 252, 255);
	public const int _animationCells = 7;

	public const int _framesPerCell = 8;
	/// <summary>
	/// Determines if the mouse is in a given rectangle.
	/// </summary>
	/// <param name="x">the x location to check</param>
	/// <param name="y">the y location to check</param>
	/// <param name="w">the width to check</param>
	/// <param name="h">the height to check</param>
	/// <returns>true if the mouse is in the area checked</returns>
	public static bool IsMouseInRectangle(int x, int y, int w, int h)
	{
		Point2D mouse = default(Point2D);
		bool result = false;

		mouse = SwinGame.MousePosition();

		//if the mouse is inline with the button horizontally
		if (mouse.X >= x & mouse.X <= x + w)
        {
			//Check vertical position
			if (mouse.Y >= y & mouse.Y <= y + h)
            {
				result = true;
			}
		}

		return result;
	}

	/// <summary>
	/// Draws a large field using the grid and the indicated player's ships.
	/// </summary>
	/// <param name="grid">the grid to draw</param>
	/// <param name="thePlayer">the players ships to show</param>
	/// <param name="showShips">indicates if the ships should be shown</param>
	public static void DrawField(ISeaGrid grid, Player thePlayer, bool showShips)
	{
		DrawCustomField(grid, thePlayer, false, showShips, _fieldLeft, _fieldTop, _fieldWidth, _fieldHeight, _cellWidth, _cellHeight,
		_cellGap);
	}

	/// <summary>
	/// Draws a small field, showing the attacks made and the locations of the player's ships
	/// </summary>
	/// <param name="grid">the grid to show</param>
	/// <param name="thePlayer">the player to show the ships of</param>
	public static void DrawSmallField(ISeaGrid grid, Player thePlayer)
	{
		const int _smallFieldLeft = 39;
		const int _smallFieldTop = 373;
		const int _smallFieldWidth = 166;
		const int _smallFieldHeight = 166;
		const int _smallFieldCellWidth = 13;
		const int _smallFieldCellHeight = 13;
		const int _smallFieldCellGap = 4;

		DrawCustomField(grid, thePlayer, true, true, _smallFieldLeft, _smallFieldTop, _smallFieldWidth, _smallFieldHeight, _smallFieldCellWidth, _smallFieldCellHeight,
		_smallFieldCellGap);
	}

	/// <summary>
	/// Draws the player's grid and ships.
	/// </summary>
	/// <param name="grid">the grid to show</param>
	/// <param name="thePlayer">the player to show the ships of</param>
	/// <param name="small">true if the small grid is shown</param>
	/// <param name="showShips">true if ships are to be shown</param>
	/// <param name="left">the left side of the grid</param>
	/// <param name="top">the top of the grid</param>
	/// <param name="width">the width of the grid</param>
	/// <param name="height">the height of the grid</param>
	/// <param name="cellWidth">the width of each cell</param>
	/// <param name="cellHeight">the height of each cell</param>
	/// <param name="cellGap">the gap between the cells</param>
	private static void DrawCustomField(ISeaGrid grid, Player thePlayer, bool small, bool showShips, int left, int top, int width, int height, int cellWidth, int cellHeight,
	int cellGap)
	{
        //line bellow is only for debugging
        //SwinGame.FillRectangle(Color.Blue, left, top, width, height)

        int rowTop = 0;
		int colLeft = 0;

		//Draw the grid
		for (int row = 0; row <= 9; row++)
        {
			rowTop = top + (cellGap + cellHeight) * row;

			for (int col = 0; col <= 9; col++)
            {
				colLeft = left + (cellGap + cellWidth) * col;

				Color fillColor = default(Color);
				bool draw = false;

				draw = true;

				switch (grid[row, col])
                {
                    //case TileView.Ship:
                    //	draw = false;
                    //	break;
                    //uncommenting line bellow causes an error
                    //If small Then fillColor = _SMALL_SHIP Else fillColor = _LARGE_SHIP
                    case TileView.Miss:
						if (small)
							fillColor = _smallMiss;
						else
							fillColor = _largeMiss;
						break;
					case TileView.Hit:
						if (small)
							fillColor = _smallHit;
						else
							fillColor = _largeHit;
						break;
					case TileView.Sea:
					case TileView.Ship:
						if (small)
							fillColor = _smallSea;
						else
							draw = false;
						break;
				}

				if (draw)
                {
					SwinGame.FillRectangle(fillColor, colLeft, rowTop, cellWidth, cellHeight);
					if (!small)
                    {
						SwinGame.DrawRectangle(_outlineColour, colLeft, rowTop, cellWidth, cellHeight);
					}
				}
			}
		}

		if (!showShips)
        {
			return;
		}

		int shipHeight = 0;
		int shipWidth = 0;
		string shipName = null;

		//Draw the ships
		foreach (Ship s in thePlayer)
        {
			if (s == null || !s.IsDeployed)
				continue;
			rowTop = top + (cellGap + cellHeight) * s.Row + _shipGap;
			colLeft = left + (cellGap + cellWidth) * s.Column + _shipGap;

            if (s.Direction == Direction.LeftRight)
            {
                shipName = "ShipLR" + s.Size;
                shipHeight = cellHeight - (_shipGap * 2);
                shipWidth = (cellWidth + cellGap) * s.Size - (_shipGap * 2) - cellGap;

            }
            else
            {
                //Up down
                shipName = "ShipUD" + s.Size;
                shipHeight = (cellHeight + cellGap) * s.Size - (_shipGap * 2) - cellGap;
                shipWidth = cellWidth - (_shipGap * 2);
            }

                if (!small)
                {
                    SwinGame.DrawBitmap(GameResources.GameImage(shipName), colLeft, rowTop);
                }
                else
                {
                    SwinGame.FillRectangle(_shipFillColor, colLeft, rowTop, shipWidth, shipHeight);
                    SwinGame.DrawRectangle(_shipOutlineColor, colLeft, rowTop, shipWidth, shipHeight);
                }
            
		}
	}

	private static string _message;
	/// <summary>
	/// The message to display
	/// </summary>
	/// <value>The message to display</value>
	/// <returns>The message to display</returns>
	public static string Message
    {
		get { return _message; }
		set { _message = value; }
	}

	/// <summary>
	/// Draws the message to the screen
	/// </summary>
	public static void DrawMessage()
	{
		SwinGame.DrawText(Message, _messageColor, GameResources.GameFont("Courier"), _fieldLeft, _messageTop);
	}

	/// <summary>
	/// Draws the background for the current state of the game
	/// </summary>

	public static void DrawBackground()
	{
		switch (GameController.CurrentState)
        {
			case GameState.ViewingMainMenu:
			case GameState.ViewingGameMenu:
			case GameState.AlteringSettings:
			case GameState.ViewingHighScores:
				SwinGame.DrawBitmap(GameResources.GameImage("Menu"), 0, 0);
				break;
			case GameState.Discovering:
			case GameState.EndingGame:
				SwinGame.DrawBitmap(GameResources.GameImage("Discovery"), 0, 0);
				break;
			case GameState.Deploying:
				SwinGame.DrawBitmap(GameResources.GameImage("Deploy"), 0, 0);
				break;
			default:
				SwinGame.ClearScreen();
				break;
		}

		SwinGame.DrawFramerate(675, 585 /*, GameResources.GameFont("CourierSmall")*/);
	}

	public static void AddExplosion(int row, int col)
	{
		AddAnimation(row, col, "Splash");
	}

	public static void AddSplash(int row, int col)
	{
		AddAnimation(row, col, "Splash");
	}

	private static List<Sprite> _Animations = new List<Sprite>();
	private static void AddAnimation(int row, int col, string image)
	{
		Sprite s = default(Sprite);
		Bitmap imgObj = default(Bitmap);

		imgObj = GameResources.GameImage(image);
		imgObj.SetCellDetails(40, 40, 3, 3, 7);

		AnimationScript animation = default(AnimationScript);
		animation = SwinGame.LoadAnimationScript("splash.txt");

		s = SwinGame.CreateSprite(imgObj, animation);
		s.X = _fieldLeft + col * (_cellWidth + _cellGap);
		s.Y = _fieldTop + row * (_cellHeight + _cellGap);

		s.StartAnimation("splash");
		_Animations.Add(s);
	}

	public static void UpdateAnimations()
	{
		List<Sprite> ended = new List<Sprite>();
		foreach (Sprite s in _Animations)
        {
			SwinGame.UpdateSprite(s);
			if (s.AnimationHasEnded)
            {
				ended.Add(s);
			}
		}

		foreach (Sprite s in ended)
        {
			_Animations.Remove(s);
			SwinGame.FreeSprite(s);
		}
	}

	public static void DrawAnimations()
	{
		foreach (Sprite s in _Animations)
        {
			SwinGame.DrawSprite(s);
		}
	}

	public static void DrawAnimationSequence()
	{
		int i = 0;
		for (i = 1; i <= _animationCells * _framesPerCell; i++)
        {
			UpdateAnimations();
			GameController.DrawScreen();
		}
	}
}