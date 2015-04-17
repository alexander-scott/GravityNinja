using System;

using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class LevelLoader
	{
		private int width;
		private int height;
		private float windowHeight;
		
		// List of level tiles
		private List<int> level;
		
		// Collectable Lists
		private List<Coin> coins;
		private List<Gem> gems;
		private LevelFlag levelFlag;
		
		// Obstacle lists
		private List<BlackHole> blackHoles;
		private List<BreakableWall> breakableWalls;
		private List<LaserGate> laserGates;
		private List<Spikes> spikes;
		private List<WindTunnel> windTunnels;
		
		public Vector2 playerPos;
		
		private Random rnd;
		
		public LevelLoader ()
		{
			// Level
			level = new List<int>();
			
			// Collectables
			coins = new List<Coin>();
			gems = new List<Gem>();
			
			// Obstacles
			blackHoles = new List<BlackHole>();
			breakableWalls = new List<BreakableWall>();
			laserGates = new List<LaserGate>();
			spikes = new List<Spikes>();
			windTunnels = new List<WindTunnel>();
			
			rnd = new Random();
		}
		
		public void SetWidth(int width)
		{
			this.width = width;	
		}
		
		public int GetWidth()
		{
			return width;	
		}
		
		public void SetHeight(int height)
		{
			this.height = height;	
			windowHeight = height * 142.0f;
		}
		
		public int GetHeight()
		{
			return height;	
		}
		
		public float GetLevelHeight()
		{
			return windowHeight;
		}
		
		public void AddObject(string type, float x, float y, float rotation)
		{
			switch(type)
			{
				// Collectables
				case "Coin":
					Coin coin = new Coin();
					coin.setPosition(new Vector2(x, y));
					coin.setRotation(-rotation);
					coins.Add (coin);
				break;
				
				case "Gem":
					Gem gem = new Gem();
					gem.setPosition(new Vector2(x, y));
					gem.setRotation(-rotation);
					gems.Add (gem);
				break;
				
				// Obstacles
				case "Small Spikes":
					Spikes smallSpikes = new Spikes(1);
				
					if(rotation == 0)
					{
						smallSpikes.setPosition(new Vector2(x, y - 100.0f));
					}
					else
					{
						smallSpikes.setPosition(new Vector2(x - 220.0f, y - 165.0f));
					}
					
					//rotation += 270;
					smallSpikes.setRotation(-rotation);
					spikes.Add(smallSpikes);
				break;
				
				case "Large Spikes":
					Spikes largeSpikes = new Spikes(2);
				
					if(rotation == 0)
					{
						largeSpikes.setPosition(new Vector2(x, y - 100.0f));
					}
					else
					{
						largeSpikes.setPosition(new Vector2(x - 240.0f, y - 210.0f));
					}
				
					//rotation += 270;
					largeSpikes.setRotation(-rotation);
					spikes.Add(largeSpikes);
				break;
				
				case "Black Hole":
					BlackHole blackHole = new BlackHole();
				
					if(rotation == 0)
					{
						blackHole.setPosition(new Vector2(x, y));
					}
					else
					{
						blackHole.setPosition(new Vector2(x, y));
					}
				
					blackHole.setRotation(-rotation);
					blackHole.setDirection((int) rotation);
					blackHoles.Add(blackHole);
				break;
				
				case "Breakable Wall":
					BreakableWall breakableWall = new BreakableWall();
				
					if(rotation == 0)
					{
						breakableWall.setPosition(new Vector2(x, y));
					}
					else
					{
						breakableWall.setPosition(new Vector2(x, y));
					}
				
					breakableWall.setRotation(-rotation);
					breakableWall.setDirection((int) rotation);
					breakableWalls.Add(breakableWall);
				break;
				
				case "Laser Gate":
					LaserGate laserGate = new LaserGate(rnd.Next(0,18));
				
					if(rotation == 0)
					{
						laserGate.setPosition(new Vector2(x, y));
					}
					else
					{
						laserGate.setPosition(new Vector2(x, y));
					}
				
					laserGate.setRotation(-rotation);
					laserGate.setDirection((int) rotation);
					laserGates.Add(laserGate);
				break;
				
				case "Wind Tunnel":
					WindTunnel windTunnel = new WindTunnel();
				
					if(rotation == 0)
					{
						windTunnel.setPosition(new Vector2(x, y));
					}
					else
					{
						windTunnel.setPosition(new Vector2(x, y));
					}
				
					windTunnel.setRotation(-rotation);
					windTunnel.setDirection((int) rotation);
					windTunnels.Add(windTunnel);
				break;
				
				// Level Flag
				case "Level":
					levelFlag = new LevelFlag();
					levelFlag.setPosition(new Vector2(x, y - 80.0f));
					
					levelFlag.setRotation(-rotation);
				break;
				
				// Player Position
				case "Player":
					playerPos = new Vector2(x, y);
				break;
			}
		}
		
		public void AddTile(int tileType)
		{
			level.Add(tileType);
		}
		
		public SpriteUV[,] LoadInLevel(Scene scene)
		{
			SpriteUV[,] sprites = new SpriteUV[height, width];
			
			int index = -1;
			
			List<int> levelCorrected = new List<int>();
			
			for(int i = 0; i < level.Count; i++)
			{
				int tile = level[level.Count - i - 1];
				levelCorrected.Add(tile);
			}
			
			level = levelCorrected;
			
			for (int i = 0; i < height; i++)
			{
				for(int ii = width - 1; ii >= 0; ii--)
				{
					index += 1;
					int test = level[index];
					
					switch(level[index])
					{
						case 1:
							SpriteUV spriteGB1 = TileManager.GetTileType("GroundBlock1");
							sprites[i,ii] = new SpriteUV(spriteGB1.TextureInfo);
							sprites[i,ii].Quad.S = spriteGB1.Quad.S;
							sprites[i,ii].Position = new Vector2(128.0f*ii, 142.0f*i);
							scene.AddChild(sprites[i,ii]);
						break;
						
						case 2:
							SpriteUV spriteGB2 = TileManager.GetTileType("GroundBlock2");
							sprites[i,ii] = new SpriteUV(spriteGB2.TextureInfo);
							sprites[i,ii].Quad.S = spriteGB2.Quad.S;
							sprites[i,ii].Position = new Vector2(128.0f*ii, 142.0f*i);
							scene.AddChild(sprites[i,ii]);
						break;
						
						case 3:
							SpriteUV spriteCB1 = TileManager.GetTileType("CeilingBlock1");
							sprites[i,ii] = new SpriteUV(spriteCB1.TextureInfo);
							sprites[i,ii].Quad.S = spriteCB1.Quad.S;
							sprites[i,ii].Position = new Vector2(128.0f*ii, 142.0f*i);
							scene.AddChild(sprites[i,ii]);
						break;
						
						case 4:
							SpriteUV spriteCB2 = TileManager.GetTileType("CeilingBlock2");
							sprites[i,ii] = new SpriteUV(spriteCB2.TextureInfo);
							sprites[i,ii].Quad.S = spriteCB2.Quad.S;
							sprites[i,ii].Position = new Vector2(128.0f*ii, 142.0f*i);
							scene.AddChild(sprites[i,ii]);
						break;
						
						case 5:
							SpriteUV spriteTB = TileManager.GetTileType("TreeBlock");
							sprites[i,ii] = new SpriteUV(spriteTB.TextureInfo);
							sprites[i,ii].Quad.S = spriteTB.Quad.S;
							sprites[i,ii].Position = new Vector2(128.0f*ii, 142.0f*i);
							scene.AddChild(sprites[i,ii]);
						break;
						
						case 6:
							SpriteUV spriteLB1 = TileManager.GetTileType("LeftBlock1");
							sprites[i,ii] = new SpriteUV(spriteLB1.TextureInfo);
							sprites[i,ii].Quad.S = spriteLB1.Quad.S;
							sprites[i,ii].Position = new Vector2(128.0f*ii, 142.0f*i);
							scene.AddChild(sprites[i,ii]);
						break;
						
						case 7:
							SpriteUV spriteLB2 = TileManager.GetTileType("LeftBlock2");
							sprites[i,ii] = new SpriteUV(spriteLB2.TextureInfo);
							sprites[i,ii].Quad.S = spriteLB2.Quad.S;
							sprites[i,ii].Position = new Vector2(128.0f*ii, 142.0f*i);
							scene.AddChild(sprites[i,ii]);
						break;
						
						case 8:
							SpriteUV spriteRB1 = TileManager.GetTileType("RightBlock1");
							sprites[i,ii] = new SpriteUV(spriteRB1.TextureInfo);
							sprites[i,ii].Quad.S = spriteRB1.Quad.S;
							sprites[i,ii].Position = new Vector2(128.0f*ii, 142.0f*i);
							scene.AddChild(sprites[i,ii]);
						break;
						
						case 9:
							SpriteUV spriteRB2 = TileManager.GetTileType("RightBlock2");
							sprites[i,ii] = new SpriteUV(spriteRB2.TextureInfo);
							sprites[i,ii].Quad.S = spriteRB2.Quad.S;
							sprites[i,ii].Position = new Vector2(128.0f*ii, 142.0f*i);
							scene.AddChild(sprites[i,ii]);
						break;
						
						case 0:
							sprites[i,ii] = new SpriteUV(null);
						break;
					}
				}
			}
			
			return sprites;
		}
		
		public void Dispose()
		{
			if (coins != null)
			{
				foreach(Coin theCoin in coins)
					theCoin.HideSprite();
			}
			if (gems != null)
			{
				foreach(Gem theGem in gems)
					theGem.HideSprite();
			}
			if (spikes != null)
			{
				foreach(Spikes s in spikes)
					s.HideSprite();
			}
			if (breakableWalls != null)
			{
				foreach(BreakableWall bw in breakableWalls)
					bw.HideSprite();
			}
			if (blackHoles != null)
			{
				foreach(BlackHole bh in blackHoles)
					bh.HideSprite();
			}
			if (laserGates != null)
			{
				foreach(LaserGate lg in laserGates)
					lg.HideSprite();
			}
			if (windTunnels != null)
			{
				foreach(WindTunnel wt in windTunnels)
					wt.HideSprite();
			}
			level.Clear();
			coins.Clear();
			gems.Clear();
			levelFlag.Dispose();
			blackHoles.Clear();
			breakableWalls.Clear();
			laserGates.Clear();
			spikes.Clear();
			windTunnels.Clear();
			
			foreach(int num in level)
				level.RemoveAt(num);

			level = null;
			coins = null;
			gems = null;
			levelFlag = null;
			blackHoles = null;
			breakableWalls = null;
			laserGates = null;
			spikes = null;
			windTunnels = null;
		}
		
		public Coin[] LoadInCoins(Scene scene)
		{
			Coin[] coinsObj = new Coin[coins.Count];
			
			for(int i = 0; i < coins.Count; i++)
			{
				coinsObj[i] = coins[i];
				scene.AddChild(coinsObj[i].GetSprite());
			}
			
			if(coins.Count != 0)
			{
				return coinsObj;
			}
			else
			{
				return null;
			}
		}
		
		public Gem[] LoadInGems(Scene scene)
		{
			Gem[] gemsObj = new Gem[gems.Count];
			
			for(int i = 0; i < gems.Count; i++)
			{
				gemsObj[i] = gems[i];
				scene.AddChild(gemsObj[i].GetSprite());
			}
			
			if(gems.Count != 0)
			{
				return gemsObj;
			}
			else
			{
				return null;
			}
		}
		
		public BlackHole[] LoadInBlackHoles(Scene scene)
		{
			BlackHole[] blackHolesObj = new BlackHole[blackHoles.Count];
			
			for (int i = 0; i < blackHoles.Count; i++)
			{
				blackHolesObj[i] = blackHoles[i];
				scene.AddChild(blackHolesObj[i].getSprite());
			}
			
			if(blackHoles.Count != 0)
			{
				return blackHolesObj;
			}
			else
			{
				return null;
			}
		}
		
		public BreakableWall[] LoadInBreakableWalls(Scene scene)
		{
			BreakableWall[] breakableWallsObj = new BreakableWall[breakableWalls.Count];
			
			for (int i = 0; i < breakableWalls.Count; i++)
			{
				breakableWallsObj[i] = breakableWalls[i];
				scene.AddChild(breakableWallsObj[i].GetSprite());
			}
			
			if(blackHoles.Count != 0)
			{
				return breakableWallsObj;
			}
			else
			{
				return null;
			}
		}
		
		public LaserGate[] LoadInLaserGates(Scene scene)
		{
			LaserGate[] laserGatesObj = new LaserGate[laserGates.Count];
			
			for (int i = 0; i < laserGates.Count; i++)
			{
				laserGatesObj[i] = laserGates[i];
				scene.AddChild(laserGatesObj[i].getSprite());
			}
			
			if(laserGates.Count != 0)
			{
				return laserGatesObj;
			}
			else
			{
				return null;
			}
		}
		
		public Spikes[] LoadInSpikes(Scene scene)
		{
			Spikes[] spikesObj = new Spikes[spikes.Count];
			
			for (int i = 0; i < spikes.Count; i++)
			{
				spikesObj[i] = spikes[i];
				scene.AddChild(spikesObj[i].GetSprite());
			}
			
			if(spikes.Count != 0)
			{
				return spikesObj;
			}
			else
			{
				return null;
			}
		}
		
		public WindTunnel[] LoadInWindTunnels(Scene scene)
		{
			WindTunnel[] windTunnelsObj = new WindTunnel[windTunnels.Count];
			
			for (int i = 0; i < windTunnels.Count; i++)
			{
				windTunnelsObj[i] = windTunnels[i];
				scene.AddChild(windTunnelsObj[i].getSprite());
			}
			
			if(windTunnels.Count != 0)
			{
				return windTunnelsObj;
			}
			else
			{
				return null;
			}
		}
		
		public LevelFlag LoadInFlag(Scene scene)
		{
			scene.AddChild(levelFlag.GetSprite());
			
			return levelFlag;
		}
		
		public int CalculateOverallLevelScore()
		{
			int NumOfCoins = coins.Count;
			int NumOfGems = gems.Count;
			int overallLevelScore = (NumOfCoins*50) +(NumOfGems*200);
			return overallLevelScore;
		}
	}
}