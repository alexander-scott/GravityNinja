using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	//Our Background class V1.0 by @AS
	public class LevelComplete
	{
		private TextureInfo completeTexture; //The background texture
		private SpriteUV sprite; //The background sprite
				
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
		}
		
		public void Update()
		{
			//CheckInput();
		}
		
		public void Show(float playerX, float playerY)
		{
			sprite.Position = new Vector2(playerX - (Director.Instance.GL.Context.GetViewport().Width/2), playerY - 150);
			sprite.Visible = true;
			
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