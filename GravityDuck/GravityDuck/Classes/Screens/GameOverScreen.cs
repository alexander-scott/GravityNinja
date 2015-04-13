using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	
	public class GameOverScreen : Screen
	{
		private TextureInfo restartButtonTexture; 
		private SpriteUV restartButtonSprite; 
				
		private bool restart = false;

		private Bounds2 restartBox;
		
		public GameOverScreen (Scene scene) :base(scene)
		{
			textureInfo		= new TextureInfo("/Application/textures/gameOver.png");
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(0.0f, 0.0f);
			scene.AddChild(sprite);
			sprite.Visible = false;
			
			restartButtonTexture = new TextureInfo("/Application/textures/restartButton.png");
			restartButtonSprite 			= new SpriteUV();
			restartButtonSprite 			= new SpriteUV(restartButtonTexture);
			restartButtonSprite.Quad.S 	= restartButtonTexture.TextureSizef;
			restartButtonSprite.Position = new Vector2(0.0f, 0.0f);
			scene.AddChild(restartButtonSprite);
			restartButtonSprite.Visible = false;
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
			
			
			if(touchBox.Overlaps(restartBox) && touches.Count != 0)
			{
				restart = true;
			}
			
			if (Input2.GamePad0.Square.Release)
			{
				restart = true;
			}
			
			if (touches.Count > 0)
				restart = true;
			
		}
		
		public void Show(float playerX, float playerY)
		{
			sprite.Position = new Vector2(playerX - (Director.Instance.GL.Context.GetViewport().Width/2), playerY-270);
			sprite.Visible = true;
			
			restartButtonSprite.Position = new Vector2(sprite.Position.X + (sprite.TextureInfo.TextureSizef.X/2) - (restartButtonSprite.TextureInfo.TextureSizef.X/2), sprite.Position.Y + 90);
			restartBox.Min = new Vector2(playerX - (Director.Instance.GL.Context.GetViewport().Width/2), playerY-270);
			restartBox.Max = new Vector2(playerX + 500, playerY + 500);
			restartButtonSprite.Visible = true;
		}
		
		public void ReOrderZ(Scene scene)
		{
			sprite = null;
			sprite 			= new SpriteUV(textureInfo);
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(0.0f, 0.0f);
			scene.AddChild(sprite);
			sprite.Visible = false;
			
			restartButtonSprite = null;
			restartButtonSprite 			= new SpriteUV(restartButtonTexture);
			restartButtonSprite.Quad.S 	= restartButtonTexture.TextureSizef;
			restartButtonSprite.Position = new Vector2(0.0f, 0.0f);
			scene.AddChild(restartButtonSprite);
			restartButtonSprite.Visible = false;
		}
		
		public bool CheckRestart()
		{
			return restart;
		}
		
		public void Reset()
		{
			restartButtonSprite.Visible = false;
			sprite.Visible = false;	
			var touches = Touch.GetData(0);	
			restart = false;
		}
//		
//		public void RemoveAll()
//		{
//			scene.RemoveChild(sprite, true);
//			scene.RemoveChild(restartButtonSprite, true);
//			
//		}
		
		public new void Dispose()
		{
			textureInfo.Dispose();
			restartButtonTexture.Dispose();		
		}
	}
}