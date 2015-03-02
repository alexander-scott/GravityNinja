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
		
		SpriteUV areaOfWind;
		
		public WindTunnel(Scene scene, Direction direction) : base(scene)
		{
			windDirection = direction;
			
			textureInfo = new TextureInfo("/Application/textures/Level/windFan.png");

			sprite          = new SpriteUV(textureInfo);
			sprite.Quad.S   = textureInfo.TextureSizef;
			
			areaOfWind   = new SpriteUV();
			areaOfWind.Quad.S = new Vector2(200.0f, textureInfo.TextureSizef.Y);
			areaOfWind.Position = new Vector2(sprite.Position.X - windDistance, sprite.Position.Y);
			
			if(windDirection == Direction.LEFT)
			{
				sprite.Rotate(FMath.PI/2);
				
			}
			else if(windDirection == Direction.DOWN)
				{
					sprite.Rotate(FMath.PI);
				}
				else if(windDirection == Direction.RIGHT)
					{
						sprite.Rotate(-FMath.PI/2);
					}
						
			
			
			
			
			scene.AddChild(sprite);
		}
		
		public Vector2 CalculateForce(Player player)
		{
			if(windDirection == Direction.UP)
			{
				float playerDistance = player.GetPos().Y - (sprite.Position.Y + sprite.Quad.S.Y);
				
				float force = (windDistance / playerDistance) * forceModifier;
				
				Console.WriteLine(force);
				
				return new Vector2(0.0f, force);
			}
			else if (windDirection == Direction.LEFT)
				{
					float playerDistance = sprite.Position.X - player.GetPos().X;
					
					float force = (windDistance / playerDistance) * forceModifier;
					
					Console.WriteLine(-force);
					
					return new Vector2(-force, 0.0f);
				}
				else if (windDirection == Direction.DOWN)
					{
						float playerDistance = sprite.Position.Y - player.GetPos().Y;
				
						float force = (windDistance / playerDistance) * forceModifier;
						
						Console.WriteLine(-force);
						
						return new Vector2(0.0f, -force);
					}
					else if (windDirection == Direction.RIGHT)
						{
							float playerDistance = player.GetPos().X - (sprite.Position.X + sprite.Quad.S.X);
				
							float force = (windDistance / playerDistance) * forceModifier;
							
							Console.WriteLine(force);
							
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
				else if(player.GetPos().Y < sprite.Position.Y || player.GetPos().Y > sprite.Position.Y + sprite.Quad.S.Y + windDistance)
					return false;
				else
					return true;
			}
				else if(windDirection == Direction.LEFT)
				{
					if(player.GetPos().Y < sprite.Position.Y || player.GetPos().Y > sprite.Position.Y + sprite.Quad.S.Y)
						return false;
					else if(player.GetPos().X < sprite.Position.X - windDistance || player.GetPos().X > sprite.Position.X + sprite.Quad.S.X)
						return false;
					else
						return true;
				}
					else if(windDirection == Direction.DOWN)
					{
						if(player.GetPos().X < sprite.Position.X || player.GetPos().X > sprite.Position.X + sprite.Quad.S.X)
							return false;
						else if(player.GetPos().Y > sprite.Position.Y + sprite.Quad.S.Y || player.GetPos().Y < sprite.Position.Y + windDistance)
							return false;
						else
							return true;
					}
						else if(windDirection == Direction.RIGHT)
						{
							if(player.GetPos().Y < sprite.Position.Y || player.GetPos().Y > sprite.Position.Y + sprite.Quad.S.Y)
								return false;
							else if(player.GetPos().X > sprite.Position.X + sprite.Quad.S.X + windDistance || player.GetPos().X < sprite.Position.X)
								return false;
							else
								return true;
						}			
							
			return false;
		}
	}
}

