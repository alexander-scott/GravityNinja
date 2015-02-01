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
		
		private static float xComponent = 0.0f;
		private static float yComponent = 0.0f;
		private static Vector2 gravityVector = new Vector2(0.0f, -1.0f); //The direction in which gravity is currently going
		private static SpriteUV	gravityArrow;
		private static Vector2 playerDirection; //Based on the rotation of the maze this is the direction the player is moving
		private static float   cameraRotation; //The rotation of the camera as a angle
		private static float   gravityVelocity; //Gravity as a angle
		private static bool reverseGravity = false;
		
		private static TouchStatus	currentTouchStatus;
		private static Vector2 oldTouchPos = new Vector2( 0.0f, 0.0f );
		private static Vector2 newTouchPos = new Vector2( 0.0f, 0.0f );
				
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
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{		
			CheckInput();
			player.Update(gravityVector);
			UpdateCamera();
			CheckCollisions();
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
					newTouchPos = new Vector2( data.X, data.Y );
				}
				
				if(data.Status.Equals(TouchStatus.Up))
				{
					if((oldTouchPos.Y - newTouchPos.Y) > 0.30f) // Swipe Upwards
					{
						gravityVector = new Vector2(-gravityVector.X, -gravityVector.Y);
					}
				}
				
				currentTouchStatus = data.Status;
			}		
			
			// Old Gesture Code	RMDS
			//FlickGestureDetector flickDetector = new FlickGestureDetector();
			//
			//if(flickDetector.Direction == FlickDirection.Vertical)	
			//	gravityVector = new Vector2(-gravityVector.X, -gravityVector.Y);		
			//	
			//if(GestureEventArgs.Equals())
			//{
			//}
			
			
			gravityArrow.Position = new Vector2(player.GetPos().X + 300.0f, player.GetPos().Y);
			gravityArrow.Angle = (xComponent * 0.5f) * FMath.PI;
			
			xComponent += gamePadData.AnalogLeftX/100.0f;
			
			if(yComponent < 0.0f)
				yComponent += gamePadData.AnalogLeftX/100.0f;
			else
				yComponent += gamePadData.AnalogLeftX/-100.0f;
			
			
			if (xComponent > 1.0f)
				xComponent = 1.0f;
			else if (xComponent < -1.0f)
					xComponent = -1.0f;
			else{}
			
			if(yComponent < -1.0f)
				yComponent = -1.0f;
			else if (yComponent > 1.0f)
					yComponent = 1.0f;
			else{}
			
			gravityVector = new Vector2(xComponent, -1.0f + (2 * yComponent));
			
			if((gamePadData.Buttons & GamePadButtons.Cross) != 0)
				reverseGravity = !reverseGravity;
			
			//if(reverseGravity)
			//	gravityVector = new Vector2(gravityVector.X, -gravityVector.Y);
			
			
			//if()
			//	gravityVector = new Vector2(-gravityVector.X, -gravityVector.Y);
			
			
			//if (Input2.GamePad0.Up.Down)
			//{
			//	cameraRotation += 2.5f;
			//	//gravityVector = new Vector2(FMath.Cos (cameraRotation), FMath.Sin (cameraRotation)); Needs to be in radians
			//}
		}
		
		//Camera. Focus on player. Don't let the camera show any off map area. If the player walks near the edge
		//leave the edge of the camera on the edge of the map but let the player walk to the actual map edge.
		//If the player isn't within screenwidth/2 or screen height/2 of a edge of the map then center on the
		//player.
		public static void UpdateCamera() //@AS (max width and max height are currently unknown so set to 2000)
		{
				if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.3f) || (player.GetX() > 2000f - Director.Instance.GL.Context.GetViewport().Width*0.3f) ||
			   (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.3f) || (player.GetY() > 2000f - Director.Instance.GL.Context.GetViewport().Height*0.3f))
			{
				if (player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.4f) //Near left side
					gameScene.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f, player.GetY()));
				if (player.GetX() > 2000f - Director.Instance.GL.Context.GetViewport().Width*0.4f) //Near right side
					gameScene.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(2000f - Director.Instance.GL.Context.GetViewport().Width*0.4f, player.GetY()));
				if (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.4f) //Near bottom side
					gameScene.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(player.GetX(), Director.Instance.GL.Context.GetViewport().Height*0.4f));
				if (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f) //Near top side
					gameScene.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(player.GetX(), 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f));
				if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.4f) && (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.4f)) //Near bottom left corner
					gameScene.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f, Director.Instance.GL.Context.GetViewport().Height*0.4f));
				if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.4f) && (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f)) //Near top left corner
					gameScene.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f, 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f));
				if ((player.GetX() > 2000.0f - Director.Instance.GL.Context.GetViewport().Width*0.4f) && (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f)) //Near top right corner
					gameScene.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(2000.0f - Director.Instance.GL.Context.GetViewport().Width*0.4f, 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f));
				if ((player.GetX() > 2000.0f -  Director.Instance.GL.Context.GetViewport().Width*0.4f) && (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.4f)) //Near bottom right corner
					gameScene.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f, Director.Instance.GL.Context.GetViewport().Height*0.4f));
			}
			else
				gameScene.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), player.GetPos()); //Player not near an edge
		}
		
		public static void CheckCollisions() //@AS
		{	
		//if(player.GetX() % 50 == 0 || player.GetY() % 50 == 0) //If the player hits the side of the a tile
		//{													     //check if its a map boundary
			if(maze.HasCollidedWithPlayer(player.Sprite))
			{
				player.SetFalling(false);
				player.SetPos(player.GetPos() - player.GetDirection());
			}
			else
			{
				player.SetFalling(true);
				
			}
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
