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
		private Vector2 gravityVector; //The direction in which gravity is currently going
		private Vector2 playerDirection; //Based on the rotation of the camera this is the direction the player is moving
		private float cameraRotation; //The rotation of the camera
		private float gravityVelocity; 
		
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
			player.Update(playerDirection);
			UpdateCamera();
			CheckCollisions();
		}
		
		public void CheckInput()
		{
			//Query gamepad for current state
			var gamePadData = GamePad.GetData(0);
		}
		
		//Camera. Focus on player. Don't let the camera show any off map area. If the player walks near the edge
		//leave the edge of the camera on the edge of the map but let the player walk to the actual map edge.
		//If the player isn't within screenwidth/2 or screen height/2 of a edge of the map then center on the
		//player.
		public void UpdateCamera() //@AS (max width and max height are currently unknown so set to 2000)
		{
			if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.5f) || (player.GetX() > 2000f - Director.Instance.GL.Context.GetViewport().Width*0.5f) ||
					    (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.5f) || (player.GetY() > 2000f - Director.Instance.GL.Context.GetViewport().Height*0.5f))
					{
						if (player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.5f) //Near left side
							this.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f, player.GetY()));
						if (player.GetX() > 2000f - Director.Instance.GL.Context.GetViewport().Width*0.5f) //Near right side
							this.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(2000f - Director.Instance.GL.Context.GetViewport().Width*0.5f, player.GetY()));
						if (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.5f) //Near bottom side
							this.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), new Vector2(player.GetX(), Director.Instance.GL.Context.GetViewport().Height*0.5f));
						if (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f) //Near top side
							this.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), 
							                            new Vector2(player.GetX(), 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f));
						if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.5f) && (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.5f)) //Near bottom left corner
							this.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
							                            new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f, Director.Instance.GL.Context.GetViewport().Height*0.5f));
						if ((player.GetX() < Director.Instance.GL.Context.GetViewport().Width*0.5f) && (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f)) //Near top left corner
							this.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
							                            new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f, 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f));
						if ((player.GetX() > 2000.0f - Director.Instance.GL.Context.GetViewport().Width*0.5f) && (player.GetY() > 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f)) //Near top right corner
							this.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
							                            new Vector2(2000.0f - Director.Instance.GL.Context.GetViewport().Width*0.5f, 2000.0f - Director.Instance.GL.Context.GetViewport().Height*0.5f));
						if ((player.GetX() > 2000.0f -  Director.Instance.GL.Context.GetViewport().Width*0.5f) && (player.GetY() < Director.Instance.GL.Context.GetViewport().Height*0.5f)) //Near bottom right corner
							this.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
							                            new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f, Director.Instance.GL.Context.GetViewport().Height*0.5f));
					}
					else
						this.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f), player.GetPos()); //Player not near an edge
		}
		
		public void CheckCollisions() //@AS
		{	if(player.GetX() % 50 == 0 || player.GetY() % 50 == 0) //If the player hits the side of the a tile
			{													   //check if its a map boundary
				if(maze.HasCollidedWithPlayer(player.Sprite))
				{
					//TODO: Invert the direction vector moving him back one space
				}
			}
			
		}
	}
}

	
