using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class Spikes : Obstacle
	{
		public Spikes(int spikeType) : base()
		{
			if(spikeType == 1)
			{
				textureInfo = new TextureInfo("/Application/textures/Level/smallSpikes.png");
			}
			else if(spikeType == 2)
			{
				textureInfo = new TextureInfo("/Application/textures/Level/largeSpikes.png");
			}
			
			sprite          = new SpriteUV(textureInfo);
			sprite.Quad.S   = textureInfo.TextureSizef;
			sprite.Pivot 	= new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2);
		}
	}
}