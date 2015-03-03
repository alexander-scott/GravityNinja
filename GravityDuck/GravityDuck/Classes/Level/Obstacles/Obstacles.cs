using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class Obstacle
	{
	    protected TextureInfo textureInfo;
		protected SpriteUV sprite;
		
		protected Vector2 position;
		
		public Obstacle(Scene scene)
		{
			
		}
		
		public bool HasCollidedWithPlayer(SpriteUV player) //Check if the a sprite has hit a part of the maze
		{
			Bounds2 playerBounds = player.GetlContentLocalBounds();
			player.GetContentWorldBounds(ref playerBounds); //Get sprite bounds (player bounds)
			
			Bounds2 coinBounds = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref coinBounds); //Get all of the maze bounds
			
			if(playerBounds.Overlaps(coinBounds))
			{
				return true;
			}
			
			return false;
		}
		
		public void setPosition(Vector2 newPosition)
		{
			position = newPosition;
			sprite.Position = newPosition;
		}
		
		public SpriteUV getSprite()
		{
			return sprite;
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();	
		}
	}
}

