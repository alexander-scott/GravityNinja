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
		private TextureInfo playSelectTexture;
		private SpriteUV playSprite; //The background sprite
		
		private TextureInfo hiscoreTexture;
		private TextureInfo hiscoreSelectTexture;//The background texture
		private SpriteUV hiscoreSprite; //The background sprite
		
		private TextureInfo controlTexture;
		private TextureInfo controlSelectTexture;//The background texture
		private SpriteUV controlSprite; //The background sprite
		
		private bool play = false;
		
		private Scene scene1;
		
		private Bounds2 playBox;
		private Bounds2 controlsBox;
		private Bounds2 hiscoreBox;
		
		public TitleScreen (Scene scene)
		{
			scene1 = scene;
			
			titleTexture 	= new TextureInfo("/Application/textures/titleScreen.png");
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(titleTexture);
			sprite.Quad.S 	= titleTexture.TextureSizef;
			sprite.Position = new Vector2(0.0f, 0.0f);
			
			playTexture 		= new TextureInfo("/Application/textures/play.png");
			playSelectTexture 	= new TextureInfo("/Application/textures/playSelected.png");
			playSprite 			= new SpriteUV();
			playSprite 			= new SpriteUV(playSelectTexture);
			playSprite.Quad.S 	= playTexture.TextureSizef;
			playSprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width/2 - (playTexture.TextureSizef.X/2),110);
			
			hiscoreTexture 	= new TextureInfo("/Application/textures/hiscores.png");
			hiscoreSelectTexture 	= new TextureInfo("/Application/textures/hiscoresSelected.png");
			hiscoreSprite 			= new SpriteUV();
			hiscoreSprite 			= new SpriteUV(hiscoreTexture);
			hiscoreSprite.Quad.S 	= hiscoreTexture.TextureSizef;
			hiscoreSprite.Position = new Vector2(10.0f, 10.0f);
			
			controlTexture 	= new TextureInfo("/Application/textures/controls.png");
			controlSelectTexture 	= new TextureInfo("/Application/textures/controlsSelected.png");
			controlSprite 			= new SpriteUV();
			controlSprite 			= new SpriteUV(controlTexture);
			controlSprite.Quad.S 	= controlTexture.TextureSizef;
			controlSprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width - controlTexture.TextureSizef.X , 10.0f);
			
			playBox.Min = playSprite.Position;
			playBox.Max = playSprite.Position + playSprite.TextureInfo.TextureSizef;
			
			
			scene.AddChild(sprite);
			scene.AddChild(playSprite);
			scene.AddChild(controlSprite);
			scene.AddChild(hiscoreSprite);
		}
		
		public void Update()
		{
			CheckInput();
			
		}
		
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
		
		public bool CheckPlay()
		{
			return play;
		}
		
		private void RemoveAll()
		{
			scene1.RemoveChild(sprite, true);
			scene1.RemoveChild(playSprite, true);
			scene1.RemoveChild(controlSprite, true);
			scene1.RemoveChild(hiscoreSprite, true);
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