using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	//Our Background class V1.0 by @AS
	public class TitleScreen
	{
		private TextureInfo titleTexture; //The background texture
		private SpriteUV sprite; //The background sprite
		
		private TextureInfo playTexture; //The background texture
		private SpriteUV playSprite; //The background sprite
		
		private TextureInfo hiscoreTexture; //The background texture
		private SpriteUV hiscoreSprite; //The background sprite
		
		private TextureInfo controlTexture; //The background texture
		private SpriteUV controlSprite; //The background sprite
		
		public TitleScreen (Scene scene)
		{
			titleTexture 	= new TextureInfo("/Application/textures/titleScreen.png");
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(titleTexture);
			sprite.Quad.S 	= titleTexture.TextureSizef;
			sprite.Position = new Vector2(0.0f, 0.0f);
			
			playTexture 		= new TextureInfo("/Application/textures/playSelected.png");
			playSprite 			= new SpriteUV();
			playSprite 			= new SpriteUV(playTexture);
			playSprite.Quad.S 	= playTexture.TextureSizef;
			playSprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width/2 - (playTexture.TextureSizef.X/2),160);
			
			hiscoreTexture 	= new TextureInfo("/Application/textures/hiscores.png");
			hiscoreSprite 			= new SpriteUV();
			hiscoreSprite 			= new SpriteUV(hiscoreTexture);
			hiscoreSprite.Quad.S 	= hiscoreTexture.TextureSizef;
			hiscoreSprite.Position = new Vector2(10.0f, 10.0f);
			
			controlTexture 	= new TextureInfo("/Application/textures/controls.png");
			controlSprite 			= new SpriteUV();
			controlSprite 			= new SpriteUV(controlTexture);
			controlSprite.Quad.S 	= controlTexture.TextureSizef;
			controlSprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width - playTexture.TextureSizef.X , 10.0f);
			
			scene.AddChild(sprite);
			scene.AddChild(playSprite);
			scene.AddChild(controlSprite);
			scene.AddChild(hiscoreSprite);
		}
		
		public void Update()
		{
		
		}
		
		public void RemoveAll(Scene scene)
		{
			
			scene.RemoveChild(sprite, true);
			scene.RemoveChild(playSprite, true);
			scene.RemoveChild(controlSprite, true);
			scene.RemoveChild(hiscoreSprite, true);
		}
		
		public void Dispose()
		{
			titleTexture.Dispose();
			playTexture.Dispose();
			controlTexture.Dispose();
			hiscoreTexture.Dispose();
				
		}
	}
}