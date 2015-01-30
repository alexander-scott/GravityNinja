using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravDuck
{
	public class GameScene : Scene
	{
		private static TouchStatus currentTouchStatus; //Will be needed for our touch gestures
		
		private Player	player; //Our player, maze and background
		private Maze maze;
		private Background background;
		private Vector2 gravityVector = new Vector2(0.0f, -1.0f); //The direction in which gravity is currently going
		private Vector2 playerDirection; //Based on the rotation of the maze this is the direction the player is moving
		private float cameraRotation; //The rotation of the camera as a angle
		private float gravityVelocity; //Gravity as a angle
		
		public GameScene()
		{
			this.Camera.SetViewFromViewport();						
			
			//Background
			background = new Background(this);
			
			//Player
			player = new Player(this);	
			
			//Maze
			maze = new Maze(this);
		}
		
		public void Dispose()
		{
			//TODO: Add dispose st00f
		}
	
		public void Update()
		{
			CheckInput();
			player.Update(gravityVector);
			UpdateCamera();
			CheckCollisions();
		}
		
		public void CheckInput()
		{
			//Query gamepad for current state
			var gamePadData = GamePad.GetData(0);
			GamePadData data = GamePad.GetData(0);
			
			if (Input2.GamePad0.Up.Down)
			{
				cameraRotation += 2.5f;
				//gravityVector = new Vector2(FMath.Cos (cameraRotation), FMath.Sin (cameraRotation)); Needs to be in radians
			}
		}
		
		//Camera. Focus on player. Don't let the camera show any off map area. If the player walks near the edge
		//leave the edge of the camera on the edge of the map but let the player walk to the actual map edge.
		//If the player isn't within screenwidth/2 or screen height/2 of a edge of the map then center on the
		//player.
		public void UpdateCamera() //@AS (max width and max height are currently unknown so set to 2000)
		{
				if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.3f) || (player.GetX() > 2000f - Director.Instance.GL.Context.GetViewport().Width*0.3f) ||
			   (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.3f) || (player.GetY() > 2000f - Director.Instance.GL.Context.GetViewport().Height*0.3f))
			{
				if (player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.4f) //Near left side
					this.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f, player.GetY()));
				if (player.GetX() > 2000f - Director.Instance.GL.Context.GetViewport().Width*0.4f) //Near right side
					this.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(2000f - Director.Instance.GL.Context.GetViewport().Width*0.4f, player.GetY()));
				if (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.4f) //Near bottom side
					this.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(player.GetX(), Director.Instance.GL.Context.GetViewport().Height*0.4f));
				if (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f) //Near top side
					this.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(player.GetX(), 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f));
				if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.4f) && (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.4f)) //Near bottom left corner
					this.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f, Director.Instance.GL.Context.GetViewport().Height*0.4f));
				if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.4f) && (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f)) //Near top left corner
					this.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f, 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f));
				if ((player.GetX() > 2000.0f - Director.Instance.GL.Context.GetViewport().Width*0.4f) && (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f)) //Near top right corner
					this.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(2000.0f - Director.Instance.GL.Context.GetViewport().Width*0.4f, 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.4f));
				if ((player.GetX() > 2000.0f -  Director.Instance.GL.Context.GetViewport().Width*0.4f) && (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.4f)) //Near bottom right corner
					this.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.4f, Director.Instance.GL.Context.GetViewport().Height*0.4f));
			}
			else
				this.Camera2D.SetViewY(new Vector2(cameraRotation,Director.Instance.GL.Context.GetViewport().Height*0.5f), player.GetPos()); //Player not near an edge
		}
		
		public void CheckCollisions() //@AS
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
		        vector.Normalize();
		    return vector;
		}
		
	}
}

	
