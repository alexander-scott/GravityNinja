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
		
		private const float radialDistance = 200.0f;
		private const float forceModifier = 3.0f;
		
		public BlackHole() : base()
		{
			textureInfo = new TextureInfo("/Application/textures/Level/blackHole.png");
			
			sprite          = new SpriteUV(textureInfo);
			sprite.Quad.S   = textureInfo.TextureSizef;
			sprite.Pivot 	= new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2);
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
			
			if(halfRadialDirection == Direction.LEFT)
			{
				sprite.Rotate(FMath.PI/2);
				
			}
			else if(halfRadialDirection == Direction.DOWN)
				{
					sprite.Rotate(FMath.PI);
				}
				else if(halfRadialDirection == Direction.RIGHT)
					{
						sprite.Rotate(-FMath.PI/2);
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
			
			if(isFacingAway)
			{
				float distance = vectorTo.Length();
				
				// Direction of force will be towards the Black Hole
				vectorTo = vectorTo.Normalize();
				vectorTo = vectorTo.Multiply(-1.0f);
				
				float forcePropToDist = distance / radialDistance;
				
				force = new Vector2(vectorTo.X * ((1 - forcePropToDist) * forceModifier), vectorTo.Y * ((1 - forcePropToDist) * forceModifier));			
			}	
			
			return force;
		}
		
		public bool CheckPlayerPos(Player player)
		{
			Vector2 vectorTo = player.GetPos() - (sprite.Position + new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2));
			float distance = vectorTo.Length();
			
			if(distance <= radialDistance)
				return true;
			
			return false;
		}
	}
}

