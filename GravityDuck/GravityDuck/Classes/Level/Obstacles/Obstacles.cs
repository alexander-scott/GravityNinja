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
		
		public Obstacle()
		{
			
		}
		
		public bool HasCollidedWithPlayer(SpriteUV player) //Check if the a sprite has hit a part of the maze
		{
			Bounds2 playerBounds = player.GetlContentLocalBounds();
			player.GetContentWorldBounds(ref playerBounds); //Get sprite bounds (player bounds)
			
			Bounds2 coinBounds = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref coinBounds); //Get all of the maze bounds
			
			coinBounds.Max = coinBounds.Max - 15.0f;
			coinBounds.Min = coinBounds.Min + 15.0f;
			
			if(playerBounds.Overlaps(coinBounds))
			{
				return true;
			}
			
			return false;
		}
		
		public SpriteUV GetSprite()
		{
			return sprite;	
		}
		
		public void setPosition(Vector2 newPosition)
		{
			position = newPosition;
			sprite.Position = newPosition;
		}
		
		public void setRotation(float rotation)
		{
			float degreesToRad = (rotation * Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Pi) / 180;
			sprite.Rotate(degreesToRad);
		}
		
		public SpriteUV getSprite()
		{
			return sprite;
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();	
		}
		
		public void HideSprite()
		{
			sprite.Visible = false;
		}
	}
}

