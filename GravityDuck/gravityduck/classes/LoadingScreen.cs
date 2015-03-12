using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;

namespace GravityDuck
{
	public class LoadingScreen
	{
		private static BusyIndicator loadingSymbol;
		
		private TextureInfo loadingTexture; 
		private SpriteUV sprite; 
		
		private Bounds2 startBox;
		private bool play = false;
		
		private int loadTime = 0;
		
		private static Sce.PlayStation.HighLevel.UI.Scene	uiScene1;
		private Sce.PlayStation.HighLevel.UI.Label	loadingLabel;
		
		private Sce.PlayStation.HighLevel.GameEngine2D.Scene scene1;
		
		public LoadingScreen (Sce.PlayStation.HighLevel.GameEngine2D.Scene scene, Sce.PlayStation.HighLevel.UI.Scene uiScene)
		{
			scene1 = scene;
			uiScene1 = uiScene;
			
		 	loadingTexture 	= new TextureInfo("/Application/textures/Level1Load.png");
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(loadingTexture);
			sprite.Quad.S 	= loadingTexture.TextureSizef;
			sprite.Position = new Vector2(-5000.0f, -5000.0f);
			sprite.CenterSprite(new Vector2(0.5f, 0.5f));

			loadingLabel = new Sce.PlayStation.HighLevel.UI.Label();
			loadingLabel.X = 804.0f;
			loadingLabel.Y = 503.0f;
			loadingLabel.Text = "Loading...";
			loadingLabel.TextColor = new UIColor(0.0f, 0.0f, 0.0f, 1.0f);
			uiScene.RootWidget.AddChildLast(loadingLabel);
			
			loadingSymbol = new BusyIndicator(true);
			loadingSymbol.SetPosition(910, 495);
			loadingSymbol.SetSize(48, 48);
			loadingSymbol.Anchors = Anchors.Height | Anchors.Width;
			uiScene.RootWidget.AddChildLast(loadingSymbol);
			loadingSymbol.Start();
			
			scene.AddChild(sprite);
			UISystem.SetScene(uiScene);
			
			startBox.Min = new Vector2(sprite.Position.X + loadingLabel.X - 500, sprite.Position.Y + loadingLabel.Y - 500);
			startBox.Max = new Vector2(sprite.Position.X + loadingLabel.X + 500, sprite.Position.Y + loadingLabel.Y + 500);
		}
		
		public void Update(int secs)
		{
			if (secs > loadTime && loadTime != 0)
			{
				CheckInput();	
				loadingSymbol.Visible = false;
				loadingLabel.Text = "Ready to Play";
				loadingLabel.TextColor = new UIColor(0.0f, 0.0f, 0.0f, 1.0f);
			}
				
		}
		
		public void CheckInput()
		{
			var touches = Touch.GetData(0);	
			
			var touchPos = Input2.Touch00.Pos;
			
			Bounds2 touchBox = new Bounds2();
			
			touchBox.Min.X = (touchPos.X * (Director.Instance.GL.Context.GetViewport().Width / 2))
				+ (Director.Instance.GL.Context.GetViewport().Width / 2) - 5000.0f;
			touchBox.Max.X = (touchPos.X * (Director.Instance.GL.Context.GetViewport().Width / 2))
				+ (Director.Instance.GL.Context.GetViewport().Width / 2)- 5000.0f;
			touchBox.Min.Y = (touchPos.Y * (Director.Instance.GL.Context.GetViewport().Height / 2))
				+ (Director.Instance.GL.Context.GetViewport().Height / 2)- 5000.0f;
			touchBox.Max.Y = (touchPos.Y * (Director.Instance.GL.Context.GetViewport().Height / 2))
				+ (Director.Instance.GL.Context.GetViewport().Height / 2)- 5000.0f;
			
			if(touchBox.Overlaps(startBox) && touches.Count != 0)
			{
				play = true;
			}
		}
		
		public bool CheckPlay()
		{
			return play;
		}
		
		public void SetVisible(bool visible)
		{
			if (visible)
			{
				sprite.Visible = true;
				loadingLabel.Visible = true;
				loadingSymbol.Visible = true;
			}
			else
			{
				sprite.Visible = false;
				loadingLabel.Visible = false;
				loadingSymbol.Visible = false;
			}
		}
		
		public void Dispose()
		{
			//loadingTexture.Dispose();	
		}
		
		public void SetLoadTime(int time)
		{
			loadTime = time;
		}
	}
}

