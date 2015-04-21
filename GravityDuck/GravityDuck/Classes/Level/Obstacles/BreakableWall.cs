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
		
		float breakThreshold;
		
		private bool intact = true;
		
		private float requiredMomentum = 7.0f;
		
		public BreakableWall() : base()
		{
			breakThreshold = 50.0f;
			
			textureInfo = new TextureInfo("/Application/textures/Level/breakableWall.png");
			
			sprite          = new SpriteUV(textureInfo);
			sprite.Quad.S   = textureInfo.TextureSizef;
			
			sprite.Pivot = new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2);
		}
		
		public void setDirection(int rotation)
		{
			switch(rotation)
			{
				case 0:
					wallSpanDirection = Direction.UP;
				break;
				
				case 360:
					wallSpanDirection = Direction.UP;
				break;
					
				case 90:
					wallSpanDirection = Direction.LEFT;
				break;
				
				case 180:
					wallSpanDirection = Direction.DOWN;
				break;
				
				case 270:
					wallSpanDirection = Direction.RIGHT;
				break;
			}
			
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

