using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	
	public class LevelComplete
	{
		private TextureInfo completeTexture; 
		private SpriteUV sprite; 
		
		private TextureInfo stars1Texture; 
		private TextureInfo stars2Texture; 
		private TextureInfo stars3Texture; 
		private SpriteUV starsSprite; 
				
		private bool play = false;
		
		private Scene scene1;
			
		public LevelComplete (Scene scene)
		{
			scene1 = scene;
			
			completeTexture 	= new TextureInfo("/Application/textures/levelComplete.png");
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(completeTexture);
			sprite.Quad.S 	= completeTexture.TextureSizef;
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
			sprite.Position = new Vector2(playerX - (Director.Instance.GL.Context.GetViewport().Width/2), playerY - 150);
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
		
		private void RemoveAll()
		{
			scene1.RemoveChild(sprite, true);
		}
		
		public void Dispose()
		{
			completeTexture.Dispose();
				
		}
	}
}