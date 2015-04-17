using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class BlackHole : Obstacle
	{
		public enum Direction {UP, DOWN, LEFT, RIGHT};
		
		private Direction halfRadialDirection;
		
		private const float radialDistance = 300.0f;
		private const float forceModifier = 7.0f;
		
		private int tileIndex;
		private new SpriteTile sprite;
		
		public BlackHole() : base()
		{
			textureInfo = new TextureInfo(new Texture2D("/Application/textures/Level/BlackHoles.png", false), new Vector2i(4, 1));
			
			sprite          = new SpriteTile(textureInfo);
			sprite.Quad.S   = textureInfo.TileSizeInPixelsf;
			//sprite.Pivot 	= new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2);
			
			tileIndex = 0;
			
			sprite.ScheduleInterval( (dt) => 
			{
				if(tileIndex >= 4)
				{
					tileIndex = 0;
				}
				
				sprite.TileIndex2D = new Vector2i(tileIndex, 0);
				tileIndex++;
			}, 0.10f);
		}
		
		public void setDirection(int rotation)
		{
			switch(rotation)
			{
				case 0:
					halfRadialDirection = Direction.UP;
				break;
				
				case 360:
					halfRadialDirection = Direction.UP;
				break;
					
				case 90:
					halfRadialDirection = Direction.LEFT;
				break;
				
				case 180:
					halfRadialDirection = Direction.DOWN;
				break;
				
				case 270:
					halfRadialDirection = Direction.RIGHT;
				break;
			}
			
		}
		
		public Vector2 CalculateForce(Player player)
		{
			Vector2 force = new Vector2(0.0f, 0.0f);
			
			Vector2 vectorTo = new Vector2(0.0f, 0.0f);
			
			if(halfRadialDirection == Direction.LEFT)
				vectorTo = player.GetPos() - (sprite.Position + new Vector2(sprite.Quad.S.X, sprite.Quad.S.Y/2));
			else if(halfRadialDirection == Direction.RIGHT)
					vectorTo = player.GetPos() - (sprite.Position + new Vector2(0.0f, sprite.Quad.S.Y/2));
				else if(halfRadialDirection == Direction.DOWN)
						vectorTo = player.GetPos() - (sprite.Position + new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y));
					else
						vectorTo = player.GetPos() - (sprite.Position + new Vector2(sprite.Quad.S.X/2, 0.0f));
			
			bool isFacingAway = false;
			
			if(halfRadialDirection == Direction.LEFT)
				if(vectorTo.X > 0.0f)
					isFacingAway = true;
				else{}			
			else if(halfRadialDirection == Direction.RIGHT)
				if(vectorTo.X > 0.0f)
					isFacingAway = true;
				else{}			
			else if(halfRadialDirection == Direction.UP)
				if(vectorTo.Y > 0.0f)
					isFacingAway = true;
				else{}		
			else if(vectorTo.Y < 0.0f)
					isFacingAway = true;
				else{}
			
			//if(isFacingAway)
			//{
				float distance = vectorTo.Length();
				
				// Direction of force will be towards the Black Hole
				vectorTo = vectorTo.Normalize();
				vectorTo = vectorTo.Multiply(-1.0f);
				
				float forcePropToDist = distance / radialDistance;
				
				force = new Vector2(vectorTo.X * ((1 - forcePropToDist) * forceModifier), vectorTo.Y * ((1 - forcePropToDist) * forceModifier));			
			//}	
			
			return force;
		}
		
		public new bool HasCollidedWithPlayer(SpriteUV player) //Check if the a sprite has hit a part of the maze
		{
			Bounds2 playerBounds = player.GetlContentLocalBounds();
			player.GetContentWorldBounds(ref playerBounds); //Get sprite bounds (player bounds)
			
			Bounds2 holeBounds = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref holeBounds); //Get all of the maze bounds
			
			holeBounds.Max = holeBounds.Max - 15.0f;
			holeBounds.Min = holeBounds.Min + 15.0f;
			
			if(playerBounds.Overlaps(holeBounds))
			{
				return true;
			}
			
			return false;
		}
		
		public bool CheckPlayerPos(Player player)
		{
			Vector2 vectorTo = player.GetPos() - (sprite.Position + new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2));
			float distance = vectorTo.Length();
			
			if(distance <= radialDistance)
				return true;
			
			return false;
		}
		
		public new void setPosition(Vector2 newPosition)
		{
			position = newPosition;
			sprite.Position = newPosition;
		}
		
		public new void setRotation(float rotation)
		{
			float degreesToRad = (rotation * Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Pi) / 180;
			sprite.Rotate(degreesToRad);
		}
		
		public new SpriteTile getSprite()
		{
			return sprite;
		}
		
		public new void Dispose()
		{
			textureInfo.Dispose();	
		}
		
		public new void HideSprite()
		{
			sprite.Visible = false;
		}
	}
}

