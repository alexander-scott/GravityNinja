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
		
		private static Timer timer;
		private static float time;
	
		
		private static Vector2 gravityVector = new Vector2(0.0f, -1.0f); //The direction in which gravity is currently going
		private static SpriteUV	gravityArrow;
		private static Vector2 playerDirection; //Based on the rotation of the maze this is the direction the player is moving
		
		private static Vector2 oldTouchPos = new Vector2( 0.0f, 0.0f ); // Position of first touch on screen
		private static Vector2 newTouchPos = new Vector2( 0.0f, 0.0f ); // Position of last touch on screen
		
		private static float cameraRotation = FMath.PI/2.0f; // The rotation of the camera as a angle, as well as other entities
		private static float zoom = 0.5f; // How much of the game can be viewed
		
		private static bool play = false;
		private static bool pause = false;
				
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
				if (!pause)
				{
					player.Update(gravityVector, gravityArrow.Angle);
					UpdateCamera();
					CheckCollisions();
				}
			}
		}
		
		public static void CheckInput()
		{
			//Query gamepad for current state
			var gamePadData = GamePad.GetData(0);
			
			
			//Determine whether the player tapped the screen
			List<TouchData> touches = Touch.GetData(0);			
			
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
					if((oldTouchPos.Y - newTouchPos.Y) > 0.30f) // Swipe Upwards.	RMDS					
						cameraRotation += FMath.PI;
				
				
				if(data.Status.Equals(TouchStatus.Up))				
					if((newTouchPos.Y - oldTouchPos.Y) > 0.30f) // Swipe Upwards.	RMDS					
						cameraRotation += FMath.PI;
				
				if(data.Status.Equals(TouchStatus.Up))				
					if((oldTouchPos.X - newTouchPos.X) > 0.30f) // Swipe Upwards. SM				
						cameraRotation -= FMath.PI/2;	
				
				if(data.Status.Equals(TouchStatus.Up))				
					if((newTouchPos.X - oldTouchPos.X) > 0.30f) // Swipe Upwards. SM				
						cameraRotation += FMath.PI/2;
				
				
				}					
			if(play)
			{
				cameraRotation += gamePadData.AnalogLeftX / 100.0f;	// Rotates via the left analog stick (need to change the data read to be from the accelerometer).	RMDS	
			
				gravityArrow.Position = new Vector2(player.GetPos().X, player.GetPos().Y); // Keeps the arrow next to the player to show the direction of gravity.	RMDS		
				gravityArrow.Angle = cameraRotation - (FMath.PI / 2.0f);			
				gravityVector = new Vector2(-FMath.Cos(cameraRotation), -FMath.Sin(cameraRotation));		
			}
			
			//Console.WriteLine("X = " + FMath.Cos(cameraRotation) + "\nY = " + FMath.Sin(cameraRotation)); For debugging.	RMDS
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
		
		public static void CheckCollisions() //@AS
		{	
		//if(player.GetX() % 50 == 0 || player.GetY() % 50 == 0) //If the player hits the side of the a tile
		//{													     //check if its a map boundary
			if(maze.circleCollision(player.GetPos(), 100))
				Console.WriteLine("hi");
			if(maze.CheckCollision(player.Sprite)) //If the player is on a tile
			{	//Check what direction the tile is to the player and move the player in the opposite direction
				player.SetPos(player.GetPos() - new Vector2(maze.HorizontalCollision(player.Sprite).X,maze.VerticalCollision(player.Sprite).Y));
				
				
				//float displacement = player.GetPos() - maze
				
				
				
				
				if (maze.birdWalking(player.Sprite))
				{
					if (maze.HorizontalCollision(player.Sprite).X > 0f)
					{
						player.SetFalling(false, new Vector2(0.0f, 1.0f));
					}
					else
						player.SetFalling(false, new Vector2(1.0f, 0.0f)); //If the bird is touching the ground tile it's not falling
				}	
			}
			else
			{
				player.SetFalling(true, new Vector2(1.0f, 0.0f)); //Bird is falling if it's not touching a tile
				
			}
			/*
			if(player.GetX() > 700)
			{
				levelComplete.Show(player.GetX(), player.GetY(), 2);
				pause = true;
				
			}*/
		//}
		}
		
		public static Vector2 Vector2FromAngle(float angle, bool normalize = true)
		{
		    Vector2 vector = new Vector2((float)FMath.Cos(angle), (float)FMath.Sin(angle));
			
		    if (vector != Vector2.Zero && normalize)
			{
		        vector.Normalize();
			}
			
		    return vector;
		}
	}
}
