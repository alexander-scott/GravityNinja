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
		private TextureInfo loadingTexture0, loadingTexture1, loadingTexture2, loadingTexture3, loadingTexture4, loadingTexture5, loadingTexture6, loadingTexture7, loadingTexture8, loadingTexture9, loadingTexture10, loadingTexture11, loadingTexture12, loadingTexture13, loadingTexture14, loadingTexture15, loadingTexture16, loadingTexture17, loadingTexture18, loadingTexture19, loadingTexture20, loadingTexture21, loadingTexture22, loadingTexture23, loadingTexture24, loadingTexture25, loadingTexture26;
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
			loadingTexture6 	= new TextureInfo("/Application/textures/LoadingScreens/Level6Load.png");
			loadingTexture7 	= new TextureInfo("/Application/textures/LoadingScreens/Level7Load.png");
			loadingTexture8 	= new TextureInfo("/Application/textures/LoadingScreens/Level8Load.png");
			loadingTexture9 	= new TextureInfo("/Application/textures/LoadingScreens/Level9Load.png");
			loadingTexture10 	= new TextureInfo("/Application/textures/LoadingScreens/Level10Load.png");
			loadingTexture11 	= new TextureInfo("/Application/textures/LoadingScreens/Level11Load.png");
			loadingTexture12 	= new TextureInfo("/Application/textures/LoadingScreens/Level12Load.png");
			loadingTexture13 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture14 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture15 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture16 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture17 	= new TextureInfo("/Application/textures/LoadingScreens/Level0Load.png");
			//loadingTexture18	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture19 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture20 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture21 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture22 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture23 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture24 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture25 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture26 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			
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
				else if (level == 6)
				{
					sprite.TextureInfo = loadingTexture6;
				}
				else if (level == 7)
				{
					sprite.TextureInfo = loadingTexture7;
				}
				else if (level == 8)
				{
					sprite.TextureInfo = loadingTexture8;
				}
				else if (level == 9)
				{
					sprite.TextureInfo = loadingTexture9;
				}
				else if (level == 10)
				{
					sprite.TextureInfo = loadingTexture10;
				}
				else if (level == 11)
				{
					sprite.TextureInfo = loadingTexture11;
				}
				else if (level == 12)
				{
					sprite.TextureInfo = loadingTexture12;
				}
				else if (level == 13)
				{
					sprite.TextureInfo = loadingTexture13;
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
		
		public void ReLoadTextures()
		{
			textureInfo 	= new TextureInfo("/Application/textures/LoadingScreens/Level0Load.png");
			loadingTexture0 	= new TextureInfo("/Application/textures/LoadingScreens/Level0Load.png");
			loadingTexture1 	= new TextureInfo("/Application/textures/LoadingScreens/Level1Load.png");
			loadingTexture2 	= new TextureInfo("/Application/textures/LoadingScreens/Level2Load.png");
			loadingTexture3 	= new TextureInfo("/Application/textures/LoadingScreens/Level3Load.png");
			loadingTexture4 	= new TextureInfo("/Application/textures/LoadingScreens/Level4Load.png");
			loadingTexture5 	= new TextureInfo("/Application/textures/LoadingScreens/Level5Load.png");
			loadingTexture6 	= new TextureInfo("/Application/textures/LoadingScreens/Level6Load.png");
			loadingTexture7 	= new TextureInfo("/Application/textures/LoadingScreens/Level7Load.png");
			loadingTexture8 	= new TextureInfo("/Application/textures/LoadingScreens/Level8Load.png");
			loadingTexture9 	= new TextureInfo("/Application/textures/LoadingScreens/Level9Load.png");
			loadingTexture10 	= new TextureInfo("/Application/textures/LoadingScreens/Level10Load.png");
			loadingTexture11 	= new TextureInfo("/Application/textures/LoadingScreens/Level11Load.png");
			loadingTexture12 	= new TextureInfo("/Application/textures/LoadingScreens/Level12Load.png");
			loadingTexture13 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture14 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture15 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture16 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture17 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");//
			//loadingTexture18	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture19 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture20 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture21 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture22 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture23 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture24 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture25 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			//loadingTexture26 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
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
			loadingTexture6.Dispose();
			loadingTexture7.Dispose();
			loadingTexture8.Dispose();
			loadingTexture9.Dispose();
			loadingTexture10.Dispose();
			loadingTexture11.Dispose();
			loadingTexture12.Dispose();
			loadingTexture13.Dispose();
		}
	}
}

