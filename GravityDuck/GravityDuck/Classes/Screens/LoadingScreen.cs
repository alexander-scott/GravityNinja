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
	//Loading screen V1.1 @AS
	public class LoadingScreen : Screen
	{
		private TextureInfo loadingTexture0, loadingTexture1, loadingTexture2, loadingTexture3, loadingTexture4, loadingTexture5;
		private static BusyIndicator loadingSymbol;
		private static Button readyButton;
		
		private Bounds2 startBox;
		private bool play = false;
		
		private int loadTime = 0;

		private Sce.PlayStation.HighLevel.UI.Label	loadingLabel;
		private Sce.PlayStation.HighLevel.UI.Label	levelLabel;
		
		public LoadingScreen (Sce.PlayStation.HighLevel.GameEngine2D.Scene scene, Sce.PlayStation.HighLevel.UI.Scene uiScene) : base(scene)
		{
			loadingTexture0 	= new TextureInfo("/Application/textures/LoadingScreens/Level0Load.png");
			loadingTexture1 	= new TextureInfo("/Application/textures/LoadingScreens/Level1Load.png");
			loadingTexture2 	= new TextureInfo("/Application/textures/LoadingScreens/Level2Load.png");
			loadingTexture3 	= new TextureInfo("/Application/textures/LoadingScreens/Level3Load.png");
			loadingTexture4 	= new TextureInfo("/Application/textures/LoadingScreens/Level4Load.png");
			loadingTexture5 	= new TextureInfo("/Application/textures/LoadingScreens/Level5Load.png");
			
		 	textureInfo 	= new TextureInfo("/Application/textures/LoadingScreens/Level0Load.png");
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(-5000.0f, -5000.0f);
			sprite.CenterSprite(new Vector2(0.5f, 0.5f));

			loadingLabel = new Sce.PlayStation.HighLevel.UI.Label();
			loadingLabel.X = 804.0f;
			loadingLabel.Y = 503.0f;
			loadingLabel.Text = "Loading...";
			loadingLabel.TextColor = new UIColor(0.0f, 0.0f, 0.0f, 1.0f);
			uiScene.RootWidget.AddChildLast(loadingLabel);
			
			levelLabel = new Sce.PlayStation.HighLevel.UI.Label();
			levelLabel.X = 15.0f;
			levelLabel.Y = 503.0f;
			levelLabel.Text = "";
			levelLabel.TextColor = new UIColor(0.0f, 0.0f, 0.0f, 1.0f);
			//levelLabel.Font = new UIFont(FontAlias.System, 32, FontStyle.Regular);
			uiScene.RootWidget.AddChildLast(levelLabel);
			
			loadingSymbol = new BusyIndicator(true);
			loadingSymbol.SetPosition(910, 495);
			loadingSymbol.SetSize(48, 48);
			loadingSymbol.Anchors = Anchors.Height | Anchors.Width;
			uiScene.RootWidget.AddChildLast(loadingSymbol);
			loadingSymbol.Start();
			
			readyButton = new Button();
			readyButton.SetPosition(860, 490);
			readyButton.Text = "PLAY";
			readyButton.SetSize(88,48);
			readyButton.BackgroundFilterColor = new UIColor(255.0f, 255.0f, 0.0f, 1.0f);
			readyButton.ButtonAction += HandleButtonAction;
			readyButton.Visible = false;
			uiScene.RootWidget.AddChildLast(readyButton);
			
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
				readyButton.Visible = true;
				loadingLabel.Visible = false;
			}	
		}
		
		public void CheckInput()
		{
			var touches = Touch.GetData(0);	
			
			var touchPos = Input2.Touch00.Pos;
			
//			Bounds2 touchBox = new Bounds2();
//			
//			touchBox.Min.X = (touchPos.X * (Director.Instance.GL.Context.GetViewport().Width / 2))
//				+ (Director.Instance.GL.Context.GetViewport().Width / 2) - 5000.0f;
//			touchBox.Max.X = (touchPos.X * (Director.Instance.GL.Context.GetViewport().Width / 2))
//				+ (Director.Instance.GL.Context.GetViewport().Width / 2)- 5000.0f;
//			touchBox.Min.Y = (touchPos.Y * (Director.Instance.GL.Context.GetViewport().Height / 2))
//				+ (Director.Instance.GL.Context.GetViewport().Height / 2)- 5000.0f;
//			touchBox.Max.Y = (touchPos.Y * (Director.Instance.GL.Context.GetViewport().Height / 2))
//				+ (Director.Instance.GL.Context.GetViewport().Height / 2)- 5000.0f;
//			
//			if(touchBox.Overlaps(startBox) && touches.Count != 0)
//			{
//				play = true;
//			}
		}
		
		public bool CheckPlay()
		{
			return play;
		}
		
		public void HandleButtonAction(object sender, TouchEventArgs e)
		{
		 	play = true;
		}
		
		public void SetVisible(bool visible, int level)
		{
			if (visible)
			{
				sprite.Visible = true;
				loadingLabel.Visible = true;
				loadingSymbol.Visible = true;
				levelLabel.Visible = true;
				levelLabel.Text = "LEVEL " + level;
				if (level == 0)
				{
					sprite.TextureInfo = loadingTexture0;
				}	
				else if (level == 1)
				{
					sprite.TextureInfo = loadingTexture1;
				}
				else if (level == 2)
				{
					sprite.TextureInfo = loadingTexture2;
				}
				else if (level == 3)
				{
					sprite.TextureInfo = loadingTexture3;
				}
				else if (level == 4)
				{
					sprite.TextureInfo = loadingTexture4;
				}
				else if (level == 5)
				{
					sprite.TextureInfo = loadingTexture5;
				}	
			}
			else
			{
				sprite.Visible = false;
				loadingLabel.Visible = false;
				loadingSymbol.Visible = false;
				readyButton.Visible = false;
				levelLabel.Visible = false;
				play = false;
			}
		}
		
		public void SetLoadTime(int time)
		{
			loadTime = time;
		}
		
		public new void Dispose()
		{
			textureInfo.Dispose();
			loadingTexture0.Dispose();
			loadingTexture1.Dispose();
			loadingTexture2.Dispose();
			loadingTexture3.Dispose();
			loadingTexture4.Dispose();
			loadingTexture5.Dispose();
		}
	}
}

