using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class Collectable
	{
	    protected TextureInfo textureInfo;
		protected SpriteTile sprite;
		
		protected int tileIndex;
		
		protected Vector2 position;
		
		protected bool collected;
		
		public Collectable (Scene scene)
		{
			collected = false;
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
		
		public void Reset()
		{
			
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();	
		}
	}
}