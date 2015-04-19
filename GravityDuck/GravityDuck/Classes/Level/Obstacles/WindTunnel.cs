using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class WindTunnel : Obstacle
	{
		public enum Direction {UP, DOWN, LEFT, RIGHT};
		
		private Direction windDirection;
		
		private const float windDistance = 400.0f;
		private const float forceModifier = 5.0f; 
		private const float smallestDistance = 100.0f;	// This is to stop the distance being so small that the calculation will
														//	increase the player's velocity by enormous amounts
		private int tileIndex;
		private new SpriteTile sprite;
		
		public WindTunnel() : base()
		{			
			textureInfo = new TextureInfo(new Texture2D("/Application/textures/Level/Winds3.png", false), new Vector2i(9, 1));

			sprite          = new SpriteTile(textureInfo);
			sprite.Quad.S   = textureInfo.TileSizeInPixelsf;
			//sprite.Pivot = new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2);
			
			tileIndex = 0;
			
			sprite.ScheduleInterval( (dt) => 
			{
				if(tileIndex >= 9)
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
					windDirection = Direction.UP;

				break;
				
				case 360:
					windDirection = Direction.UP;
				break;
					
				case 90:
					windDirection = Direction.LEFT;
				break;
				
				case 180:
					windDirection = Direction.DOWN;
				break;
				
				case 270:
					windDirection = Direction.RIGHT;
				break;
			}
			
			if(windDirection == Direction.LEFT)
			{
				sprite.Rotate(FMath.PI);
			}
			else if(windDirection == Direction.RIGHT)
			{
				sprite.Rotate(FMath.PI);
			}
		}	
		
		public Vector2 CalculateForce(Player player)
		{
			if(windDirection == Direction.UP)
			{
				float playerDistance = player.GetPos().Y - (sprite.Position.Y + sprite.Quad.S.Y);
				
				if(playerDistance < smallestDistance)
					playerDistance = smallestDistance;
				
				float force = (windDistance / playerDistance) * forceModifier;
				
				return new Vector2(0.0f, force);
			}
			else if (windDirection == Direction.LEFT)
				{
					float playerDistance = sprite.Position.X - player.GetPos().X;
					
					if(playerDistance < smallestDistance)
						playerDistance = smallestDistance;
				
					float force = (windDistance / playerDistance) * forceModifier;

					return new Vector2(-force, 0.0f);
				}
				else if (windDirection == Direction.DOWN)
					{
						float playerDistance = sprite.Position.Y - player.GetPos().Y;
				
						if(playerDistance < smallestDistance)
							playerDistance = smallestDistance;
				
						float force = (windDistance / playerDistance) * forceModifier;

						return new Vector2(0.0f, -force);
					}
					else if (windDirection == Direction.RIGHT)
						{
							float playerDistance = player.GetPos().X - (sprite.Position.X + sprite.Quad.S.X);
				
							if(playerDistance < smallestDistance)
								playerDistance = smallestDistance;
				
							float force = (windDistance / playerDistance) * forceModifier;

							return new Vector2(force, 0.0f);
						}
						
			
			return new Vector2(0.0f, 0.0f);
		}
		
		public bool CheckPlayerPos(Player player)
		{
			if(windDirection == Direction.UP)
			{
				if(player.GetPos().X < sprite.Position.X || player.GetPos().X > sprite.Position.X + sprite.Quad.S.X)
					return false;
				else if(player.GetPos().Y < sprite.Position.Y + sprite.Quad.S.Y || player.GetPos().Y > sprite.Position.Y + sprite.Quad.S.Y + windDistance)
					return false;
				else
					return true;
			}
				else if(windDirection == Direction.LEFT)
				{
					if(player.GetPos().Y < sprite.Position.Y || player.GetPos().Y > sprite.Position.Y + sprite.Quad.S.Y)
						return false;
					else if(player.GetPos().X < sprite.Position.X - windDistance || player.GetPos().X > sprite.Position.X)
						return false;
					else
						return true;
				}
					else if(windDirection == Direction.DOWN)
					{
						if(player.GetPos().X < sprite.Position.X || player.GetPos().X > sprite.Position.X + sprite.Quad.S.X)
							return false;
						else if(player.GetPos().Y > sprite.Position.Y || player.GetPos().Y < sprite.Position.Y - windDistance)
							return false;
						else
							return true;
					}
						else if(windDirection == Direction.RIGHT)
						{
							if(player.GetPos().Y < sprite.Position.Y || player.GetPos().Y > sprite.Position.Y + sprite.Quad.S.Y)
								return false;
							else if(player.GetPos().X > sprite.Position.X + sprite.Quad.S.X + windDistance || player.GetPos().X < sprite.Position.X + sprite.Quad.S.X)
								return false;
							else
								return true;
						}			
							
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

