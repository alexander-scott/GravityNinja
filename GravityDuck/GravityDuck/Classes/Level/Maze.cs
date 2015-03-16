using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	//Our Maze class V2 by @AW && @AS
	public class Maze
	{
		private LevelLoader level;
		
		private Coin[] coins;
		private Gem[] gems;
		
		private Spikes[] spikes;
		
		private SpriteUV[,] sprites;
		
		private LevelFlag levelFlag;
		private bool levelFinished;
						
		public Maze (Scene scene)
		{
			level = new LevelLoader();
			string filePath = "/Application/tiledMapRU.tmx";
			XMLLoader.LoadLevel(level, filePath);
			
			// Obstacles
			spikes = level.LoadInSpikes(scene);
			
			// Level
			sprites = level.LoadInLevel(scene);
			
			// Collectables
			coins = level.LoadInCoins(scene);
			gems = level.LoadInGems(scene);
			
			levelFlag = level.LoadInFlag(scene);
			
			levelFinished = false;
		}
		
		public void Dispose() //Dispose texture data
		{
		}
		
		public bool HasCollidedWithPlayer(Bounds2 player) //Check if the a sprite has hit a part of the maze
		{
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = spri.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				
				if (player.Overlaps(mazeTile)) //Return true if the player overlaps with the maze
				{
				   return true;
				}	
			}
			return false;
	
		}
		
		public bool HasHitSide(Bounds2 player, int gravity) //Checks if the player has hit the side of the maze and not the floor
		{
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = spri.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				if (player.Overlaps(mazeTile)) //Return true if the player overlaps with the maze
				{
					if (checkSides(player, spri, gravity)) //Return true if the player has come into contact with a side
					{
					   return true;
					}
				}
			}
			return false;
		}
		
		public bool checkSides(Bounds2 player, SpriteUV sprite2, int gravity) //ComparrightRotation = false;es 2 sprites to see is the left or right side has intersected
		{
			Bounds2 mazeTile = sprite2.GetlContentLocalBounds();
			sprite2.GetContentWorldBounds(ref mazeTile); 
			if (gravity == 1) //Down
			{
				if (((player.Point01.X < mazeTile.Point11.X) || (player.Point11.X > mazeTile.Point01.X)))
				{ //If the left side of the player is past the right side of the tile and vica versa
					if ((player.Point01.Y) < mazeTile.Point01.Y) //If the tile is above the player
						return true;	
					else
						return false;
				}
				else
						return false;
			}
			else if (gravity == 2) // Right
			{
				if (((player.Point10.Y < mazeTile.Point10.Y) || (player.Point10.Y > mazeTile.Point11.Y)))
				{ //If the left side of the player is past the right side of the tile and vica versa
					if ((player.Point11.X) < mazeTile.Point11.X) //If the tile is above the player
						return true;	
					else
						return false;
				}
				else
						return false;
			}
			else if (gravity == 3) // Up
			{
				if (((player.Point10.X < mazeTile.Point00.X) || (player.Point00.X > mazeTile.Point10.X)))
				{ //If the left side of the player is past the right side of the tile and vica versa
					if ((player.Point10.Y) < mazeTile.Point10.Y) //If the tile is above the player
						return true;
					else
						return false;
				}
				else
						return false;
			}
			else if (gravity == 4) // Left
			{
				if (((player.Point10.Y < mazeTile.Point10.Y) || (player.Point10.Y > mazeTile.Point11.Y)))
				{ //If the left side of the player is past the right side of the tile and vica versa
					if ((player.Point11.X) < mazeTile.Point11.X) //If the tile is above the player
						return true;	
					else
						return false;
				}
				else
						return false;
			}
			else
				return false;
				
			}
		
		//Check Collectable Collisions
		public int CheckCollectableCollision(SpriteUV sprite, Scene scene)
		{
			foreach(Coin coin in coins)
			{
				bool collide = coin.HasCollidedWithPlayer(sprite);
				
				if (collide)
				{
					int scoreValue = coin.Collected(scene);
					return scoreValue;
				}
			}
			
			foreach(Gem gem in gems)
			{
				bool collide = gem.HasCollidedWithPlayer(sprite);
				
				if (collide)
				{
					int scoreValue = gem.Collected(scene);
					return scoreValue;
				}
			}
			//If no collisions occur
			return 0;
		}
		
		//Check Obstacle Collisions
		public bool CheckObstacleCollisions(SpriteUV sprite)
		{
			foreach(Spikes spike in spikes)
			{
				bool collide = spike.HasCollidedWithPlayer(sprite);
				
				if (collide)
				{
				   return true;
				}
			}
			return false;
		}
		
		//Check Level Flag Collisions
		public bool CheckFlagCollision(SpriteUV sprite)
		{
			bool collide = levelFlag.HasCollidedWithPlayer(sprite);
				
			if (collide)
			{
				return true;
			}
			return false;
		}
		
		//Returns Level Complete Boolean
		public bool IsLevelComplete()
		{
			return levelFinished;
		}
		
		//Set Level Complete Boolean
		public void SetLevelFinished(bool newLevelFinished)
		{
			levelFinished = newLevelFinished;
		}
	}
}