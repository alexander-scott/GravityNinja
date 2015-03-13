using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class BreakableWall : Obstacle
	{
		public enum Direction {UP, DOWN, LEFT, RIGHT};
		
		private Direction wallSpanDirection;
		
		private bool intact = true;
		
		private float requiredMomentum = 0.0f;
		
		public BreakableWall(Scene scene, Direction direction, float breakThreshold) : base(scene)
		{
			textureInfo = new TextureInfo("/Application/textures/Level/breakableWall.png");
			
			wallSpanDirection = direction;
						
			requiredMomentum = breakThreshold;
			
			sprite          = new SpriteUV(textureInfo);
			sprite.Quad.S   = textureInfo.TextureSizef;
			
			sprite.Pivot = new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2);
			
			if(wallSpanDirection == Direction.LEFT)
			{
				sprite.Rotate(FMath.PI/2);
				
			}
			else if(wallSpanDirection == Direction.DOWN)
				{
					sprite.Rotate(FMath.PI);
				}
				else if(wallSpanDirection == Direction.RIGHT)
					{
						sprite.Rotate(-FMath.PI/2);
					}
			
			scene.AddChild(sprite);
		}
		
		public bool CheckIfBreak(float momentum)
		{
			if(intact)		
				if(momentum >= requiredMomentum)
					intact = false;
			
			return intact;
		}
	}
}

