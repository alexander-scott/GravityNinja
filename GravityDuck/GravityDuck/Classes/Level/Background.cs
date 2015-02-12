using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	//Our Background class V1.0 by @AS
	public class Background
	{
		private TextureInfo backgroundTexture; //The background texture
		private SpriteUV sprite; //The background sprite
		
		public Background (Scene scene)
		{
			backgroundTexture 	= new TextureInfo("/Application/textures/background.png");
			
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(backgroundTexture);
			sprite.Quad.S 	= backgroundTexture.TextureSizef;
			sprite.Scale 	= new Vector2(3.0f, 3.0f); //Make
			sprite.Pivot 	= new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2);
			sprite.Position = new Vector2(0.0f, 0.0f);
			
			scene.AddChild(sprite);
		}
		
		public void Dispose()
		{
			backgroundTexture.Dispose();
		}
		
		public SpriteUV GetSprite(){ return sprite; }
	}
}