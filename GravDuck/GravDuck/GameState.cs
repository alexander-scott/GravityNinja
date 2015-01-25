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
		private static TouchStatus currentTouchStatus; //Will be need for our touch gestures
		
		private Player	player; //Our maze, player and background
		private Maze maze;
		private Background background;
	
		// Public
		public GameScene()
		{
			this.Camera.SetViewFromViewport();						
			
			//Setup all our entities
			//Background
			background = new Background(this);
			
			//Player
			player = new Player(this);	
			
			//Maze
			maze = new Maze(this);
					
			
		}
		
		public void Dispose()
		{
			
		}
		
		public void SetUpSceneObjects()
		{
		}

		public void Update()
		{
			player.Update();
			CheckInput();
		}
		
		public void CheckInput()
		{
			//Query gamepad for current state
			var gamePadData = GamePad.GetData(0);
		}
		
		public void CheckCollisions()
		{	
			
		}
		
		public void Reset()
		{         			          
		}
	
	}
}

	
