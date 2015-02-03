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
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene; //Create the scene

		private static Background background; //Create the background
		private static Maze maze; //Create the maze
		private static Player player; //Create the player
		
		private static Vector2 gravityVector = new Vector2(0.0f, -1.0f); //The direction in which gravity is currently going
		private static Vector2 playerDirection; //Based on the rotation of the maze this is the direction the player is moving
		private static float   cameraRotation = 1; //The rotation of the camera as a angle
		private static float   gravityVelocity; //Gravity as a angle
				
		public static void Main (string[] args)
		{
			Initialize();
			
			//Game loop
			bool quitGame = false;
			while (!quitGame) 
			{
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
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{
			//Determine whether the player tapped the screen
			var touches = Touch.GetData(0);
			
			CheckInput();
			player.Update(gravityVector);
			UpdateCamera();
			CheckCollisions();
		}
		
		public static void CheckInput() //@AS
		{
			//Query gamepad for current state
			var gamePadData = GamePad.GetData(0);
			GamePadData data = GamePad.GetData(0);
			
			if (Input2.GamePad0.Up.Down) //Rotates the camera to the right
			{
				cameraRotation += 1f;
				gravityVector = new Vector2(gravityVector.X - 0.01f, gravityVector.Y); //Make sure we change gravity
			}
			
			if (Input2.GamePad0.Down.Down)
			{
				cameraRotation -= 1f;
				gravityVector = new Vector2(gravityVector.X+ 0.01f, gravityVector.Y);
			}
			
			if (Input2.GamePad0.Cross.Down) //Resets bird
			{
				player.SetPos(new Vector2(100.0f, 700.0f));
				cameraRotation = 0f;
			}
		}
		
		//Camera. Focus on player. Don't let the camera show any off map area. If the player walks near the edge
		//leave the edge of the camera on the edge of the map but let the player walk to the actual map edge.
		//If the player isn't within screenwidth/2 or screen height/2 of a edge of the map then center on the
		//player.
		public static void UpdateCamera() //@AS (max width and max height are currently unknown so set to 2000)
		{
				if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.4f) || (player.GetX() > 2000f - Director.Instance.GL.Context.GetViewport().Width*0.4f) ||
			   (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.4f) || (player.GetY() > 2000f - Director.Instance.GL.Context.GetViewport().Height*0.4f))
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
			if(maze.CheckCollision(player.Sprite)) //If the player is on a tile
			{	//Check what direction the tile is to the player and move the player in the opposite direction
				player.SetPos(player.GetPos() - maze.HasCollidedWithPlayer(player.Sprite) + new Vector2(0.0f, 0.00001f));
				player.SetFalling(false); //If the bird is touching a tile it's not falling
			}
			else
			{
				player.SetFalling(true); //Bird is falling if it's not touching a tile
			}
		
		}
		
		public static Vector2 Vector2FromAngle(float angle, bool normalize = true) //Returns vector2 from a float
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
