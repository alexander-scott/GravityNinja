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
		
		private List<int> level;
		private List<Coin> coins;
		private List<Gem> gems;
		private List<Spikes> spikes;
		private LevelFlag levelFlag;
		
		private Vector2 playerPos;
		
		public LevelLoader ()
		{
			level = new List<int>();
			coins = new List<Coin>();
			gems = new List<Gem>();
			spikes = new List<Spikes>();
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
						smallSpikes.setPosition(new Vector2(x - 120.0f, y + 25.0f));
					}
					
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
						largeSpikes.setPosition(new Vector2(x - 120.0f, y + 25.0f));
					}
				
					largeSpikes.setRotation(-rotation);
					spikes.Add(largeSpikes);
				break;
				
				// Level Flag
				case "Level":
					levelFlag = new LevelFlag();
					levelFlag.setPosition(new Vector2(x, y - 80.0f));
					levelFlag.setRotation(-rotation);
				break;
				
				// Player Position
				case "":
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
						
						case 0:
							sprites[i,ii] = new SpriteUV(null);
						break;
					}
				}
			}
			
			return sprites;
		}
		
		public Coin[] LoadInCoins(Scene scene)
		{
			Coin[] coinsObj = new Coin[coins.Count];
			
			for(int i = 0; i < coins.Count; i++)
			{
				coinsObj[i] = coins[i];
				scene.AddChild(coins[i].GetSprite());
			}
			
			return coinsObj;
		}
		
		public Gem[] LoadInGems(Scene scene)
		{
			Gem[] gemsObj = new Gem[gems.Count];
			
			for(int i = 0; i < gems.Count; i++)
			{
				gemsObj[i] = gems[i];
				scene.AddChild(gems[i].GetSprite());
			}
			
			return gemsObj;
		}
		
		public Spikes[] LoadInSpikes(Scene scene)
		{
			Spikes[] spikesObj = new Spikes[spikes.Count];
			
			for (int i = 0; i < spikes.Count; i++)
			{
				spikesObj[i] = spikes[i];
				scene.AddChild(spikes[i].GetSprite());
			}
			
			return spikesObj;
		}
		
		public LevelFlag LoadInFlag(Scene scene)
		{
			scene.AddChild(levelFlag.GetSprite());
			
			return levelFlag;
		}
	}
}