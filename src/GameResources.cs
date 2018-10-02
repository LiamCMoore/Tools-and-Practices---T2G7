
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Diagnostics;
using SwinGameSDK;

public static class GameResources
{
    private static Dictionary<string, Bitmap> _images = new Dictionary<string, Bitmap>();
    private static Dictionary<string, Font> _fonts = new Dictionary<string, Font>();
    private static Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();

    private static Dictionary<string, Music> _music = new Dictionary<string, Music>();
    private static Bitmap _background;
    private static Bitmap _animation;
    private static Bitmap _loaderFull;
    private static Bitmap _loaderEmpty;
    private static Font _loadingFont;

    private static SoundEffect _startSound;

    private static void LoadFonts()
	{
		NewFont("ArialLarge", "arial.ttf", 80);
		NewFont("Courier", "cour.ttf", 14);
		NewFont("CourierSmall", "cour.ttf", 8);
		NewFont("Menu", "ffaccess.ttf", 8);
	}

	private static void LoadImages()
	{
		//Backgrounds
		NewImage("Menu", "main_page.jpg");
		NewImage("Discovery", "discover.jpg");
		NewImage("Deploy", "deploy.jpg");

		//Deployment
		NewImage("LeftRightButton", "deploy_dir_button_horiz.png");
		NewImage("UpDownButton", "deploy_dir_button_vert.png");
		NewImage("SelectedShip", "deploy_button_hl.png");
		NewImage("PlayButton", "deploy_play_button.png");
		NewImage("RandomButton", "deploy_randomize_button.png");

		//Ships
		int i = 0;
		for (i = 1; i <= 5; i++) {
			NewImage("ShipLR" + i, "ship_deploy_horiz_" + i + ".png");
			NewImage("ShipUD" + i, "ship_deploy_vert_" + i + ".png");
		}

		//Explosions
		NewImage("Explosion", "explosion.png");
		NewImage("Splash", "splash.png");

	}

	private static void LoadSounds()
	{
		NewSound("Error", "error.wav");
		NewSound("Hit", "hit.wav");
		NewSound("Sink", "sink.wav");
		NewSound("Siren", "siren.wav");
		NewSound("Miss", "watershot.wav");
		NewSound("Winner", "winner.wav");
		NewSound("Lose", "lose.wav");
	}

	private static void LoadMusic()
	{
		NewMusic("Background", "horrordrone.mp3");
	}

	/// <summary>
	/// Gets a Font Loaded in the Resources
	/// </summary>
	/// <param name="font">Name of Font</param>
	/// <returns>The Font Loaded with this Name</returns>

	public static Font GameFont(string font)
	{
		return _fonts[font];
	}

	/// <summary>
	/// Gets an Image loaded in the Resources
	/// </summary>
	/// <param name="image">Name of image</param>
	/// <returns>The image loaded with this name</returns>

	public static Bitmap GameImage(string image)
	{
		return _images[image];
	}

	/// <summary>
	/// Gets an sound loaded in the Resources
	/// </summary>
	/// <param name="sound">Name of sound</param>
	/// <returns>The sound with this name</returns>

	public static SoundEffect GameSound(string sound)
	{
		return _sounds[sound];
	}

	/// <summary>
	/// Gets the music loaded in the Resources
	/// </summary>
	/// <param name="music">Name of music</param>
	/// <returns>The music with this name</returns>

	public static Music GameMusic(string music)
	{
		return _music[music];
	}

	/// <summary>
	/// The Resources Class stores all of the Games Media Resources, such as Images, Fonts
	/// Sounds, Music.
	/// </summary>

	public static void LoadResources()
	{
		int width = 0;
		int height = 0;

		width = SwinGame.ScreenWidth();
		height = SwinGame.ScreenHeight();

		SwinGame.ChangeScreenSize(800, 600);

		ShowLoadingScreen();

		ShowMessage("Loading fonts...", 0);
		LoadFonts();
		SwinGame.Delay(100);

		ShowMessage("Loading images...", 1);
		LoadImages();
		SwinGame.Delay(100);

		ShowMessage("Loading sounds...", 2);
		LoadSounds();
		SwinGame.Delay(100);

		ShowMessage("Loading music...", 3);
		LoadMusic();
		SwinGame.Delay(100);

		SwinGame.Delay(100);
		ShowMessage("Game loaded...", 5);
		SwinGame.Delay(100);
		EndLoadingScreen(width, height);
	}

	private static void ShowLoadingScreen()
	{
		_background = SwinGame.LoadBitmap(SwinGame.PathToResource("SplashBack.png", ResourceKind.BitmapResource));
		SwinGame.DrawBitmap(_background, 0, 0);
		SwinGame.RefreshScreen();
		SwinGame.ProcessEvents();

		_animation = SwinGame.LoadBitmap(SwinGame.PathToResource("SwinGameAni.jpg", ResourceKind.BitmapResource));
		_loadingFont = SwinGame.LoadFont(SwinGame.PathToResource("arial.ttf", ResourceKind.FontResource), 12);
		_startSound = Audio.LoadSoundEffect(SwinGame.PathToResource("SwinGameStart.ogg", ResourceKind.SoundResource));

		_loaderFull = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_full.png", ResourceKind.BitmapResource));
		_loaderEmpty = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_empty.png", ResourceKind.BitmapResource));

		PlaySwinGameIntro();
	}

	private static void PlaySwinGameIntro()
	{
		//const int ANI_X = 143;
		//const int ANI_Y = 134;
		const int _aniW = 546;
		const int _aniH = 327;
		const int _aniVCellCount = 6;
		const int _aniCellCount = 11;

		Audio.PlaySoundEffect(_startSound);
		SwinGame.Delay(200);

		int i = 0;
		for (i = 0; i <= _aniCellCount - 1; i++) {
			SwinGame.DrawBitmap(_background, 0, 0);
		    SwinGame.DrawBitmap(_animation, (i / _aniVCellCount) * _aniW, (i % (_aniVCellCount)) * _aniH);
            SwinGame.Delay(20);
			SwinGame.RefreshScreen();
			SwinGame.ProcessEvents();
		}

		SwinGame.Delay(1500);

	}

	private static void ShowMessage(string message, int number)
	{
		const int _tx = 310;
		const int _ty = 493;
		const int _tw = 200;
		const int _th = 25;
		const int _steps = 5;
		const int _bgX = 279;
		const int _bgY = 453;

		int _fullW = 0;

		_fullW = 260 * number / _steps;
		SwinGame.DrawBitmap(_loaderEmpty, _bgX, _bgY);
		SwinGame.DrawBitmap(_loaderFull, 0, 0);

		SwinGame.DrawText(message, Color.White, Color.Transparent, _loadingFont, FontAlignment.AlignCenter, SwinGame.CreateRectangle(_tx, _ty, _tw, _th));


		SwinGame.RefreshScreen();
		SwinGame.ProcessEvents();
	}

	private static void EndLoadingScreen(int width, int height)
	{
		SwinGame.ProcessEvents();
		SwinGame.Delay(500);
		SwinGame.ClearScreen();
		SwinGame.RefreshScreen();
		SwinGame.FreeFont(_loadingFont);
		SwinGame.FreeBitmap(_background);
		SwinGame.FreeBitmap(_animation);
		SwinGame.FreeBitmap(_loaderEmpty);
		SwinGame.FreeBitmap(_loaderFull);
		//Audio.FreeSoundEffect(_StartSound);
		SwinGame.ChangeScreenSize(width, height);
	}

	private static void NewFont(string fontName, string fileName, int size)
	{
		_fonts.Add(fontName, SwinGame.LoadFont(SwinGame.PathToResource(fileName, ResourceKind.FontResource), size));
	}

	private static void NewImage(string imageName, string fileName)
	{
		_images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(fileName, ResourceKind.BitmapResource)));
	}

	private static void NewTransparentColorImage(string imageName, string fileName, Color transColor)
	{
		_images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(fileName, ResourceKind.BitmapResource) /*true, transColor*/ ));
	}

	private static void NewTransparentColourImage(string imageName, string fileName, Color transColor)
	{
		NewTransparentColorImage(imageName, fileName, transColor);
	}

	private static void NewSound(string soundName, string fileName)
	{
		_sounds.Add(soundName, Audio.LoadSoundEffect(SwinGame.PathToResource(fileName, ResourceKind.SoundResource)));
	}

	private static void NewMusic(string musicName, string fileName)
	{
		_music.Add(musicName, Audio.LoadMusic(SwinGame.PathToResource(fileName, ResourceKind.SoundResource)));
	}

	private static void FreeFonts()
	{
		foreach (Font obj in _fonts.Values) {
			SwinGame.FreeFont(obj);
		}
	}

	private static void FreeImages()
	{
		foreach (Bitmap obj in _images.Values) {
			SwinGame.FreeBitmap(obj);
		}
	}

    
	private static void FreeSounds()
	{		
		foreach (SoundEffect obj in _sounds.Values) {
			//Audio.FreeSoundEffect(obj);
		}
	}
    
	private static void FreeMusic()
	{

		foreach (Music obj in _music.Values) {
			Audio.FreeMusic(obj);
		}
	}

	public static void FreeResources()
	{
		FreeFonts();
		FreeImages();
		FreeMusic();
		FreeSounds();
		SwinGame.ProcessEvents();
	}
}