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
		
		private static string SAVE_DATA = "/Documents/savedata.xml";
		private static bool	doesDataFileExist = false;
		
		//------ UI ------\\
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label				scoreLabel;
		private static Sce.PlayStation.HighLevel.UI.Label[]				highscoreLabel;
		private static Sce.PlayStation.HighLevel.UI.Label				levelScore;
		private static Sce.PlayStation.HighLevel.UI.Label				timerLabel;
		private static Sce.PlayStation.HighLevel.UI.Label				levelTimer;
		
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
		
		//------ Player Movement ------\\
		private static Vector2 gravityVector = new Vector2(0.0f, -1.0f); //The direction in which gravity is currently going
		private static Vector2 playerDirection; //Rotation of the player sprite
		private static Vector2 movementVector = new Vector2(0.0f, 0.0f); //Direction player is moving when not falling
		private static Vector2 keyboardVector = new Vector2(0.0f, 0.0f); //Keyboard input
		private static bool invert = false; //To switch between the Y and X axis movement
		private static bool falling = true; //If the player isn't touching a tile then he's falling
		private static Bounds2 playerBox; //Non-rotatable bounds that encompass the player
		public static int currGrav = 1; //ID for the 4 types of camera rotation
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

//		private static float upperCameraRange = FMath.PI/4;
//		private static float lowerCameraRange = -FMath.PI/4;
		
		//------ Menu Data ------\\
		private static bool play = false;
		private static bool pause = false;
		private static int timeStamp1;
		private enum States {TITLE, LEVELSELECT, LOADING, LOADED, PLAYING, LEVELCOMPLETE};
		private static States currentState;
//		private static bool loaded = false;
//		private static bool startLoading = false;
//		private static bool levelSelect = false;
//		private static bool levelSelected = false;
		
		//------ Level Data ------\\
		private static int currentLevel = 1; //The level to load
		private static int highestUnlockedLevel = 5; //Read this in from file eventually (local highscores)
		private static int totalNumOfLevels = 6;
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
		
		public static void Dispose()
		{	
			background.Dispose();
			//maze.Dispose();
			player.Dispose();
			gameOverScreen.Dispose();
			levelComplete.Dispose();
			levelSelectScreen.Dispose();
			loadingScreen.Dispose();	
		}

		public static void Initialize ()
		{
			//Set up director and UISystem.
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
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
			
			title = new TitleScreen(gameScene);	
			AudioManager.PlayMusic("Level1", true, 1.0f, 1.0f);
			
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			
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
			//Load game data
			if(doesDataFileExist = System.IO.File.Exists(SAVE_DATA))
			{
				LoadData();
			}
			
			//Background
			background = new Background(gameScene, new Vector2(190.0f, 1215f));
			
			//Maze
			maze = new Maze(gameScene, currentLevel);
			
			//Player
			player = new Player(gameScene, maze.GetSpawnPoint());

			TextureInfo texture = new TextureInfo("/Application/textures/arrow.png");
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
			
			scoreLabel = new Sce.PlayStation.HighLevel.UI.Label(); //Set the Score Label
			scoreLabel.X = 34.0f;
			scoreLabel.Y = 33.0f;
			scoreLabel.Text = "Score";
			scoreLabel.Visible = false;
			uiScene.RootWidget.AddChildLast(scoreLabel);
			
			levelScore = new Sce.PlayStation.HighLevel.UI.Label(); //Set the Score 
			levelScore.X = 118.0f;
			levelScore.Y = 33.0f;
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
					highscoreLabel[i].Text = "1st :  " + loadedLevelHighscores[currentLevel - 1][i].GetScore();
				else if(i == 1)
					highscoreLabel[i].Text = "2nd : " + loadedLevelHighscores[currentLevel - 1][i].GetScore();
				else if(i == 2)
					highscoreLabel[i].Text = "3rd :  " + loadedLevelHighscores[currentLevel - 1][i].GetScore();
				else if(i == 3)
					highscoreLabel[i].Text = "4th :  " + loadedLevelHighscores[currentLevel - 1][i].GetScore();
				else 
					highscoreLabel[i].Text = "5th :  " + loadedLevelHighscores[currentLevel - 1][i].GetScore();
				
				highscoreLabel[i].Visible = false;
				uiScene.RootWidget.AddChildLast(highscoreLabel[i]);
			}
			timerLabel = new Sce.PlayStation.HighLevel.UI.Label(); //Set the Timer Label
			timerLabel.X = 743.0f;
			timerLabel.Y = 33.0f;
			timerLabel.Text = "Time";
			timerLabel.Visible = false;
			uiScene.RootWidget.AddChildLast(timerLabel);
			
			levelTimer = new Sce.PlayStation.HighLevel.UI.Label(); //Set the Timer
			levelTimer.X = 819.0f;
			levelTimer.Y = 33.0f;
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
					if (title.CheckPlay())
				    {
						AudioManager.PlaySound("Click", false, 1.0f, 1.0f);
						levelSelectScreen.SetVisible(true, currentLevel);
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
						levelSelectScreen.SetVisible(false, currentLevel);
						loadingScreen.SetLoadTime((int)timer.Milliseconds() + 1500);
						currentState = States.LOADING;
						title.RemoveAll();
					}
					if (levelSelectScreen.BackPressed())
					{
						AudioManager.PlaySound("Click", false, 1.0f, 1.0f);
						currentState = States.TITLE;
						levelSelectScreen.SetVisible(false, currentLevel);
						title = new TitleScreen(gameScene);	
					}
					levelSelectScreen.Update();
				break;
				}
				case States.LOADING:
				{
					loadingScreen.SetVisible(true, currentLevel);
					currentState = States.LOADED;
					InitializeGame();
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
						currentLevel++;
						maze.RemoveLevel();
						gameScene.Camera2D.SetViewY(new Vector2((Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Cos(cameraRotation), (Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Sin(cameraRotation)), new Vector2(480.0f, 272.0f)); 
						levelSelectScreen.SetVisible(true, currentLevel);
						currentState = States.LEVELSELECT;
						play = false;
						pause = false;
						scoreLabel.Visible = false;
						timerLabel.Visible = false;
						levelTimer.Visible = false;
						levelScore.Visible = false;
						levelComplete.HideScreen();
						for(int i = 0; i < 5; i++)
						{
							highscoreLabel[i].Visible = false;
						}
						highscoreTab.Visible = false;
						background.SetVisible(false);
						
					}
					else if (levelComplete.GetState() == 2) //Replay the current level
					{
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
						currentState = States.PLAYING;
						timer.Reset();
						levelComplete.ReOrderZ(gameScene);
						gameOverScreen.ReOrderZ(gameScene);
						score = 0;
					}
					else if (levelComplete.GetState() == 3) //Play the next level
					{
						gameScene.Camera2D.SetViewY(new Vector2((Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Cos(cameraRotation), (Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Sin(cameraRotation)), new Vector2(-5000.0f, -5000.0f)); 
						currentLevel++;
						maze.RemoveLevel();
						maze.LoadLevel(gameScene, currentLevel);
						loadingScreen.SetVisible(true, currentLevel);
						timeStamp1 = (int)timer.Milliseconds() + 1;
						loadingScreen.SetLoadTime((int)timer.Milliseconds() + 1500);
						currentState = States.LOADED;
						play = false;
						pause = false;
						scoreLabel.Visible = false;
						timerLabel.Visible = false;
						levelTimer.Visible = false;
						levelScore.Visible = false;
						levelComplete.HideScreen();
						for(int i = 0; i < 5; i++)
						{
							highscoreLabel[i].Visible = false;
						}
						highscoreTab.Visible = false;
						levelComplete.ReOrderZ(gameScene);
						gameOverScreen.ReOrderZ(gameScene);
						player.SetPos(maze.GetSpawnPoint());
					}
				break;
				}
			}
			
//			if (!play)
//			{
//				if (!loaded) //Update title screen
//					title.Update();
//				else if (levelSelect) //Update level select screen
//					levelSelectScreen.Update();
//				else //Update loading screen
//					loadingScreen.Update((int)timer.Milliseconds()); 
//				
//				if (title.CheckPlay() && !loaded && !startLoading) //If we have clicked play on the title screen
//				{
//					levelSelectScreen.SetVisible(true, currentLevel);
//					title.RemoveAll();
//					levelSelected = true;
//				}
//				
//				if (levelSelected && levelSelectScreen.Selected()) //If we have picked a level
//				{
//					AudioManager.PlaySound("Click", false, 1.0f, 1.0f);
//					levelSelect = true;
//					currentLevel = levelSelectScreen.levelSelected;
//				}
//				else if (levelSelected && levelSelectScreen.BackPressed())
//				{
//					AudioManager.PlaySound("Click", false, 1.0f, 1.0f);
//					levelSelected = false;
//					levelSelectScreen.SetVisible(false, currentLevel);
//					title = new TitleScreen(gameScene);	
//				}
//				
//				if (levelSelect && !loaded && !startLoading)
//				{
//					AudioManager.PlaySound("Click", false, 1.0f, 1.0f);
//					startLoading = true;
//					loadingScreen.SetVisible(true, currentLevel);
//					timeStamp1 = (int)timer.Milliseconds() + 1;
//					levelSelect = false;
//					levelSelected = false;
//				}
//				
//				if (startLoading && timer.Milliseconds() > timeStamp1) //If the level has loaded
//				{
//					levelSelectScreen.SetVisible(false, currentLevel);
//					InitializeGame();
//					loadingScreen.SetLoadTime((int)timer.Milliseconds() + 1500);
//					loaded = true;
//					startLoading = false;
//					title.RemoveAll();
//				}
//				
//				if (loaded && loadingScreen.CheckPlay()) //If the play button has been clicked on the loading screen
//				{
//					AudioManager.PlaySound("Click", false, 1.0f, 1.0f);
//					StartLevel();
//					play = true;
//				}
//			} 
//			else
//			{
//				
//				if (!pause && player.IsAlive())
//				{
//					time = (int)timer.Seconds();
//					player.Update(gravityVector, playerDirection, movementVector, invert, falling, additionalForces);
//					
//					UpdateCamera();
//					CheckCollisions();
//					currentTime = time;
//					UpdateUI ();
//				}
//				else if (!player.IsAlive())
//				{
//					gameOverScreen.Update();
//					if (gameOverScreen.CheckRestart())
//					{
//						restartGame();
//					}
//				}
//			}
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
				player.SetPos(new Vector2(190.0f, 330.0f));
				player.SetVelocity(0.5f);
				cameraRotation = FMath.PI/2.0f;
			}
			
			if (Input2.GamePad0.Cross.Down) //Include gravity arrow
			{
				gravityArrow.Visible = true;
			}
			
			if (Input2.GamePad0.Circle.Down) //Exits app
			{
				//Director.Terminate();
			}
			
			if (Input2.GamePad0.Square.Down) //Change camera zoom
			{
				if (play)
				{
					if(lastTime == 0)
						lastTime = time;
					else if((time - lastTime) < 300.0f) // May need to replace value
					{
						zoomedIn = !zoomedIn;
						lastTime = 0;
					}
					else
					{
						lastTime = time;
					}
				}
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

				if(data.Status.Equals(TouchStatus.Up))	
				{				
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
			
			if(play)
			{			
				cameraRotation += gamePadData.AnalogLeftX / 10.0f;	// Rotates via the left analog stick (need to change the data read to be from the accelerometer).	RMDS				
							
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
			    // 	Console.WriteLine("");
				//Console.WriteLine("Current Gravity: " + currGrav + " --- Falling: " + falling + " --- Invert: " + invert + " --- GravVec:  " + gravityVector + " --- CamRot: " + cameraRotation + " --- MotionData: " + motionData.Acceleration.X + " --- PlayerDir: " + playerDirection);
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
				int starScore = 0;
				if(score <= 500)
				{
					starScore = 1;
				}
				else if(score > 500 && score <= 800)
				{
					starScore = 2;
				}
				else if(score > 800 && score <= 1000)
				{
					starScore = 3;
				}
				levelComplete.Show(player.GetX(), player.GetY(), starScore);
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
						highscoreLabel[i].Text = "1st :  " + loadedLevelHighscores[currentLevel - 1][i].GetScore();
					else if(i == 1)
						highscoreLabel[i].Text = "2nd : " + loadedLevelHighscores[currentLevel - 1][i].GetScore();
					else if(i == 2)
						highscoreLabel[i].Text = "3rd :  " + loadedLevelHighscores[currentLevel - 1][i].GetScore();
					else if(i == 3)
						highscoreLabel[i].Text = "4th :  " + loadedLevelHighscores[currentLevel - 1][i].GetScore();
					else 
						highscoreLabel[i].Text = "5th :  " + loadedLevelHighscores[currentLevel - 1][i].GetScore();
					
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
			//cameraRotation = FMath.PI/2.0f;
			player.setAlive();
			player.SetPos(maze.GetSpawnPoint());
			gameOverScreen.Reset();
			play = true;
			pause = false;
			score = 0;
			
			highscoreTab.Visible = false;
			
			for(int i = 0; i < 5; i++)
					highscoreLabel[i].Visible = false;	
			
			
			timer.Reset();
			//endRotation = FMath.PI/2.0f;
			//cameraRotation = FMath.PI/2.0f;
			SaveData();	
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
				if(currentScore.GetScore() > loadedLevelHighscores[level - 1][i].GetScore())
				{
					highscorePos = i;
					break;
				}		
			}
			
			if(highscorePos != -1)
			{			
				for(int i = 4; i > highscorePos; i--)
				{
					int movedScore = loadedLevelHighscores[level - 1][i - 1].GetScore();
					
					loadedLevelHighscores[level - 1][i].SetScore(movedScore);
				}
				
				loadedLevelHighscores[level - 1][highscorePos].SetScore(currentScore.GetScore());
				loadedLevelHighscores[level - 1][highscorePos].SetPlayerName(currentScore.GetPlayerName());
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
						
						currentNode.ChildNodes.Item(0 + (j * 2)).InnerText =
							loadedLevelHighscores[i][j].GetPlayerName();
						currentNode.ChildNodes.Item(1 + (j * 2)).InnerText =
							loadedLevelHighscores[i][j].GetScore().ToString();	
					}		
				}				
		
		        doc.Save(@SAVE_DATA);			
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
					string playerName;
					int highscore;
					List<Highscore> levelHighscores = new List<Highscore>();
					
					for(int i = 0; i < 5; i++)
					{
						playerName = currentNode.ChildNodes.Item(0 + (i * 2)).InnerText;
						
						highscore = 0;
						
						if(!currentNode.ChildNodes.Item(1 + (i * 2)).InnerText.Equals(""))
							highscore = Int32.Parse(currentNode.ChildNodes.Item(1 + (i * 2)).InnerText);
						
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
					for(int j = 0; j < 5; j++)
					{
						currentNode.ChildNodes.Item(0 + (j * 2)).InnerText = "";
						currentNode.ChildNodes.Item(1 + (j * 2)).InnerText = "";	
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
