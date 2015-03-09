using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class LaserGate : Obstacle
	{
		public enum Direction {UP, DOWN, LEFT, RIGHT};
		
		private Direction laserDirection;
		
		private bool laserOn = true;
		
		//private const float laserDistance = 200.0f;
		//private const float forceModifier = 3.0f; 
		
		public LaserGate(Scene scene, Direction direction) : base(scene)
		{
			textureInfo = new TextureInfo("/Application/textures/Level/laserGate.png");
			
			laserDirection = direction;
			
			sprite          = new SpriteUV(textureInfo);
			sprite.Quad.S   = textureInfo.TextureSizef;
			
			sprite.Pivot = new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2);
			
			if(laserDirection == Direction.LEFT)
			{
				sprite.Rotate(FMath.PI/2);
				
			}
			else if(laserDirection == Direction.DOWN)
				{
					sprite.Rotate(FMath.PI);
				}
				else if(laserDirection == Direction.RIGHT)
					{
						sprite.Rotate(-FMath.PI/2);
					}
			
			scene.AddChild(sprite);
		}
		
		public bool CheckPlayerPos(Player player)
		{
			bool playerHit = false;
			
			if(laserDirection == Direction.UP || laserDirection == Direction.DOWN)
			{
				if(player.GetPos().X < sprite.Position.X + (sprite.Quad.S.X / 4)  ||
				   player.GetPos().X > sprite.Position.X + ((3 * sprite.Quad.S.X) / 4))
					playerHit = false;
				else if(player.GetPos().Y < sprite.Position.Y ||
				        player.GetPos().Y > sprite.Position.Y + sprite.Quad.S.Y)
					playerHit = false;
				else
					playerHit = true;
			}
				else if(laserDirection == Direction.LEFT || laserDirection == Direction.RIGHT)
				{
					if(player.GetPos().Y < sprite.Position.Y  + (sprite.Quad.S.Y / 4) ||
				   player.GetPos().Y > sprite.Position.Y + ((3 * sprite.Quad.S.Y) / 4))
						playerHit = false;
					else if(player.GetPos().X < sprite.Position.X ||
				        player.GetPos().X > sprite.Position.X + sprite.Quad.S.X)
						playerHit = false;
					else
						playerHit = true;
				}
			
			// If the player is passing the Gate AND the laser is on, then the player is hit.	RMDS
			if(playerHit)
				if(laserOn)
					return true;
				else
					return false;
			else
				return false;
		}
	}
}

