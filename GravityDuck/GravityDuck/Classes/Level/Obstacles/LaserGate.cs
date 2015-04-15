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
		
		private bool laserOn = false;
		
		private int tileIndex;
		private new SpriteTile sprite;
			
		public LaserGate(int random) : base()
		{
			textureInfo = new TextureInfo(new Texture2D("/Application/textures/Level/LaserBeams.png", false), new Vector2i(18, 1));
			
			sprite          = new SpriteTile(textureInfo);
			sprite.Quad.S   = textureInfo.TileSizeInPixelsf;
			
			//sprite.Pivot = new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2);

			tileIndex = random;
			
			sprite.ScheduleInterval( (dt) => 
			{
				if(tileIndex >= 18)
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
					laserDirection = Direction.UP;
				break;
				
				case 360:
					laserDirection = Direction.UP;
				break;
					
				case 90:
					laserDirection = Direction.LEFT;
				break;
				
				case 180:
					laserDirection = Direction.DOWN;
				break;
				
				case 270:
					laserDirection = Direction.RIGHT;
				break;
			}
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
		
		public new bool HasCollidedWithPlayer(SpriteUV player) //Check if the a sprite has hit a part of the maze
		{
			if (tileIndex > 5 && tileIndex < 15)
				laserOn = true;
			else
				laserOn = false;
			
			Bounds2 playerBounds = player.GetlContentLocalBounds();
			player.GetContentWorldBounds(ref playerBounds); //Get sprite bounds (player bounds)
			
			Bounds2 laserBounds = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref laserBounds); //Get all of the maze bounds
			
			laserBounds.Max = laserBounds.Max - 15.0f;
			laserBounds.Min = laserBounds.Min + 15.0f;
			
			if(playerBounds.Overlaps(laserBounds) && laserOn)
			{
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

