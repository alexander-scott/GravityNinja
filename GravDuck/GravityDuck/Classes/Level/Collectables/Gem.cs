using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class Gem : Collectable
	{
		public Gem () : base()
		{
			textureInfo = new TextureInfo(new Texture2D("/Application/textures/Level/gem.png", false), new Vector2i(8, 1));
			
			sprite          = new SpriteTile(textureInfo);
			sprite.Quad.S   = textureInfo.TileSizeInPixelsf;
			
			tileIndex = 0;
			
			scoreValue = 200;
			
			sprite.ScheduleInterval( (dt) => 
			{
				if(tileIndex >= 6)
				{
					tileIndex = 0;
				}
				
				sprite.TileIndex2D = new Vector2i(tileIndex, 0);
				tileIndex++;
			}, 0.12f);
		}
	}
}