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
		
		public Background (Scene scene, Vector2 centrePos)
		{
			backgroundTexture 	= new TextureInfo("/Application/textures/bground.png");
			
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(backgroundTexture);
			sprite.Quad.S 	= backgroundTexture.TextureSizef;
			//sprite.Pivot 	= new Vector2(sprite.Quad.S.X, sprite.Quad.S.Y);
			sprite.CenterSprite(new Vector2(0.5f,0.5f));
			sprite.Position = centrePos;
			sprite.Angle = 0.0f;
			
			scene.AddChild(sprite);
		}
		
		public void Update(Vector2 centrePos, Vector2 rotation)
		{
			sprite.Position = centrePos;
			sprite.Angle = -(float)FMath.Atan2(rotation.X, rotation.Y);
		}
		
		public void Dispose()
		{
			backgroundTexture.Dispose();
		}
	}
}