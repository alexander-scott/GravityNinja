using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;
using System.Xml.Serialization;
using System.Xml;
	
namespace GravityDuck
{
	public class AppMain
	{
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
				
		//------ UI ------\\
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label				scoreLabel;
		private static Sce.PlayStation.HighLevel.UI.Label[]				highscoreLabel;
		private static Sce.PlayStation.HighLevel.UI.Label				levelScore;
		private static Sce.PlayStation.HighLevel.UI.Label				timerLabel;
		private static Sce.PlayStation.HighLevel.UI.Label				levelTimer;
		private static Sce.PlayStation.HighLevel.UI.ImageBox			scoreHUD;
		private static Sce.PlayStation.HighLevel.UI.ImageBox			timerHUD;
		
		
		//------ Classes ------\\
		private static Background background;
		private static Maze maze;
		private static Player player;
		private static TitleScreen title;
		private static LevelComplete levelComplete;
		private static LoadingScreen loadingScreen;
		private static GameOverScreen gameOverScreen;
		private static LevelSelectScreen levelSelectScreen;
				
		//------ HUD ------\\
		private static Timer timer;
		private static int time;
		private static int currentTime;
		private static int score;
		private static SpriteUV	gravityArrow;
		private static SpriteUV highscoreTab;
		private static int overallLevelScore;
		
		//------ I/O ------\\
		private static string SAVE_DATA;
		private static bool	doesDataFileExist = false;
		
		//------ Player Movement ------\\
		private static Vector2 gravityVector = new Vector2(0.0f, -1.0f); //The direction in which gravity is currently going
		private static Vector2 playerDirection; //Rotation of the player sprite
		private static Vector2 movementVector = new Vector2(0.0f, 0.0f); //Direction player is moving when not falling
		private static Vector2 keyboardVector = new Vector2(0.0f, 0.0f); //Keyboard input
		private static bool invert = false; //To switch between the Y and X axis movement
		private static bool falling = true; //If the player isn't touching a tile then he's falling
		private static Bounds2 playerBox; //Non-rotatable bounds that encompass the player
		public static int currGrav = 0; //ID for the 4 types of camera rotation
		public static Vector2 additionalForces = new Vector2(0.0f, 0.0f); // External forces from other entities (Obstacles)
		
		//------ Touch Data ------\\
		private static Vector2 oldTouchPos = new Vector2( 0.0f, 0.0f ); // Position of first touch on screen
		private static Vector2 newTouchPos = new Vector2( 0.0f, 0.0f ); // Position of last touch on screen
		
		//------ Camera Data ------\\
		private static float cameraRotation = FMath.PI/2.0f; // The rotation of the camera as a angle, as well as other entities
		private static float zoom = 0.5f; // How much of the game can be viewed
		private static bool rotating = false; //Camera rotations bools
		private static bool sideRotation = false;
		private static bool rightRotation = false;
		private static float endRotation;
		public static float lastTime = 0.0f;
		public static bool zoomedIn = false;
		public static bool rotationNotAllowed = false;
		
		//------ Menu Data ------\\
		private static bool play = false;
		private static bool pause = false;
		private static bool loadedTextures = false;
		private static int timeStamp1;
		private enum States {TITLE, LEVELSELECT, LOADING, LOADED, PLAYING, LEVELCOMPLETE};
		private static States currentState;
		public static TextureInfo loadingTexture0, loadingTexture1, loadingTexture2, loadingTexture3, loadingTexture4, loadingTexture5, loadingTexture6, loadingTexture7, loadingTexture8, loadingTexture9, loadingTexture10, loadingTexture11, loadingTexture12, loadingTexture13, loadingTexture14, loadingTexture15, loadingTexture16, loadingTexture17, loadingTexture18, loadingTexture19, loadingTexture20, loadingTexture21, loadingTexture22, loadingTexture23, loadingTexture24, loadingTexture25, loadingTexture26;
		
		//------ Level Data ------\\
		private static int currentLevel = 0; //The highest level we have unlocked, Read this in from file eventually (local highscores)
		private static int highestUnlockedLevel = 0;
		private static int totalNumOfLevels = 27;
		private static List<List<Highscore>> loadedLevelHighscores;
		private static Highscore currentScore;
		
		public static void Main (string[] args)
		{
			Initialize();
			
			//Game loop
			bool quitGame = false;
			while (!quitGame) 
			{ 
				SystemEvents.CheckEvents ();				
				
				Update ();
				
				Director.Instance.Update();
				Director.Instance.Render();
				UISystem.Render();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
			}
	
			Director.Terminate ();
		}
		
		private static void LoadLoadingTextures(bool loadOrDispose, int levelToLoad)
		{
			if(loadOrDispose)
			{
				if (levelToLoad == 0)
					loadingTexture0 	= new TextureInfo("/Application/textures/LoadingScreens/Level0Load.png");
				else if (levelToLoad ==1)
					loadingTexture1 	= new TextureInfo("/Application/textures/LoadingScreens/Level1Load.png");
				else if (levelToLoad ==2)
					loadingTexture2 	= new TextureInfo("/Application/textures/LoadingScreens/Level2Load.png");
				else if (levelToLoad ==3)
					loadingTexture3 	= new TextureInfo("/Application/textures/LoadingScreens/Level3Load.png");
				else if (levelToLoad ==4)
					loadingTexture4 	= new TextureInfo("/Application/textures/LoadingScreens/Level4Load.png");
				else if (levelToLoad ==5)
					loadingTexture5 	= new TextureInfo("/Application/textures/LoadingScreens/Level5Load.png");
				else if (levelToLoad ==6)
					loadingTexture6 	= new TextureInfo("/Application/textures/LoadingScreens/Level6Load.png");
				else if (levelToLoad ==7)
					loadingTexture7 	= new TextureInfo("/Application/textures/LoadingScreens/Level7Load.png");
				else if (levelToLoad ==8)
					loadingTexture8 	= new TextureInfo("/Application/textures/LoadingScreens/Level8Load.png");
				else if (levelToLoad ==9)
					loadingTexture9 	= new TextureInfo("/Application/textures/LoadingScreens/Level9Load.png");
				else if (levelToLoad ==10)
					loadingTexture10 	= new TextureInfo("/Application/textures/LoadingScreens/Level10Load.png");
				else if (levelToLoad ==11)
					loadingTexture11 	= new TextureInfo("/Application/textures/LoadingScreens/Level11Load.png");
				else if (levelToLoad ==12)
					loadingTexture12 	= new TextureInfo("/Application/textures/LoadingScreens/Level12Load.png");
				else if (levelToLoad ==13)
					loadingTexture13	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
				else if (levelToLoad ==14)
					loadingTexture14	= new TextureInfo("/Application/textures/LoadingScreens/Level14Load.png");
				else if (levelToLoad ==15)
					loadingTexture15	= new TextureInfo("/Application/textures/LoadingScreens/Level15Load.png");
				else if (levelToLoad ==16)
					loadingTexture16	= new TextureInfo("/Application/textures/LoadingScreens/Level16Load.png");
				else if (levelToLoad ==17)
					loadingTexture17	= new TextureInfo("/Application/textures/LoadingScreens/Level17Load.png");
				else if (levelToLoad ==18)
					loadingTexture18	= new TextureInfo("/Application/textures/LoadingScreens/Level18Load.png");
				else if (levelToLoad ==19)
					loadingTexture19	= new TextureInfo("/Application/textures/LoadingScreens/Level19Load.png");
				else if (levelToLoad ==20)
					loadingTexture20	= new TextureInfo("/Application/textures/LoadingScreens/Level20Load.png");
				else if (levelToLoad ==21)
					loadingTexture21	= new TextureInfo("/Application/textures/LoadingScreens/Level21Load.png");
				else if (levelToLoad ==22)
					loadingTexture22	= new TextureInfo("/Application/textures/LoadingScreens/Level22Load.png");
				else if (levelToLoad ==23)
					loadingTexture23	= new TextureInfo("/Application/textures/LoadingScreens/Level23Load.png");
				else if (levelToLoad ==24)
					loadingTexture24	= new TextureInfo("/Application/textures/LoadingScreens/Level24Load.png");
				else if (levelToLoad ==25)
					loadingTexture25	= new TextureInfo("/Application/textures/LoadingScreens/Level25Load.png");
				else if (levelToLoad ==26)
					loadingTexture26	= new TextureInfo("/Application/textures/LoadingScreens/Level26Load.png");
				else
				{
					if (highestUnlockedLevel >= 0)
						loadingTexture0 	= new TextureInfo("/Application/textures/LoadingScreens/Level0Load.png");
					if (highestUnlockedLevel >= 1)
						loadingTexture1 	= new TextureInfo("/Application/textures/LoadingScreens/Level1Load.png");
					if (highestUnlockedLevel >= 2)
						loadingTexture2 	= new TextureInfo("/Application/textures/LoadingScreens/Level2Load.png");
					if (highestUnlockedLevel >= 3)
						loadingTexture3 	= new TextureInfo("/Application/textures/LoadingScreens/Level3Load.png");
					if (highestUnlockedLevel >= 4)
						loadingTexture4 	= new TextureInfo("/Application/textures/LoadingScreens/Level4Load.png");
					if (highestUnlockedLevel >= 5)
						loadingTexture5 	= new TextureInfo("/Application/textures/LoadingScreens/Level5Load.png");
					if (highestUnlockedLevel >= 6)
						loadingTexture6 	= new TextureInfo("/Application/textures/LoadingScreens/Level6Load.png");
					if (highestUnlockedLevel >= 7)
						loadingTexture7 	= new TextureInfo("/Application/textures/LoadingScreens/Level7Load.png");
					if (highestUnlockedLevel >= 8)
						loadingTexture8 	= new TextureInfo("/Application/textures/LoadingScreens/Level8Load.png");
					if (highestUnlockedLevel >= 9)
						loadingTexture9 	= new TextureInfo("/Application/textures/LoadingScreens/Level9Load.png");
					if (highestUnlockedLevel >= 10)
						loadingTexture10 	= new TextureInfo("/Application/textures/LoadingScreens/Level10Load.png");
					if (highestUnlockedLevel >= 11)
						loadingTexture11 	= new TextureInfo("/Application/textures/LoadingScreens/Level11Load.png");
					if (highestUnlockedLevel >= 12)
						loadingTexture12 	= new TextureInfo("/Application/textures/LoadingScreens/Level12Load.png");
					if (highestUnlockedLevel >= 13)	
						loadingTexture13	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
					if (highestUnlockedLevel >= 14)
						loadingTexture14	= new TextureInfo("/Application/textures/LoadingScreens/Level14Load.png");
					if (highestUnlockedLevel >= 15)
						loadingTexture15	= new TextureInfo("/Application/textures/LoadingScreens/Level15Load.png");
					if (highestUnlockedLevel >= 16)
						loadingTexture16	= new TextureInfo("/Application/textures/LoadingScreens/Level16Load.png");
					if (highestUnlockedLevel >= 17)
						loadingTexture17	= new TextureInfo("/Application/textures/LoadingScreens/Level17Load.png");
					if (highestUnlockedLevel >= 18)
						loadingTexture18	= new TextureInfo("/Application/textures/LoadingScreens/Level18Load.png");
					if (highestUnlockedLevel >= 19)
						loadingTexture19	= new TextureInfo("/Application/textures/LoadingScreens/Level19Load.png");
					if (highestUnlockedLevel >= 20)
						loadingTexture20	= new TextureInfo("/Application/textures/LoadingScreens/Level20Load.png");
					if (highestUnlockedLevel >= 21)
						loadingTexture21	= new TextureInfo("/Application/textures/LoadingScreens/Level21Load.png");
					if (highestUnlockedLevel >= 22)
						loadingTexture22	= new TextureInfo("/Application/textures/LoadingScreens/Level22Load.png");
					if (highestUnlockedLevel >= 23)
						loadingTexture23	= new TextureInfo("/Application/textures/LoadingScreens/Level23Load.png");
					if (highestUnlockedLevel >= 24)
						loadingTexture24	= new TextureInfo("/Application/textures/LoadingScreens/Level24Load.png");
					if (highestUnlockedLevel >= 25)
						loadingTexture25	= new TextureInfo("/Application/textures/LoadingScreens/Level25Load.png");
					if (highestUnlockedLevel >= 26)
						loadingTexture26	= new TextureInfo("/Application/textures/LoadingScreens/Level26Load.png");
						loadedTextures = true;
				}
			}
			else
			{
				if (highestUnlockedLevel >= 0)
					loadingTexture0.Dispose();
				if (highestUnlockedLevel >= 1)
					loadingTexture1.Dispose();
				if (highestUnlockedLevel >= 2)
					loadingTexture2.Dispose();
				if (highestUnlockedLevel >= 3)
					loadingTexture3.Dispose();
				if (highestUnlockedLevel >= 4)
					loadingTexture4.Dispose();
				if (highestUnlockedLevel >= 5)
					loadingTexture5.Dispose();
				if (highestUnlockedLevel >= 6)
					loadingTexture6.Dispose();
				if (highestUnlockedLevel >= 7)
					loadingTexture7.Dispose();
				if (highestUnlockedLevel >= 8)
					loadingTexture8.Dispose();
				if (highestUnlockedLevel >= 9)
					loadingTexture9.Dispose();
				if (highestUnlockedLevel >= 10)
					loadingTexture10.Dispose();
				if (highestUnlockedLevel >= 11)
					loadingTexture11.Dispose();
				if (highestUnlockedLevel >= 12)
					loadingTexture12.Dispose();
				if (highestUnlockedLevel >= 13)
					loadingTexture13.Dispose();
				if (highestUnlockedLevel >= 14)
					loadingTexture14.Dispose();
				if (highestUnlockedLevel >= 15)
					loadingTexture15.Dispose();
				if (highestUnlockedLevel >= 16)
					loadingTexture16.Dispose();
				if (highestUnlockedLevel >= 17)
					loadingTexture17.Dispose();
				if (highestUnlockedLevel >= 18)
					loadingTexture18.Dispose();
				if (highestUnlockedLevel >= 19)
					loadingTexture19.Dispose();
				if (highestUnlockedLevel >= 20)
					loadingTexture20.Dispose();
				if (highestUnlockedLevel >= 21)
					loadingTexture21.Dispose();
				if (highestUnlockedLevel >= 22)
					loadingTexture22.Dispose();
				if (highestUnlockedLevel >= 23)
					loadingTexture23.Dispose();
				if (highestUnlockedLevel >= 24)
					loadingTexture24.Dispose();
				if (highestUnlockedLevel >= 25)
					loadingTexture25.Dispose();
				if (highestUnlockedLevel >= 26)
					loadingTexture26.Dispose();
				loadedTextures = false;
			}
		}
		
		public static void Dispose()
		{	
			background.Dispose();
			//maze.Dispose();
			player.Dispose();
			gameOverScreen.Dispose();
			levelComplete.Dispose();
			LoadLoadingTextures(false, 100);
			loadingScreen.Dispose();	
			levelSelectScreen.Dispose();
		}

		public static void Initialize ()
		{
			//Set up director and UISystem.
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			try
			{
				var motionData = Motion.GetData(0);
				if(motionData.Acceleration.X == 0) //If we're on the simulator
					SAVE_DATA = "/Documents/savedata.xml";
				else //If we're on the vita
					SAVE_DATA = "/Application/Documents/savedata.xml";
				
				//Load game data
				if(doesDataFileExist = System.IO.File.Exists(@SAVE_DATA))
				{
					LoadData(); 
				}
				else
					Console.WriteLine("COULD NOT ACCESS SAVE");
			}
			catch
			{
				Console.WriteLine("COULD NOT ACCESS SAVE");
			}
			
			maze = null;
			
			currentLevel = highestUnlockedLevel;
			
			//Add Music and Sound Effects
			AudioManager.AddMusic("/Application/sounds/Music/levelMusic.mp3", "Level1");
			
			AudioManager.AddSound("/Application/sounds/Effects/bounce.wav", "Bounce");
			AudioManager.AddSound("/Application/sounds/Effects/screenFlip.wav", "Screen Flip");
			AudioManager.AddSound("/Application/sounds/Effects/spikesDeath.wav", "Spikes Death");
			AudioManager.AddSound("/Application/sounds/Effects/coinGrab.wav", "Coin Grab");
			AudioManager.AddSound("/Application/sounds/Effects/levelFinish.wav", "Level Finished");
			AudioManager.AddSound("/Application/sounds/Effects/click.wav", "Click");
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			currentState = States.TITLE;
			
			title = new TitleScreen(gameScene, uiScene);	
			AudioManager.PlayMusic("Level1", true, 1.0f, 1.0f);
			
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			
			LoadLoadingTextures(true, 100);
			
			levelSelectScreen = new LevelSelectScreen(gameScene, uiScene, highestUnlockedLevel);
			levelSelectScreen.SetVisible(false, currentLevel);
			
			loadingScreen = new LoadingScreen(gameScene, uiScene);
			loadingScreen.SetVisible(false, currentLevel);
			
			//Begin Timer
			timer = new Timer();
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void InitializeGame()
		{		
			//Background
			background = new Background(gameScene, new Vector2(190.0f, 1215f));
			
			//Maze
			maze = new Maze(gameScene, currentLevel);
			
			//Player
			player = new Player(gameScene, maze.GetSpawnPoint());

			TextureInfo texture		= new TextureInfo("/Application/textures/arrow.png");
			gravityArrow 			= new SpriteUV();
			gravityArrow 			= new SpriteUV(texture);
			gravityArrow.Quad.S 	= texture.TextureSizef;
			gravityArrow.Scale 		= new Vector2(0.5f, .5f);
			gravityArrow.Pivot 		= new Vector2(gravityArrow.Quad.S.X/2, gravityArrow.Quad.S.Y);
			gravityArrow.Position 	= new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f,
			                                  (Director.Instance.GL.Context.GetViewport().Height*0.5f) - gravityArrow.Quad.S.Y);
			
			gameScene.AddChild(gravityArrow);
			gravityArrow.Visible = false;
			
			levelComplete = new LevelComplete(gameScene, uiScene);
			gameOverScreen = new GameOverScreen(gameScene);
			playerBox = player.getBounds();
			
			scoreHUD = new Sce.PlayStation.HighLevel.UI.ImageBox();
			scoreHUD.Image = new ImageAsset("/Application/assets/HUD Side Piece.png");
			scoreHUD.ImageScaleType = ImageScaleType.Center;
			scoreHUD.X = 23.0f;
			scoreHUD.Y = -25.0f;
			scoreHUD.SetSize(200, 200);
			scoreHUD.Visible = false;
			uiScene.RootWidget.AddChildLast(scoreHUD);
			
			scoreLabel = new Sce.PlayStation.HighLevel.UI.Label(); //Set the Score Label
			scoreLabel.X = 55.0f;
			scoreLabel.Y = 60.0f;
			scoreLabel.Text = "Score";
			scoreLabel.Visible = false;
			uiScene.RootWidget.AddChildLast(scoreLabel);
			
			levelScore = new Sce.PlayStation.HighLevel.UI.Label(); //Set the Score 
			levelScore.X = 140.0f;
			levelScore.Y = 60.0f;
			levelScore.Text = "" + score;
			levelScore.Visible = false;
			uiScene.RootWidget.AddChildLast(levelScore);
			
			texture = new TextureInfo("/Application/textures/highscoreTab.png");
			highscoreTab 				= new SpriteUV();
			highscoreTab 				= new SpriteUV(texture);
			highscoreTab.Quad.S 		= texture.TextureSizef;
			highscoreTab.Scale 		= new Vector2(1.0f, 1.0f);
			highscoreTab.Pivot 		= new Vector2(highscoreTab.Quad.S.X/2, highscoreTab.Quad.S.Y);
			highscoreTab.Position 	= new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f, Director.Instance.GL.Context.GetViewport().Height*0.5f);
			highscoreTab.Visible = false;
			gameScene.AddChild(highscoreTab);
			
			highscoreLabel = new Sce.PlayStation.HighLevel.UI.Label[5];
			
			for(int i = 0; i < 5; i++)
			{
				highscoreLabel[i] = new Sce.PlayStation.HighLevel.UI.Label(); //Set the Score Label
				highscoreLabel[i].X = 50.0f;
				highscoreLabel[i].Y = 350.0f + (i * 30.0f);
				
				if(i == 0)
					highscoreLabel[i].Text = "1st :  " + loadedLevelHighscores[(currentLevel != 0) ? currentLevel - 1 : currentLevel][i].GetScore();
				else if(i == 1)
					highscoreLabel[i].Text = "2nd : " + loadedLevelHighscores[(currentLevel != 0) ? currentLevel - 1 : currentLevel][i].GetScore();
				else if(i == 2)
					highscoreLabel[i].Text = "3rd :  " + loadedLevelHighscores[(currentLevel != 0) ? currentLevel - 1 : currentLevel][i].GetScore();
				else if(i == 3)
					highscoreLabel[i].Text = "4th :  " + loadedLevelHighscores[(currentLevel != 0) ? currentLevel - 1 : currentLevel][i].GetScore();
				else 
					highscoreLabel[i].Text = "5th :  " + loadedLevelHighscores[(currentLevel != 0) ? currentLevel - 1 : currentLevel][i].GetScore();
				
				highscoreLabel[i].Visible = false;
				uiScene.RootWidget.AddChildLast(highscoreLabel[i]);
			}
			timerHUD = new Sce.PlayStation.HighLevel.UI.ImageBox();
			timerHUD.Image = new ImageAsset("/Application/assets/HUD Side Piece.png");
			timerHUD.ImageScaleType = ImageScaleType.Center;
			timerHUD.X = 729.0f;
			timerHUD.Y = -25.0f;
			timerHUD.SetSize(200, 200);
			timerHUD.Visible = false;
			uiScene.RootWidget.AddChildLast(timerHUD);
			
			timerLabel = new Sce.PlayStation.HighLevel.UI.Label(); //Set the Timer Label
			timerLabel.X = 765.0f;
			timerLabel.Y = 60.0f;
			timerLabel.Text = "Time";
			timerLabel.Visible = false;
			uiScene.RootWidget.AddChildLast(timerLabel);
			
			levelTimer = new Sce.PlayStation.HighLevel.UI.Label(); //Set the Timer
			levelTimer.X = 855.0f;
			levelTimer.Y = 60.0f;
			levelTimer.Visible = false;
			levelTimer.Text = "" + currentTime;
			uiScene.RootWidget.AddChildLast(levelTimer);
			
			UISystem.SetScene(uiScene);
		}
		
		public static void StartLevel()
		{
			currentScore = new Highscore(currentLevel, 0, "player");
			score = 0;
			loadingScreen.SetVisible(false, currentLevel);
			gameScene.Camera2D.SetViewY(new Vector2((Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Cos(cameraRotation), (Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Sin(cameraRotation)), player.GetPos()); 
			scoreLabel.Visible = true;
			timerLabel.Visible = true;
			timerHUD.Visible = true;
			scoreHUD.Visible = true;
			levelTimer.Visible = true;
			levelScore.Visible = true;
			timer.Reset();
		}
		
		public static void Update()
		{		
			CheckInput();
			switch (currentState)
			{
				case States.TITLE:
				{
					title.Update();
					if(title.HighestLevelChanged())
					{
						highestUnlockedLevel = title.NewHighestLevel();
						LoadLoadingTextures(false, currentLevel);
						LoadLoadingTextures(true, highestUnlockedLevel);
					}
					if (title.CheckPlay())
				    {
						AudioManager.PlaySound("Click", false, 1.0f, 1.0f);
						levelSelectScreen.SetVisible(true, highestUnlockedLevel);
						title.RemoveAll();
						currentState = States.LEVELSELECT;
					}
				break;
				}
				case States.LEVELSELECT:
				{
					if (levelSelectScreen.Selected()) 
					{
						AudioManager.PlaySound("Click", false, 1.0f, 1.0f);
						currentLevel = levelSelectScreen.levelSelected;
						loadingScreen.SetVisible(true, currentLevel);
						gameScene.Camera2D.SetViewY(new Vector2((Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Cos(cameraRotation), (Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Sin(cameraRotation)), new Vector2(-5000.0f, -5000.0f)); 
						timeStamp1 = (int)timer.Milliseconds() + 1;
						levelSelectScreen.SetVisible(false, highestUnlockedLevel);
						loadingScreen.SetLoadTime((int)timer.Milliseconds() + 1500);
						currentState = States.LOADING;
						title.RemoveAll();
					}
					if (levelSelectScreen.BackPressed())
					{
						AudioManager.PlaySound("Click", false, 1.0f, 1.0f);
						currentState = States.TITLE;
						levelSelectScreen.SetVisible(false, highestUnlockedLevel);
						title = new TitleScreen(gameScene, uiScene);	
					}
					levelSelectScreen.Update();
					if (!loadedTextures)
					{
						LoadLoadingTextures(true, 100);
					}
				break;
				}
				case States.LOADING:
				{
					loadingScreen.SetVisible(true, currentLevel);
					currentState = States.LOADED;
					InitializeGame();
					background.UpdateTexture(currentLevel);
				break;
				}
				case States.LOADED:
				{
					loadingScreen.Update((int)timer.Milliseconds());
					if (loadingScreen.CheckPlay())
					{
						AudioManager.PlaySound("Click", false, 1.0f, 1.0f);
						StartLevel();
						currentState = States.PLAYING;
						loadingScreen.SetVisible(false, currentLevel);
						play = true;
						player.setVisibility(true);
						LoadLoadingTextures(false, 100);
						maze.SetLevelFinished(false);
					}
				break;
				}
				case States.PLAYING:
				{
				
					if (!pause && player.IsAlive())
					{
						time = (int)timer.Seconds();
						player.Update(gravityVector, playerDirection, movementVector, invert, falling, additionalForces);
						UpdateCamera();
						CheckCollisions();
						currentTime = time;
						UpdateUI ();
					}
					else if (!player.IsAlive())
					{
						UpdateCamera();
						gameOverScreen.Update();
						if (gameOverScreen.CheckRestart())
						{
							restartGame();
						}
					}
				break;
				}
				case States.LEVELCOMPLETE:
				{
					if (levelComplete.GetState() == 0) //Waiting for the user to make a choice
					{
						
					}
					else if (levelComplete.GetState() == 1) //Back to level select screen
					{
						cameraRotation = FMath.PI/2.0f;
						currentLevel++;
						if (currentLevel > highestUnlockedLevel)
							highestUnlockedLevel = currentLevel;
						maze.RemoveLevel();
						gameScene.Camera2D.SetViewY(new Vector2((Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Cos(cameraRotation), (Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Sin(cameraRotation)), new Vector2(480.0f, 272.0f)); 
						levelSelectScreen.SetVisible(true, highestUnlockedLevel);
						currentState = States.LEVELSELECT;
						play = false;
						pause = false;
						scoreLabel.Visible = false;
						timerLabel.Visible = false;
						timerHUD.Visible = false;
						scoreHUD.Visible = false;
						levelTimer.Visible = false;
						levelScore.Visible = false;
						levelComplete.HideScreen();
						for(int i = 0; i < 5; i++)
						{
							highscoreLabel[i].Visible = false;
						}
						highscoreTab.Visible = false;
						gameScene.RemoveChild(highscoreTab, true);
						gameScene.AddChild(highscoreTab);
						background.SetVisible(false);
						player.setVisibility(false);
						System.GC.Collect();
					}
					else if (levelComplete.GetState() == 2) //Replay the current level
					{
						cameraRotation = FMath.PI/2.0f;
						if (currentLevel+1 > highestUnlockedLevel)
							highestUnlockedLevel = currentLevel+1;
						maze.RemoveLevel();
						maze.LoadLevel(gameScene, currentLevel);
						player.SetPos(maze.GetSpawnPoint());
						play = true;
						pause = false;
						levelComplete.HideScreen();
						for(int i = 0; i < 5; i++)
						{
							highscoreLabel[i].Visible = false;
						}
						highscoreTab.Visible = false;
						gameScene.RemoveChild(highscoreTab, true);
						currentState = States.PLAYING;
						timer.Reset();
						levelComplete.ReOrderZ(gameScene);
						gameOverScreen.ReOrderZ(gameScene);
						gameScene.AddChild(highscoreTab);
						score = 0;
						maze.SetLevelFinished(false);
					}
					else if (levelComplete.GetState() == 3) //Play the next level
					{
						cameraRotation = FMath.PI/2.0f;
						if (currentLevel > highestUnlockedLevel)
							highestUnlockedLevel = currentLevel;
						gameScene.Camera2D.SetViewY(new Vector2((Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Cos(cameraRotation), (Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Sin(cameraRotation)), new Vector2(-5000.0f, -5000.0f)); 
						currentLevel++;
						LoadLoadingTextures(true, currentLevel);
						maze.RemoveLevel();
						maze.LoadLevel(gameScene, currentLevel);
						background.UpdateTexture(currentLevel);
						System.GC.Collect();
						loadingScreen.SetVisible(true, currentLevel);
						timeStamp1 = (int)timer.Milliseconds() + 1;
						loadingScreen.SetLoadTime((int)timer.Milliseconds() + 1500);
						currentState = States.LOADED;
						play = false;
						pause = false;
						scoreLabel.Visible = false;
						timerLabel.Visible = false;
						timerHUD.Visible = false;
						scoreHUD.Visible = false;
						levelTimer.Visible = false;
						levelScore.Visible = false;
						levelComplete.HideScreen();
						for(int i = 0; i < 5; i++)
						{
							highscoreLabel[i].Visible = false;
						}
						highscoreTab.Visible = false;
						gameScene.RemoveChild(highscoreTab, true);
						levelComplete.ReOrderZ(gameScene);
						gameOverScreen.ReOrderZ(gameScene);
						gameScene.AddChild(highscoreTab);
						player.SetPos(maze.GetSpawnPoint());
						maze.SetLevelFinished(false);
					}
				break;
				}
			}
		}
		
		public static void CheckInput()
		{
			//Query gamepad for current state
			var gamePadData = GamePad.GetData(0);
			
			var motionData = Motion.GetData(0);
			
			//Determine whether the player tapped the screen
			List<TouchData> touches = Touch.GetData(0);			
			UISystem.Update(touches);
			
			if (Input2.GamePad0.Triangle.Down) //Reset player
			{
				player.SetPos(maze.GetSpawnPoint());
				player.SetVelocity(0.0f);
				cameraRotation = FMath.PI/2.0f;
			}
			
			if (Input2.GamePad0.Cross.Down) //Include gravity arrow
			{
				//gravityArrow.Visible = true;
			}
			
			if (Input2.GamePad0.Circle.Down) //Exits app
			{
				SystemMemory.Dump();
			}
			
			if (Input2.GamePad0.Square.Down) //Change camera zoom
			{
//				if (play)
//				{
//					if(lastTime == 0)
//						lastTime = time;
//					else if((time - lastTime) < 300.0f) // May need to replace value
//					{
//						zoomedIn = !zoomedIn;
//						lastTime = 0;
//					}
//					else
//					{
//						lastTime = time;
//					}
//				}
			}
			
			if (Input2.GamePad0.Right.Down) //Move right
			{
				keyboardVector = new Vector2(-1.0f, 0.0f);	
			}
			else if (Input2.GamePad0.Left.Down) //Move left
			{
				keyboardVector = new Vector2(1.0f, 0.0f);	
			}
			else
			{
				keyboardVector = new Vector2(0.0f, 0.0f);
				
			}
			
			foreach(TouchData data in touches) //Get touch data
			{
				if(data.Status.Equals(TouchStatus.Down))
				{
					oldTouchPos = new Vector2( data.X, data.Y );
					newTouchPos = new Vector2( data.X, data.Y );
				}
				
				if(data.Status.Equals(TouchStatus.Move))
				{
					newTouchPos = new Vector2( data.X, data.Y ); // Records the last position of swipe if movement is detected.	RMDS
				}
				
				if(maze == null)
				{
					rotationNotAllowed = true;
				}
				else
				{
					rotationNotAllowed = maze.IsLevelComplete();
				}
				
				if(data.Status.Equals(TouchStatus.Up) && rotationNotAllowed == false)	
				{			
					if (cameraRotation > 60f)
						cameraRotation = 0;
					else if (cameraRotation < -60f)
						cameraRotation = 0;
					
					if((oldTouchPos.Y - newTouchPos.Y) > 0.20f) // Swipe Upwards @AS @RMDS				
					{
						endRotation = cameraRotation + FMath.PI;
						player.SetPos(player.GetPos() - gravityVector*25);
						AudioManager.PlaySound("Screen Flip", false, 1.0f, 1.0f);
						rotating = true;
						sideRotation = false;
						rightRotation = false;
						falling = true;
					}
					
					if((oldTouchPos.X - newTouchPos.X) > 0.20f)	//Swipe Left @AS @SM			
					{
						endRotation = cameraRotation - FMath.PI/2;	
						player.SetPos(player.GetPos() - gravityVector*25);
						AudioManager.PlaySound("Screen Flip", false, 1.0f, 1.0f);
						rotating = true;
						sideRotation = true;
						rightRotation = false;
						falling = true;
					}
					if((oldTouchPos.X - newTouchPos.X) < -0.20f) //Swipe Right @AS @SM
					{
						endRotation = cameraRotation + FMath.PI/2;	
						player.SetPos(player.GetPos() - gravityVector*25);
						AudioManager.PlaySound("Screen Flip", false, 1.0f, 1.0f);
						rotating = true;
						sideRotation = true;
						rightRotation = true;
						falling = true;
					}
					//falling = true;
					//CalcCamRestrictions();
				}						
			}	
			
			if (rotating) //If we're rotating the camera
			{
				if (!player.IsAlive())
				{
					cameraRotation = FMath.PI/2.0f;
					rotating = false;
				}
				else
				{
					if (!sideRotation) //Rotating upwards
					{
						cameraRotation += FMath.PI/10;
						if(cameraRotation >= endRotation)
							rotating = false;
						else
							rotating = true;
					}
					else
					{
						if (rightRotation) //Rotating right
						{
							cameraRotation += FMath.PI/10;
							if(cameraRotation >= endRotation)
							{
								rotating = false;
								sideRotation = false;
								rightRotation = false;
							}
							else
							{
								rotating = true;
								rightRotation = true;
							}
						}
						else //Rotating left
						{
							cameraRotation -= FMath.PI/10;
							if(cameraRotation <= endRotation)
							{
								rotating = false;
								sideRotation = false;
								rightRotation = false;
							}
							else
								rotating = true;
						}
					}		
				}
			}
			
			if(play)
			{			
				//cameraRotation += gamePadData.AnalogLeftX / 10.0f;	// Rotates via the left analog stick (need to change the data read to be from the accelerometer).	RMDS				
							
				gravityArrow.Position = new Vector2(player.GetPos().X, player.GetPos().Y); // Keeps the arrow next to the player to show the direction of gravity.	RMDS		
				gravityArrow.Angle = motionData.Acceleration.X + cameraRotation - (FMath.PI / 2.0f);			
					
				
				if (-FMath.Sin(cameraRotation) == 1f) //Up
				{
					currGrav = 3;
					movementVector = new Vector2(-motionData.Acceleration.X + keyboardVector.X, 0.0f);	
					gravityVector = new Vector2(-FMath.Cos(cameraRotation) - motionData.Acceleration.X + keyboardVector.X, -FMath.Sin(cameraRotation));
				}
				else  
				{
						if (-FMath.Cos(cameraRotation) == 1f) //Right
						{
							movementVector = new Vector2(0.0f, motionData.Acceleration.X - keyboardVector.X);
							currGrav = 2;
							gravityVector = new Vector2(-FMath.Cos(cameraRotation), -FMath.Sin(cameraRotation) + motionData.Acceleration.X - keyboardVector.X);
						}
						else if (-FMath.Cos(cameraRotation) == -1f) //Left
						{
							movementVector = new Vector2(0.0f, -motionData.Acceleration.X + keyboardVector.X);
							currGrav = 4;
							gravityVector = new Vector2(-FMath.Cos(cameraRotation), -FMath.Sin(cameraRotation) - motionData.Acceleration.X + keyboardVector.X);
						}
						else //Down
						{
							movementVector = new Vector2(motionData.Acceleration.X - keyboardVector.X, 0.0f);
							currGrav = 1;
							gravityVector = new Vector2(-FMath.Cos(cameraRotation) + motionData.Acceleration.X - keyboardVector.X, -FMath.Sin(cameraRotation));
						}
				}
				
				playerDirection = -gravityVector; //Rotation is the invert of the gravity vector
				//for( int i = 0; i < 25; i++ ) //Output details to console
			    //   Console.WriteLine("");
				//Console.WriteLine("Current Gravity: " + currGrav + " --- Falling: " + falling + " --- Invert: " + invert + " --- GravVec:  " + gravityVector + " --- CamRot: " + cameraRotation + " --- MotionData: " + motionData.Acceleration.X + " --- CurrentState: " + currentState + " --- HighestLevel: " + highestUnlockedLevel);
			}
		}

		//Camera. Focus on player. Don't let the camera show any off map area. If the player walks near the edge
		//leave the edge of the camera on the edge of the map but let the player walk to the actual map edge.
		//If the player isn't within screenwidth/2 or screen height/2 of a edge of the map then center on the
		//player.
		public static void UpdateCamera() //@AS (max width and max height are currently unknown so set to 2000)
		{
			// Commented Out this code for debugging and demonstrating gravity.	RMDS
			
			//	if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.3f) || (player.GetX() > 2000f - Director.Instance.GL.Context.GetViewport().Width*0.3f) ||
			//   (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.3f) || (player.GetY() > 2000f - Director.Instance.GL.Context.GetViewport().Height*0.3f))
			//{
			//	if (player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.4f) //Near left side
			//		gameScene.Camera2D.SetViewY(new Vector2(cameraRotationX,Director.Instance.GL.Context.GetViewport().Height*supportVectorYValue),
			//		                            new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f, player.GetY()));
			//	if (player.GetX() > 2000f - Director.Instance.GL.Context.GetViewport().Width*0.4f) //Near right side
			//		gameScene.Camera2D.SetViewY(new Vector2(cameraRotationX,Director.Instance.GL.Context.GetViewport().Height*supportVectorYValue),
			//		                            new Vector2(2000f - Director.Instance.GL.Context.GetViewport().Width*0.4f, player.GetY()));
			//	if (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.4f) //Near bottom side
			//		gameScene.Camera2D.SetViewY(new Vector2(cameraRotationX,Director.Instance.GL.Context.GetViewport().Height*supportVectorYValue),
			//		                            new Vector2(player.GetX(), Director.Instance.GL.Context.GetViewport().Height*0.4f));
			//	if (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f) //Near top side
			//		gameScene.Camera2D.SetViewY(new Vector2(cameraRotationX,Director.Instance.GL.Context.GetViewport().Height*supportVectorYValue),
			//		                            new Vector2(player.GetX(), 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f));
			//	if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.4f) && (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.4f)) //Near bottom left corner
			//		gameScene.Camera2D.SetViewY(new Vector2(cameraRotationX,Director.Instance.GL.Context.GetViewport().Height*supportVectorYValue),
			//		                            new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f, Director.Instance.GL.Context.GetViewport().Height*0.4f));
			//	if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.4f) && (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f)) //Near top left corner
			//		gameScene.Camera2D.SetViewY(new Vector2(cameraRotationX,Director.Instance.GL.Context.GetViewport().Height*supportVectorYValue),
			//		                            new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f, 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f));
			//	if ((player.GetX() > 2000.0f - Director.Instance.GL.Context.GetViewport().Width*0.4f) && (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f)) //Near top right corner
			//		gameScene.Camera2D.SetViewY(new Vector2(cameraRotationX,Director.Instance.GL.Context.GetViewport().Height*supportVectorYValue),
			//		                            new Vector2(2000.0f - Director.Instance.GL.Context.GetViewport().Width*0.4f, 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f));
			//	if ((player.GetX() > 2000.0f -  Director.Instance.GL.Context.GetViewport().Width*0.4f) && (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.4f)) //Near bottom right corner
			//		gameScene.Camera2D.SetViewY(new Vector2(cameraRotationX,Director.Instance.GL.Context.GetViewport().Height*supportVectorYValue),
			//		                            new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f, Director.Instance.GL.Context.GetViewport().Height*0.4f));
			//}
			//else
			background.Update(player.GetPos(), -new Vector2(-FMath.Cos(cameraRotation), -FMath.Sin(cameraRotation)));
			
			if(zoomedIn)
			{
				zoom = 2.0f;
				gameScene.Camera2D.SetViewY(new Vector2(0.0f, (Director.Instance.GL.Context.GetViewport().Height * 1.5f)), new Vector2(Director.Instance.GL.Context.GetViewport().Width/2, (Director.Instance.GL.Context.GetViewport().Height/2) + 500.0f)); //Player not near an edge		
			}
			else
			{
				//zoom = 0.5f;
				//gameScene.Camera2D.SetViewY(new Vector2((Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Cos(cameraRotation), (Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Sin(cameraRotation)), player.GetPos()); //Player not near an edge				
				gameScene.Camera2D.SetViewY(new Vector2((Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Cos(cameraRotation), (Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Sin(cameraRotation)), player.GetPos()); 
			}
		}
			
		public static void CheckCollisions() //V2.1 @AS @AW
		{	
			playerBox.Min.X = player.GetPos().X - 25; //Non-rotatable bounding box around the player
			playerBox.Max.X = player.GetPos().X + 25;
			playerBox.Min.Y = player.GetPos().Y - 25;
			playerBox.Max.Y = player.GetPos().Y + 25;
				
			if (play)
			{
				if(maze.HasCollidedWithPlayer(playerBox)) //If the box has collided with a tile
				{	
					if (maze.HasHitSide(playerBox, currGrav)) //Check if it's a side tile
					{
						if (currGrav == 3 || currGrav == 1)
						{
							invert = true; //Set invert to false so the X axis gets inverted
						}
						else
						{
							invert = false;
						}
						player.SetPos(player.GetPos() - movementVector*10f); //Bounce the player off the sides (WILL MAKE SMOOTHER) 
					}
					else
					{
						if (currGrav == 3 || currGrav == 1)
							invert = false; //Set invert to false so the X axis gets inverted
						else
							invert = true;
					}
	
					if (player.GetVelocity() > -2.0f && player.GetVelocity() < 2.0f)
					{
						falling = false; //If he's moving too slowly stop him bouncing
						if (player.GetVelocity() < 0)
							player.SetVelocity(-player.GetVelocity()); 
					}
					else
					{
						if (player.GetVelocity() > 6f)
							player.SetVelocity(-6f);
						else
							player.SetVelocity(-player.GetVelocity()); //Invert the velocity so the player bounces
					}
				}
				else
					falling = true; //If no intersection then we are falling
			}
			
			//Scoring collsions @AW
			int prevScore = score;
			if(time < 20)
			{
				score += (int)(maze.CheckCollectableCollision(player.Sprite, gameScene));
			}
			else if(time < 40 && time >= 20)
			{
				score += (int)(maze.CheckCollectableCollision(player.Sprite, gameScene) * 0.75);
			}
			else if(time < 60 && time >= 40)
			{
				score += (int)(maze.CheckCollectableCollision(player.Sprite, gameScene) * 0.50);
			}
			else if(time >= 60)
			{
				score += (int)(maze.CheckCollectableCollision(player.Sprite, gameScene) * 0.25);
			}
			
			if(score != prevScore)
			{
				AudioManager.PlaySound("Coin Grab" , false, 1.0f, 1.0f);
			}
			
			bool collide = maze.CheckObstacleCollisions(player.Sprite);
			
			if(collide && player.IsAlive())
			{
				cameraRotation = FMath.PI/2.0f;
				UpdateCamera();
				AudioManager.PlaySound("Spikes Death", false, 1.0f, 1.0f);
				player.Dead();
				gameOverScreen.Show(player.GetX(), player.GetY());
				pause = true;
			}
			
			collide = maze.CheckFlagCollision(player.Sprite);
			
			if(collide && maze.IsLevelComplete() == false)
			{
				cameraRotation = FMath.PI/2.0f;
				UpdateCamera();
				
				overallLevelScore = maze.GetOverallLevelScore();
				
				int starScore = 0;
				
				if(score <= (int)(overallLevelScore * 0.33))
				{
					starScore = 1;
				}
				else if(score > (int)(overallLevelScore * 0.33) && score <= (int)(overallLevelScore * 0.66))
				{
					starScore = 2;
				}
				else if(score > (int)(overallLevelScore * 0.66))
				{
					starScore = 3;
				}
				levelComplete.Show(player.GetX(), player.GetY(), starScore, currentLevel);
				pause = true;
				maze.SetLevelFinished(true);
				AudioManager.PlaySound("Level Finished", false, 1.0f, 1.0f);
				
				currentScore.SetScore(score);
				
				SaveData();
				
				highscoreTab.Position 	= new Vector2(player.GetPos().X - 600.0f, player.GetPos().Y - 300.0f);
				
				highscoreTab.Visible = true;
				
				for(int i = 0; i < 5; i++)
				{
					if(i == 0)
						highscoreLabel[i].Text = "1st :  " + loadedLevelHighscores[(currentLevel != 0) ? currentLevel - 1 : currentLevel][i].GetScore();
					else if(i == 1)
						highscoreLabel[i].Text = "2nd : " + loadedLevelHighscores[(currentLevel != 0) ? currentLevel - 1 : currentLevel][i].GetScore();
					else if(i == 2)
						highscoreLabel[i].Text = "3rd :  " + loadedLevelHighscores[(currentLevel != 0) ? currentLevel - 1 : currentLevel][i].GetScore();
					else if(i == 3)
						highscoreLabel[i].Text = "4th :  " + loadedLevelHighscores[(currentLevel != 0) ? currentLevel - 1 : currentLevel][i].GetScore();
					else 
						highscoreLabel[i].Text = "5th :  " + loadedLevelHighscores[(currentLevel != 0) ? currentLevel - 1 : currentLevel][i].GetScore();
					
					highscoreLabel[i].Visible = true;
				}
				currentState = States.LEVELCOMPLETE;
				collide = false;
			}
			
			// Check Laser Gate collision		RMDS
			//collide = maze.CheckLaserGates(player);
				
			if(collide && player.IsAlive())
			{
				AudioManager.PlaySound("Spikes Death", false, 1.0f, 1.0f);
				player.Dead();
			}
			
			// Check Breakable Wall collision	
			collide = maze.CheckBreakableWalls(player);
			
			if(collide) //If the box has collided with a tile
			{	
				if (maze.HasHitSide(playerBox, currGrav)) //Check if it's a side tile
				{
					invert = true; //Set invert to true so the Y axis gets inverted
					player.SetPos(player.GetPos() - movementVector*10);
				}
				else
				{
					if (currGrav == 3 || currGrav == 1)
						invert = false; //Set invert to false so the X axis gets inverted
					else
						invert = true;
				}
							
				//if (player.GetVelocity() > -6.1f && player.GetVelocity() < 6.1f)
				//{
					if (player.GetVelocity() > -2.0f && player.GetVelocity() < 2.0f)
					{
						falling = false; //If he's moving too slowly stop him falling
					}
					else
					{
						player.SetVelocity(-player.GetVelocity()); //Invert the velocity so the player rebounds
					}
					
				//}
			}
			//else
			//	falling = true; //If no intersection then we are falling
			
			additionalForces = maze.CheckBlackHole(player) + maze.CheckWindTunnel(player);
		}
		
		public static void restartGame()
		{
			cameraRotation = FMath.PI/2.0f;
			gameScene.RemoveChild(highscoreTab, true);
			player.setAlive();
			player.SetPos(maze.GetSpawnPoint());
			player.SetVelocity(0.0f);
			gameOverScreen.Reset();
			play = true;
			pause = false;
			score = 0;
			maze.RemoveLevel();
			maze.LoadLevel(gameScene, currentLevel);
			levelComplete.ReOrderZ(gameScene);
			gameOverScreen.ReOrderZ(gameScene);
			currentState = States.PLAYING;
			gameScene.AddChild(highscoreTab);
			highscoreTab.Visible = false;
			
			for(int i = 0; i < 5; i++)
					highscoreLabel[i].Visible = false;	
			
			
			timer.Reset();
			//endRotation = FMath.PI/2.0f;
			//cameraRotation = FMath.PI/2.0f;
		}
		
		
		public static void UpdateUI()
		{
//			//Set the HUD
//			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
//			//Set the Score Label
//			scoreLabel = new Sce.PlayStation.HighLevel.UI.Label();
//			scoreLabel.X = 34.0f;
//			scoreLabel.Y = 33.0f;
//			scoreLabel.Text = "Score";
//			uiScene.RootWidget.AddChildLast(scoreLabel);
//			//Set the Score 
//			levelScore = new Sce.PlayStation.HighLevel.UI.Label();
//			levelScore.X = 118.0f;
//			levelScore.Y = 33.0f;
			levelScore.Text = "" + score;
//			uiScene.RootWidget.AddChildLast(levelScore);
//			//Set the Timer Label
//			timerLabel = new Sce.PlayStation.HighLevel.UI.Label();
//			timerLabel.X = 743.0f;
//			timerLabel.Y = 33.0f;
//			timerLabel.Text = "Time";
//			uiScene.RootWidget.AddChildLast(timerLabel);
//			//Set the Timer
//			levelTimer = new Sce.PlayStation.HighLevel.UI.Label();
//			levelTimer.X = 819.0f;
//			levelTimer.Y = 33.0f;
			levelTimer.Text = "" + currentTime;
//			
//			uiScene.RootWidget.AddChildLast(levelTimer);
//			UISystem.SetScene(uiScene);
		}
		
		public static void SaveData() // Save the game data (highscore)		RMDS
		{			
			// Amend highscores of this current level
			//currentHighscore
			
			int level = currentScore.GetLevel();
			int highscorePos = -1;
			
			for(int i = 0; i < 5; i++)
			{
				if(currentScore.GetScore() > loadedLevelHighscores[(level != 0) ? level - 1 : level][i].GetScore())
				{
					highscorePos = i;
					break;
				}		
			}
			
			if(highscorePos != -1)
			{			
				for(int i = 4; i > highscorePos; i--)
				{
					int movedScore = loadedLevelHighscores[(level != 0) ? level - 1 : level][i - 1].GetScore();
					
					loadedLevelHighscores[(level != 0) ? level - 1 : level][i].SetScore(movedScore);
				}
				
				loadedLevelHighscores[(level != 0) ? level - 1 : level][highscorePos].SetScore(currentScore.GetScore());
				loadedLevelHighscores[(level != 0) ? level - 1 : level][highscorePos].SetPlayerName(currentScore.GetPlayerName());
			}
			
			
			
			List<int> allId = new List<int>();
			
			XmlDocument doc = new XmlDocument();
			
			if(System.IO.File.Exists(@SAVE_DATA) == true)
				doc.Load(@SAVE_DATA);
			
			
			XmlNodeList list = doc.SelectNodes("game/level");

			int numOfCompletedLevels = loadedLevelHighscores.Count;
			
			for(int i = 0; i < numOfCompletedLevels; i++)
			{
				XmlNode currentNode = doc.SelectSingleNode("game/level[@id=\"" + (i + 1).ToString() + "\"]");
				
				// If the level data exists then append its children	RMDS
				if(currentNode != null)
				{
					for(int j = 0; j < 5; j++)
					{
						//// Append highscores
						//doc.SelectSingleNode("game/level[@id=\"" + (i + 1).ToString() + "\"]").ChildNodes.Item(0).InnerText =
						//	loadedHighscores[i].GetPlayerName();
						//doc.SelectSingleNode("game/level[@id=\"" + (i + 1).ToString() + "\"]").ChildNodes.Item(1).InnerText =
						//	loadedHighscores[i].GetScore().ToString();	
						
						currentNode.ChildNodes.Item(1 + (j * 2)).InnerText =
							loadedLevelHighscores[i][j].GetPlayerName();
						currentNode.ChildNodes.Item(2 + (j * 2)).InnerText =
							loadedLevelHighscores[i][j].GetScore().ToString();	
					}		
				}				
		
				// Unlock the following level
				if(i < highestUnlockedLevel)
				{				
					doc.SelectSingleNode("game/level[@id=\"" + (i).ToString() + "\"]").ChildNodes.Item(0).InnerText = "unlocked";
				}
				if(i == highestUnlockedLevel && highestUnlockedLevel == currentLevel)
				{				
					doc.SelectSingleNode("game/level[@id=\"" + (i+1).ToString() + "\"]").ChildNodes.Item(0).InnerText = "unlocked";
				}
				try
				{
		        	doc.Save(@SAVE_DATA);	
				}
				catch
				{
					Console.WriteLine("COULD NOT FIND SAVE FILE");
				}
				
			}			
		}
		
		public static bool LoadData() // Load the game data (highscore)	RMDS
		{
			if(System.IO.File.Exists(@SAVE_DATA) == true)
			{		
				// The XML file loaded has data for each level about their highscore and the player
				// that acquired that highscore.	RMDS
							
				XmlDocument doc = new XmlDocument();
				doc.Load(@SAVE_DATA);
				
				// This will provide a list of all the levels that the player has currently earned a highscore in,
				// therefore the player's total progress.	RMDS
				loadedLevelHighscores = new List<List<Highscore>>();
						
				int level = 1;
				
				XmlNode currentNode = doc.SelectSingleNode("game/level[@id=\"" + level.ToString() + "\"]");
					
			
				while(currentNode != null)
				{			
					bool unlocked;
					string playerName;
					int highscore;
					List<Highscore> levelHighscores = new List<Highscore>();
					
					if(currentNode.ChildNodes.Item(0).InnerText.Equals("unlocked"))
						unlocked = true;
					else
						unlocked = false;
						
					if(unlocked)
						highestUnlockedLevel = level;
					
					for(int i = 0; i < 5; i++)
					{
						playerName = currentNode.ChildNodes.Item(1 + (i * 2)).InnerText;
						
						highscore = 0;
						
						if(!currentNode.ChildNodes.Item(2 + (i * 2)).InnerText.Equals(""))
							highscore = Int32.Parse(currentNode.ChildNodes.Item(2 + (i * 2)).InnerText);
						
						Highscore currentLoad = new Highscore(level, highscore, playerName);
						
						levelHighscores.Add(currentLoad);
					}	
					
						loadedLevelHighscores.Add(levelHighscores);
						
						// Load the next level data from the saved data file.	RMDS
						level++;
									
						currentNode.Attributes.GetNamedItem("player");												
						currentNode = doc.SelectSingleNode("game/level[@id=\"" + level.ToString() + "\"]");	
				}			
				
				int numOfLevels = level - 1;
				
				if(numOfLevels > totalNumOfLevels)
					return false; // Should throw exception.	RMDS
			}
			else
				return true;
				 
			return false;
			
		}
		
		public static bool DeleteData()
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(@SAVE_DATA);
					
			for(int i = 0; i < totalNumOfLevels; i++)
			{		
				XmlNode currentNode = doc.SelectSingleNode("game/level[@id=\"" + (i + 1).ToString() + "\"]");
				
				if(currentNode != null)
				{
					currentNode.ChildNodes.Item(0).InnerText = "locked";
					
					for(int j = 0; j < 5; j++)
					{
						currentNode.ChildNodes.Item(1 + (j * 2)).InnerText = "";
						currentNode.ChildNodes.Item(2 + (j * 2)).InnerText = "";	
					}		
				}
				//// Reset all values
				//doc.SelectSingleNode("game/level[@id=\"" + (i + 1).ToString() + "\"]").ChildNodes.Item(0).InnerText = "";
				//doc.SelectSingleNode("game/level[@id=\"" + (i + 1).ToString() + "\"]").ChildNodes.Item(1).InnerText = "";
					
		        doc.Save(@SAVE_DATA);			
			}
			
			return true;
		}
	}
}
