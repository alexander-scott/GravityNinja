using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class LevelFlag : Collectable
	{
		public LevelFlag () : base()
		{
			textureInfo = new TextureInfo(new Texture2D("/Application/textures/Level/levelFlag.png", false), new Vector2i(4, 1));
			
			sprite          = new SpriteTile(textureInfo);
			sprite.Quad.S   = textureInfo.TileSizeInPixelsf;
			sprite.Scale = new Vector2(1.7f, 1.7f);
			
			tileIndex = 0;
			
			sprite.ScheduleInterval( (dt) => 
			{
				if(tileIndex >= 4)
				{
					tileIndex = 0;
				}
				
				sprite.TileIndex2D = new Vector2i(tileIndex, 0);
				tileIndex++;
			}, 0.12f);
		}
	}
}