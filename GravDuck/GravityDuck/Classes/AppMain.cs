using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;
	
namespace GravityDuck
{
	public class AppMain
	{
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label				scoreLabel;
		
		private static Background background;
		private static Maze maze;
		private static Player player;
		private static TitleScreen title;
		private static LevelComplete levelComplete;
		private static GameOverScreen gameOverScreen;
		
		private static Timer timer;
		private static float time;
		
		private static int score;
	
		private static Vector2 gravityVector = new Vector2(0.0f, -1.0f); //The direction in which gravity is currently going
		private static SpriteUV	gravityArrow;
		private static Vector2 playerDirection; //Based on the rotation of the maze this is the direction the player is moving
		private static Vector2 movementVector = new Vector2(0.0f, 0.0f);
		
		private static Vector2 oldTouchPos = new Vector2( 0.0f, 0.0f ); // Position of first touch on screen
		private static Vector2 newTouchPos = new Vector2( 0.0f, 0.0f ); // Position of last touch on screen
		
		private static float cameraRotation = FMath.PI/2.0f; // The rotation of the camera as a angle, as well as other entities
		private static float zoom = 0.5f; // How much of the game can be viewed
		
		private static float upperCameraRange = FMath.PI/4;
		private static float lowerCameraRange = -FMath.PI/4;
		
		private static bool play = false;
		private static bool pause = false;
		private static bool invert = false;
		private static bool falling = true;
		
		private static bool rotating = false; //Camera rotations bools
		private static bool sideRotation = false;
		private static bool rightRotation = false;
		private static float endRotation;
		
		private static Bounds2 playerBox; //Non-rotatable bounds that encompass the player
		
		public static int currGrav = 1;
				
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
			
			background.Dispose();
			maze.Dispose();
			player.Dispose();
			
			Director.Terminate ();
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
			
			AudioManager.PlayMusic("Level1", true, 1.0f, 1.0f);
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			title = new TitleScreen(gameScene);
			
			//Begin Timer
			timer = new Timer();
			time = (float)timer.Milliseconds();
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void InitializeGame()
		{
			//Background
			background = new Background(gameScene);
			
			//Player
			player = new Player(gameScene);
			
			//Maze
			maze = new Maze(gameScene);
				
			TextureInfo texture = new TextureInfo("/Application/textures/arrow.png");
			gravityArrow 			= new SpriteUV();
			gravityArrow 			= new SpriteUV(texture);
			gravityArrow.Quad.S 	= texture.TextureSizef;
			gravityArrow.Scale 		= new Vector2(1.0f, 1.0f);
			gravityArrow.Pivot 		= new Vector2(gravityArrow.Quad.S.X/2, gravityArrow.Quad.S.Y);
			gravityArrow.Position 	= new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f,
			                                  (Director.Instance.GL.Context.GetViewport().Height*0.5f) - gravityArrow.Quad.S.Y);
			gameScene.AddChild(gravityArrow);
			
			levelComplete = new LevelComplete(gameScene);
			
			gameOverScreen = new GameOverScreen(gameScene);
			
			playerBox = player.getBounds();
		}
		
		public static void Update()
		{		
			CheckInput();
			if (!play)
			{
				title.Update();
				if (title.CheckPlay())
				{
					play = true;
					InitializeGame();
				}
			} else{
				if (!pause && player.IsAlive())
				{
					player.Update(gravityVector, playerDirection, movementVector, invert, falling);
					UpdateCamera();
					CheckCollisions();
				} else if (!player.IsAlive())
				{
					gameOverScreen.Update();
					if (gameOverScreen.CheckRestart())
					{
						restartGame();
					}
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
			
			if (Input2.GamePad0.Up.Down)
			{
				player.SetPos(new Vector2(190.0f, 330.0f));
				cameraRotation = FMath.PI/2.0f;
			}
			
			if(play)
			{	
				
				foreach(TouchData data in touches)
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
						if((oldTouchPos.Y - newTouchPos.Y) > 0.30f) // Swipe Upwards @AS @RMDS				
						{
							endRotation = cameraRotation + FMath.PI;
							player.SetPos(player.GetPos() - gravityVector*10);
							AudioManager.PlaySound("Screen Flip", false, 1.0f, 1.0f);
							rotating = true;
							sideRotation = false;
							rightRotation = false;
						}
						
						if((oldTouchPos.X - newTouchPos.X) > 0.30f)	//Swipe Left @AS @SM			
						{
							endRotation = cameraRotation - FMath.PI/2;	
							player.SetPos(player.GetPos() - gravityVector*10);
							AudioManager.PlaySound("Screen Flip", false, 1.0f, 1.0f);
							rotating = true;
							sideRotation = true;
							rightRotation = false;
						}
						if((oldTouchPos.X - newTouchPos.X) < -0.30f) //Swipe Right @AS @SM
						{
							endRotation = cameraRotation + FMath.PI/2;	
							player.SetPos(player.GetPos() - gravityVector*10);
							AudioManager.PlaySound("Screen Flip", false, 1.0f, 1.0f);
							rotating = true;
							sideRotation = true;
							rightRotation = true;
						}
						
						CalcCamRestrictions();
						falling = true;
					}						
				}
				
				cameraRotation += gamePadData.AnalogLeftX / 10.0f;	// Rotates via the left analog stick (need to change the data read to be from the accelerometer).	RMDS				
							
				gravityArrow.Position = new Vector2(player.GetPos().X, player.GetPos().Y); // Keeps the arrow next to the player to show the direction of gravity.	RMDS		
				gravityArrow.Angle = motionData.Acceleration.X + cameraRotation - (FMath.PI / 2.0f);			
				gravityVector = new Vector2(-FMath.Cos(cameraRotation) + motionData.Acceleration.X, -FMath.Sin(cameraRotation));	
				
				if (gravityVector.Y == 1f)
				{
					currGrav = 3;
					if (!falling)
						movementVector = new Vector2(-motionData.Acceleration.X, 0.0f);
				}
				else 
				{
					currGrav = 1;
					if (!falling)
						movementVector = new Vector2(motionData.Acceleration.X, 0.0f);
				}
				
				playerDirection = -gravityVector;
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
		}
		
		public static float FlipCamera(float rotation)
		{
			float change = cameraRotation + rotation;
			
			if(change < 0.0f)
			{
				return change + 6.283184f;
			}
			else if(change > 6.283184f)
				{
					return change - 6.283184f;
				}
			
			return change;
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
				gameScene.Camera2D.SetViewY(new Vector2((Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Cos(cameraRotation),
			                                        (Director.Instance.GL.Context.GetViewport().Height * zoom) * FMath.Sin(cameraRotation)), player.GetPos()); //Player not near an edge
		}
		
		public static void CalcCamRestrictions()
		{	
			if(FMath.Cos(cameraRotation) >= 0)
				if(FMath.Sin(cameraRotation) > 0.45f)
				{
					// Lower quadrant.	RMDS
				
					upperCameraRange = 2.356194f;
					lowerCameraRange = 0.785398f;	
				}
				else if(FMath.Sin(cameraRotation) > -0.45f)
					{
						// Left quadrant.	RMDS
		
						// Determined later within CheckInput(), so these values are placed
						// to cause no events and to be later changed.
					
						if(cameraRotation < 5.497786f)
						{
							upperCameraRange = 0.785398f;
							lowerCameraRange = -0.1f;
						}
						else
						{
							upperCameraRange = 7.0f;
							lowerCameraRange = 5.497786f;
						}
					}
					else
					{
						// Upper quadrant.	RMDS
				
						upperCameraRange = 5.497786f;
						lowerCameraRange = 3.926990f;
					}
			else if(FMath.Sin(cameraRotation) > 0.45f)
				{
					// Lower quadrant.	RMDS
				
					upperCameraRange = 2.356194f;
					lowerCameraRange = 0.785398f;
				}
				else if(FMath.Sin(cameraRotation) > -0.45f)
					{
						// Right quadrant.	RMDS
				
						upperCameraRange = 3.926990f;
						lowerCameraRange = 2.356194f;	
					}
					else
					{
						// Upper quadrant.	RMDS
				
						upperCameraRange = 5.497786f;
						lowerCameraRange = 3.926990f;
					}
		}
		
		public static void CheckCollisions() //V2.0 @AS @AW
		{	
			playerBox.Min.X = player.GetPos().X - 25; //Non-rotatable bounding box around the player
			playerBox.Max.X = player.GetPos().X + 25;
			playerBox.Min.Y = player.GetPos().Y - 25;
			playerBox.Max.Y = player.GetPos().Y + 25;
				
			if(maze.HasCollidedWithPlayer(playerBox)) //If the box has collided with a tile
			{	
				if (maze.HasHitSide(playerBox, currGrav)) //Check if it's a side tile
				{
					invert = true; //Set invert to true so the Y axis gets inverted
					player.SetPos(player.GetPos() - movementVector);
				}
				else
				{
					invert = false; //Set invert to false so the X axis gets inverted
				}
							
				if (player.GetVelocity() > -6.1f && player.GetVelocity() < 6.1f)
				{
					if (player.GetVelocity() > -2.0f && player.GetVelocity() < 2.0f)
					{
						falling = false; //If he's moving too slowly stop him falling
						
					}
					else
						player.SetVelocity(-player.GetVelocity()); //Invert the velocity so the player rebounds
					
				}
			}
			else
				falling = true; //If no intersection then we are falling
			
			//Scoring collsions @AW
			int prevScore = score;
			score += maze.CheckCollectableCollision(player.Sprite, gameScene);
			
			if(score != prevScore)
			{
				AudioManager.PlaySound("Coin Grab" , false, 1.0f, 1.0f);
			}
			
			bool collide = maze.CheckObstacleCollisions(player.Sprite);
			
			if(collide && player.IsAlive())
			{
				AudioManager.PlaySound("Spikes Death", false, 1.0f, 1.0f);
				player.Dead();
				gameOverScreen.Show(player.GetX(), player.GetY());
				pause = true;
			}
			
			collide = maze.CheckFlagCollision(player.Sprite);
			
			if(collide && maze.IsLevelComplete() == false)
			{
				maze.SetLevelFinished(true);
				AudioManager.PlaySound("Level Finished", false, 1.0f, 1.0f);
			}
		}
		public static void restartGame()
		{
			player.setAlive();
			player.resetPosition();
			gameOverScreen.Reset();
			play = true;
			pause = false;
		}
	}
}
