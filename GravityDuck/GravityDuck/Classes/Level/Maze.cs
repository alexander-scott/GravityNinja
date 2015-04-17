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
		public static Vector2 spawnPoint;
		public static int overallLevelScore;
		
		// Level Loader
		private LevelLoader level;
		
		// Collectables
		private Coin[] coins;
		private Gem[] gems;
		private LevelFlag levelFlag;
		
		// Obstacles
		private BlackHole[] blackHoles;
		private BreakableWall[] breakableWalls;
		private LaserGate[] laserGates;
		private Spikes[] spikes;
		private WindTunnel[] windTunnels;
		
		// Level
		private SpriteUV[,] sprites;
		
		private bool levelFinished = true;
						
		public Maze (Scene scene, int currentLevel)
		{
			LoadLevel(scene, currentLevel);
		}
		
		public void LoadLevel(Scene scene, int levelNum)
		{
			string levelNumStr = Convert.ToString(levelNum);
			
			// Load In Level
			level = new LevelLoader();
			string filePath = "/Application/maps/level" + levelNumStr + ".tmx";
			XMLLoader.LoadLevel(level, filePath);
			
			// Load In Objects
			
			spikes = level.LoadInSpikes(scene);
			
			// Level
			sprites = level.LoadInLevel(scene);
				
			// Obstacles
			blackHoles = level.LoadInBlackHoles(scene);
			breakableWalls = level.LoadInBreakableWalls(scene);
			laserGates = level.LoadInLaserGates(scene);
			windTunnels = level.LoadInWindTunnels(scene);
			
			// Collectables
			coins = level.LoadInCoins(scene);
			gems = level.LoadInGems(scene);
			
			//Player Position
			spawnPoint = level.playerPos;
			
			levelFlag = level.LoadInFlag(scene);
			
			overallLevelScore = level.CalculateOverallLevelScore();
		}
		
		public void RemoveLevel() //Dispose texture data
		{
			levelFlag.HideSprite();
			
			foreach(SpriteUV spritess in sprites)
				spritess.Visible = false;
			level.Dispose();				
			level = null;
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
				if (((player.Point10.Y < mazeTile.Point11.Y) || (player.Point10.Y > mazeTile.Point11.Y)))
				{ //If the left side of the player is past the right side of the tile and vica versa
					if ((player.Point00.X) > mazeTile.Point00.X) //If the tile is above the player
						return true;	
					else
						return false;
				}
				else
						return false;
			}
			else if (gravity == 3) // Up
			{
				//if (((player.Point10.X < mazeTile.Point00.X) || (player.Point00.X > mazeTile.Point10.X)))
				if (((player.Point01.X < mazeTile.Point11.X) || (player.Point11.X > mazeTile.Point01.X)))
				{ //If the left side of the player is past the right side of the tile and vica versa
					if ((player.Point10.Y) > mazeTile.Point10.Y) //If the tile is above the player
						return true;
					else
						return false;
				}
				else
						return false;
			}
			else if (gravity == 4) // Left
			{
				if (((player.Point10.Y < mazeTile.Point11.Y) || (player.Point10.Y > mazeTile.Point11.Y)))
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
			if (spikes != null)
			{
				foreach(Spikes spike in spikes)
				{
					bool collide = spike.HasCollidedWithPlayer(sprite);
					
					if (collide)
					{
					   return true;
					}
				}
			}
			if (blackHoles != null)
			{
				foreach(BlackHole bh in blackHoles)
				{
					bool collide = bh.HasCollidedWithPlayer(sprite);
					
					if (collide)
					{
					   return true;
					}
				}
			}
			
			if (laserGates != null)
			{
				foreach(LaserGate lg in laserGates)
				{
					bool collide = lg.HasCollidedWithPlayer(sprite);
					
					if (collide)
					{
					   return true;
					}
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
		
		public Vector2 GetSpawnPoint()
		{
			return spawnPoint;
		}
		
		public int GetOverallLevelScore()
		{
			return overallLevelScore;
		}
		
		//Set Level Complete Boolean
		public void SetLevelFinished(bool newLevelFinished)
		{
			levelFinished = newLevelFinished;
		}
		
		// Check collision between player and the wind force exerted by the Wind Tunnels	RMDS
		public Vector2 CheckWindTunnel(Player player)
		{
			Vector2 force = new Vector2(0.0f, 0.0f);
			
			if(windTunnels != null)
			{
				foreach(WindTunnel windTunnel in windTunnels)
				{
					if(windTunnel.CheckPlayerPos(player))
					{
						force = windTunnel.CalculateForce(player);
					}
				}
			}
		
			return force;
		}
		
		// Check collision between player and Black Hole gravitational pulls	RMDS
		public Vector2 CheckBlackHole(Player player)
		{
			Vector2 force = new Vector2(0.0f, 0.0f);
			
			if(blackHoles != null)
			{
				foreach (BlackHole blackHole in blackHoles)
				{
					if(blackHole.CheckPlayerPos(player))
					{
						force = blackHole.CalculateForce(player);
					}
				}
			}
			
			return force;
		}
		
		// Check collision between player and Laser Gates	RMDS
		public bool CheckLaserGates(Player player)
		{
			if(laserGates != null)
			{
				foreach (LaserGate laserGate in laserGates)
				{
					if(laserGate.CheckPlayerPos(player))
					{	
						return true;
					}
				}
			}
			
			return false;
		}
		
		public bool CheckBreakableWalls(Player player)
		{
			if(breakableWalls != null)
			{
				foreach (BreakableWall breakableWall in breakableWalls)
				{
					if(breakableWall.HasCollidedWithPlayer(player.Sprite))
					{
						return breakableWall.CheckIfBreak(player.GetMomentum());
					}
				}
			}
			
			return false;
		}	
	}
}
