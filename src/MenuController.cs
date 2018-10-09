
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Diagnostics;
using SwinGameSDK;

/// <summary>
/// The menu controller handles the drawing and user interactions
/// from the menus in the game. These include the main menu, game
/// menu and the settings m,enu.
/// </summary>

static class MenuController
{
    /// <summary>
    /// The menu structure for the game.
    /// </summary>
    /// <remarks>
    /// These are the text captions for the menu items.
    /// </remarks>
    private static readonly string[][] _menuStructure =
    {
        new string[] {
            "PLAY",
            "SETUP",
            "SCORES",
            "QUIT"
        },
        new string[] {
            "RETURN",
            "SURRENDER",
            "QUIT"
        },
        new string[] {
            "EASY",
            "MEDIUM",
            "HARD"
        }
    };
	private const int _menuTop = 575;
	private const int _menuLeft = 30;
	private const int _menuGap = 0;
	private const int _buttonWidth = 75;
	private const int _buttonHeight = 15;
	private const int _buttonSeparation = _buttonWidth + _menuGap;

	private const int _TextOffset = 0;
	private const int _MainMenu = 0;
	private const int _GameMenu = 1;

	private const int _SetupMenu = 2;
	private const int _MainMenuPlayButton = 0;
	private const int _MainMenuSetupButton = 1;
	private const int _MainMenuTopScoresButton = 2;

	private const int _MainMenuQuitButton = 3;
	private const int _SetupMenuEasyButton = 0;
	private const int _SetupMenuMediumButton = 1;
	private const int _SetupMenuHardButton = 2;

	private const int _SetupMenuExitButton = 3;
	private const int _GameMenuReturnButton = 0;
	private const int _GameMenuSurrenderButton = 1;

	private const int _GameMenuQuitButton = 2;
	private static readonly Color _MenuColor = SwinGame.RGBAColor(2, 167, 252, 255);

	private static readonly Color _HighlightColor = SwinGame.RGBAColor(1, 57, 86, 255);
	/// <summary>
	/// Handles the processing of user input when the main menu is showing
	/// </summary>
	public static void HandleMainMenuInput()
	{
		HandleMenuInput(_MainMenu, 0, 0);
	}

	/// <summary>
	/// Handles the processing of user input when the main menu is showing
	/// </summary>
	public static void HandleSetupMenuInput()
	{
		bool handled = false;
		handled = HandleMenuInput(_SetupMenu, 1, 1);

		if (!handled)
        {
			HandleMenuInput(_MainMenu, 0, 0);
		}
	}

	/// <summary>
	/// Handle input in the game menu.
	/// </summary>
	/// <remarks>
	/// Player can return to the game, surrender, or quit entirely
	/// </remarks>
	public static void HandleGameMenuInput()
	{
		HandleMenuInput(_GameMenu, 0, 0);
	}

	/// <summary>
	/// Handles input for the specified menu.
	/// </summary>
	/// <param name="menu">the identifier of the menu being processed</param>
	/// <param name="level">the vertical level of the menu</param>
	/// <param name="xOffset">the xoffset of the menu</param>
	/// <returns>false if a clicked missed the buttons. This can be used to check prior menus.</returns>
	private static bool HandleMenuInput(int menu, int level, int xOffset)
	{
		if (SwinGame.KeyTyped(KeyCode.EscapeKey))
        {
			GameController.EndCurrentState();
			return true;
		}

		if (SwinGame.MouseClicked(MouseButton.LeftButton))
        {
			int i = 0;
			for (i = 0; i <= _menuStructure[menu].Length - 1; i++)
            {
				//IsMouseOver the i'th button of the menu
				if (IsMouseOverMenu(i, level, xOffset)) {
					PerformMenuAction(menu, i);
					return true;
				}
			}

			if (level > 0)
            {
				//none clicked - so end this sub menu
				GameController.EndCurrentState();
			}
		}

		return false;
	}

	/// <summary>
	/// Draws the main menu to the screen.
	/// </summary>
	public static void DrawMainMenu()
	{
		//Clears the Screen to Black
        //line bellow is only for dubugging
		//SwinGame.DrawText("Main Menu", Color.White, GameFont("ArialLarge"), 50, 50)

		DrawButtons(_MainMenu);
	}

	/// <summary>
	/// Draws the Game menu to the screen
	/// </summary>
	public static void DrawGameMenu()
	{
        //Clears the Screen to Black
        //line bellow is only for dubugging
        //SwinGame.DrawText("Paused", Color.White, GameFont("ArialLarge"), 50, 50)

        DrawButtons(_GameMenu);
	}

	/// <summary>
	/// Draws the settings menu to the screen.
	/// </summary>
	/// <remarks>
	/// Also shows the main menu
	/// </remarks>
	public static void DrawSettings()
	{
        //Clears the Screen to Black
        //line bellow is only for dubugging
        //SwinGame.DrawText("Settings", Color.White, GameFont("ArialLarge"), 50, 50)

        DrawButtons(_MainMenu);
		DrawButtons(_SetupMenu, 1, 1);
	}

	/// <summary>
	/// Draw the buttons associated with a top level menu.
	/// </summary>
	/// <param name="menu">the index of the menu to draw</param>
	private static void DrawButtons(int menu)
	{
		DrawButtons(menu, 0, 0);
	}

	/// <summary>
	/// Draws the menu at the indicated level.
	/// </summary>
	/// <param name="menu">the menu to draw</param>
	/// <param name="level">the level (height) of the menu</param>
	/// <param name="xOffset">the offset of the menu</param>
	/// <remarks>
	/// The menu text comes from the _menuStructure field. The level indicates the height
	/// of the menu, to enable sub menus. The xOffset repositions the menu horizontally
	/// to allow the submenus to be positioned correctly.
	/// </remarks>
	private static void DrawButtons(int menu, int level, int xOffset)
	{
		int buttonTop = 0;

		buttonTop = _menuTop - (_menuGap + _buttonHeight) * level;
		int i = 0;
		for (i = 0; i <= _menuStructure[menu].Length - 1; i++)
        {
			int buttonLeft = 0;
			buttonLeft = _menuLeft + _buttonSeparation * (i + xOffset);
            //The below commeted code makes the buttons look ugly
            //SwinGame.FillRectangle(Color.White, buttonLeft, buttonTop, _buttonWidth, _buttonHeight);
			SwinGame.DrawText(_menuStructure[menu][i], _MenuColor, Color.Black, GameResources.GameFont("Menu"), FontAlignment.AlignCenter, SwinGame.CreateRectangle(buttonLeft + _TextOffset, buttonTop + _TextOffset, _buttonWidth, _buttonHeight));
            
            if (SwinGame.MouseDown(MouseButton.LeftButton) & IsMouseOverMenu(i, level, xOffset))
            {
				SwinGame.DrawRectangle(_HighlightColor, buttonLeft, buttonTop, _buttonWidth, _buttonHeight);
			}
		}
	}
    

	/// <summary>
	/// Determined if the mouse is over one of the button in the main menu.
	/// </summary>
	/// <param name="button">the index of the button to check</param>
	/// <returns>true if the mouse is over that button</returns>
	private static bool IsMouseOverButton(int button)
	{
		return IsMouseOverMenu(button, 0, 0);
	}

	/// <summary>
	/// Checks if the mouse is over one of the buttons in a menu.
	/// </summary>
	/// <param name="button">the index of the button to check</param>
	/// <param name="level">the level of the menu</param>
	/// <param name="xOffset">the xOffset of the menu</param>
	/// <returns>true if the mouse is over the button</returns>
	private static bool IsMouseOverMenu(int button, int level, int xOffset)
	{
		int buttonTop = _menuTop - (_menuGap + _buttonHeight) * level;
		int buttonLeft = _menuLeft + _buttonSeparation * (button + xOffset);

		return UtilityFunctions.IsMouseInRectangle(buttonLeft, buttonTop, _buttonWidth, _buttonHeight);
	}

	/// <summary>
	/// A button has been clicked, perform the associated action.
	/// </summary>
	/// <param name="menu">the menu that has been clicked</param>
	/// <param name="button">the index of the button that was clicked</param>
	private static void PerformMenuAction(int menu, int button)
	{
		switch (menu)
        {
			case _MainMenu:
				PerformMainMenuAction(button);
				break;
			case _SetupMenu:
				PerformSetupMenuAction(button);
				break;
			case _GameMenu:
				PerformGameMenuAction(button);
				break;
		}
	}

	/// <summary>
	/// The main menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">the button pressed</param>
	private static void PerformMainMenuAction(int button)
	{
		switch (button)
        {
			case _MainMenuPlayButton:
				GameController.StartGame();
				break;
			case _MainMenuSetupButton:
				GameController.AddNewState(GameState.AlteringSettings);
				break;
			case _MainMenuTopScoresButton:
				GameController.AddNewState(GameState.ViewingHighScores);
				break;
			case _MainMenuQuitButton:
				GameController.EndCurrentState();
				break;
		}
	}

	/// <summary>
	/// The setup menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">the button pressed</param>
	private static void PerformSetupMenuAction(int button)
	{
		switch (button)
        {
			case _SetupMenuEasyButton:
				GameController.SetDifficulty(AIOption.Easy);
				break;
			case _SetupMenuMediumButton:
				GameController.SetDifficulty(AIOption.Medium);
				break;
			case _SetupMenuHardButton:
				GameController.SetDifficulty(AIOption.Hard);
				break;
		}
		//Always end state - handles exit button as well
		GameController.EndCurrentState();
	}

	/// <summary>
	/// The game menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">the button pressed</param>
	private static void PerformGameMenuAction(int button)
	{
		switch (button)
        {
			case _GameMenuReturnButton:
				GameController.EndCurrentState();
				break;
			case _GameMenuSurrenderButton:
				GameController.EndCurrentState();
				//end game menu
				GameController.EndCurrentState();
				//end game
				break;
			case _GameMenuQuitButton:
				GameController.AddNewState(GameState.Quitting);
				break;
		}
	}
}