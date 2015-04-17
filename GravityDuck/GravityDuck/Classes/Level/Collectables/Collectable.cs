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
		
		protected int scoreValue;
		
		public Collectable ()
		{
			collected = false;
		}
		
		public bool HasCollidedWithPlayer(SpriteUV player) //Check if the a sprite has hit a part of the maze
		{
			if(collected == false)
			{
				Bounds2 playerBounds = player.GetlContentLocalBounds();
				player.GetContentWorldBounds(ref playerBounds); //Get sprite bounds (player bounds)
			
				Bounds2 spriteBounds = sprite.GetlContentLocalBounds();	
				sprite.GetContentWorldBounds(ref spriteBounds); //Get all of the maze bounds
			
				if(playerBounds.Overlaps(spriteBounds))
				{
					return true;
				}
			}
			
			return false;
		}
		
		public SpriteTile GetSprite()
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
		
		public int Collected(Scene scene)
		{
			collected = true;
			
			scene.RemoveChild(sprite, false);
			
			return scoreValue;
		}
		
		public void Reset()
		{
			
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