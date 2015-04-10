using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class Coin : Collectable
	{
		public Coin () : base()
		{
			textureInfo = new TextureInfo(new Texture2D("/Application/textures/Level/goldCoin.png", false), new Vector2i(6, 1));
			
			sprite          = new SpriteTile(textureInfo);
			sprite.Quad.S   = textureInfo.TileSizeInPixelsf;
			
			tileIndex = 0;
			
			scoreValue = 50;
			
			sprite.ScheduleInterval( (dt) => 
			{
				if(tileIndex >= 6)
				{
					tileIndex = 0;
				}
				
				sprite.TileIndex2D = new Vector2i(tileIndex, 0);
				tileIndex++;
			}, 0.10f);
		}
	}
}