using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	//Our Background class V1.0 by @AS
	public class LevelComplete : Screen
	{
		private TextureInfo stars1Texture; //The background texture
		private TextureInfo stars2Texture; //The background texture
		private TextureInfo stars3Texture; //The background texture
		private SpriteUV starsSprite; //The background sprite
				
		private bool play = false;

		public LevelComplete (Scene scene) : base(scene)
		{
			textureInfo 	= new TextureInfo("/Application/textures/levelComplete.png");
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(0.0f, 0.0f);
			scene.AddChild(sprite);
			sprite.Visible = false;
			
			stars1Texture 	= new TextureInfo("/Application/textures/stars1.png");
			stars2Texture 	= new TextureInfo("/Application/textures/stars2.png");
			stars3Texture 	= new TextureInfo("/Application/textures/stars3.png");
			starsSprite 			= new SpriteUV();
			starsSprite 			= new SpriteUV(stars1Texture);
			starsSprite.Quad.S 	= stars1Texture.TextureSizef;
			starsSprite.Position = new Vector2(0.0f, 0.0f);
			scene.AddChild(starsSprite);
			starsSprite.Visible = false;
		}
		
		public void Update()
		{
			//CheckInput();
		}
		
		public void Show(float playerX, float playerY, int stars)
		{
			sprite.Position = new Vector2(playerX - (Director.Instance.GL.Context.GetViewport().Width/2), playerY-270);
			sprite.Visible = true;
			
			starsSprite.Position = new Vector2(sprite.Position.X + (sprite.TextureInfo.TextureSizef.X/2) - (starsSprite.TextureInfo.TextureSizef.X/2), sprite.Position.Y + 90);
			
			if(stars==1)
				starsSprite.TextureInfo = stars1Texture;
			if(stars==2)
				starsSprite.TextureInfo = stars2Texture;
			if(stars==3)
				starsSprite.TextureInfo = stars3Texture;
			
			starsSprite.Visible = true;
		}
		
		/*
		public void CheckInput()
		{
			var touches = Touch.GetData(0);	
			
			var touchPos = Input2.Touch00.Pos;
			
			Bounds2 touchBox = new Bounds2();
		
			touchBox.Min.X = (touchPos.X * (Director.Instance.GL.Context.GetViewport().Width / 2))
				+ (Director.Instance.GL.Context.GetViewport().Width / 2);
			touchBox.Max.X = (touchPos.X * (Director.Instance.GL.Context.GetViewport().Width / 2))
				+ (Director.Instance.GL.Context.GetViewport().Width / 2);
			touchBox.Min.Y = (touchPos.Y * (Director.Instance.GL.Context.GetViewport().Height / 2))
				+ (Director.Instance.GL.Context.GetViewport().Height / 2);
			touchBox.Max.Y = (touchPos.Y * (Director.Instance.GL.Context.GetViewport().Height / 2))
				+ (Director.Instance.GL.Context.GetViewport().Height / 2);
		
			if(touchBox.Overlaps(playBox) && touches.Count != 0)
			{
				play = true;
				RemoveAll();
			}
			
			if (Input2.GamePad0.Cross.Release && playSprite.TextureInfo == playSelectTexture)
			{
				play = true;
				RemoveAll();
			}
			
			if (Input2.GamePad0.Down.Release)
			{
				hiscoreSprite.TextureInfo = hiscoreSelectTexture;
				playSprite.TextureInfo = playTexture;
				controlSprite.TextureInfo = controlTexture;
			}
			
			if (Input2.GamePad0.Right.Release)
			{
				hiscoreSprite.TextureInfo = hiscoreTexture;
				controlSprite.TextureInfo = controlSelectTexture;
				playSprite.TextureInfo = playTexture;
			}
			
			if (Input2.GamePad0.Left.Release)
			{
				hiscoreSprite.TextureInfo = hiscoreSelectTexture;
				controlSprite.TextureInfo = controlTexture;
				playSprite.TextureInfo = playTexture;
			}
			
			if (Input2.GamePad0.Up.Release)
			{
				hiscoreSprite.TextureInfo = hiscoreTexture;
				controlSprite.TextureInfo = controlTexture;
				playSprite.TextureInfo = playSelectTexture;

			}
			
		}
		*/
		
		public bool CheckPlay()
		{
			return play;
		}
//		
//		private void RemoveAll()
//		{
//			scene1.RemoveChild(sprite, true);
//		}
//		
		public new void Dispose()
		{
			textureInfo.Dispose();
			stars1Texture.Dispose();
			stars2Texture.Dispose();
			stars3Texture.Dispose();
		}
	}
}