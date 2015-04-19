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
		private static BusyIndicator loadingSymbol;
		private static Button readyButton;
		private static Button readyButton2;
		
		private Bounds2 startBox;
		private bool play = false;
		
		private int loadTime = 0;
		private int level = 0;

		private Sce.PlayStation.HighLevel.UI.Label	loadingLabel;
		private Sce.PlayStation.HighLevel.UI.Label	levelLabel;
		
		public LoadingScreen (Sce.PlayStation.HighLevel.GameEngine2D.Scene scene, Sce.PlayStation.HighLevel.UI.Scene uiScene) : base(scene)
		{
//			loadingTexture0 	= new TextureInfo("/Application/textures/LoadingScreens/Level0Load.png");
//			loadingTexture1 	= new TextureInfo("/Application/textures/LoadingScreens/Level1Load.png");
//			loadingTexture2 	= new TextureInfo("/Application/textures/LoadingScreens/Level2Load.png");
//			loadingTexture3 	= new TextureInfo("/Application/textures/LoadingScreens/Level3Load.png");
//			loadingTexture4 	= new TextureInfo("/Application/textures/LoadingScreens/Level4Load.png");
//			loadingTexture5 	= new TextureInfo("/Application/textures/LoadingScreens/Level5Load.png");
//			loadingTexture6 	= new TextureInfo("/Application/textures/LoadingScreens/Level6Load.png");
//			loadingTexture7 	= new TextureInfo("/Application/textures/LoadingScreens/Level7Load.png");
//			loadingTexture8 	= new TextureInfo("/Application/textures/LoadingScreens/Level8Load.png");
//			loadingTexture9 	= new TextureInfo("/Application/textures/LoadingScreens/Level9Load.png");
//			loadingTexture10 	= new TextureInfo("/Application/textures/LoadingScreens/Level10Load.png");
//			loadingTexture11 	= new TextureInfo("/Application/textures/LoadingScreens/Level11Load.png");
//			loadingTexture12 	= new TextureInfo("/Application/textures/LoadingScreens/Level12Load.png");
//			loadingTexture13 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture14 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture15 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture16 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture17 	= new TextureInfo("/Application/textures/LoadingScreens/Level0Load.png");
//			//loadingTexture18	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture19 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture20 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture21 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture22 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture23 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture24 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture25 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture26 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			
		 	textureInfo 	= AppMain.loadingTexture0;
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
			levelLabel.TextColor = new UIColor(1.0f, 1.0f, 1.0f, 1.0f);
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
			
			readyButton2 = new Button();
			readyButton2.SetPosition(190, 450);
			readyButton2.Text = "JOIN X";
			readyButton2.SetSize(88,48);
			readyButton2.BackgroundFilterColor = new UIColor(255.0f, 255.0f, 0.0f, 1.0f);
			readyButton2.ButtonAction += HandleButtonAction;
			readyButton2.Visible = false;
			uiScene.RootWidget.AddChildLast(readyButton2);
			
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
				if(level == 26)
					readyButton2.Visible = true;
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
			this.level = level;
			if (visible)
			{
				sprite.Visible = true;
				if(level != 26)
				{
					loadingLabel.Visible = true;
					loadingSymbol.Visible = true;
				}
				levelLabel.Visible = true;
				levelLabel.Text = "L" + level;
				readyButton.SetPosition(860, 490);
				readyButton.Text = "PLAY";
				if (level == 0)
				{
					sprite.TextureInfo = AppMain.loadingTexture0;
				}	
				else if (level == 1)
				{
					sprite.TextureInfo = AppMain.loadingTexture1;
				}
				else if (level == 2)
				{
					sprite.TextureInfo = AppMain.loadingTexture2;
				}
				else if (level == 3)
				{
					sprite.TextureInfo = AppMain.loadingTexture3;
				}
				else if (level == 4)
				{
					sprite.TextureInfo = AppMain.loadingTexture4;
				}
				else if (level == 5)
				{
					sprite.TextureInfo = AppMain.loadingTexture5;
				}
				else if (level == 6)
				{
					sprite.TextureInfo = AppMain.loadingTexture6;
				}
				else if (level == 7)
				{
					sprite.TextureInfo = AppMain.loadingTexture7;
				}
				else if (level == 8)
				{
					sprite.TextureInfo = AppMain.loadingTexture8;
				}
				else if (level == 9)
				{
					sprite.TextureInfo = AppMain.loadingTexture9;
				}
				else if (level == 10)
				{
					sprite.TextureInfo = AppMain.loadingTexture10;
				}
				else if (level == 11)
				{
					sprite.TextureInfo = AppMain.loadingTexture11;
				}
				else if (level == 12)
				{
					sprite.TextureInfo = AppMain.loadingTexture12;
				}
				else if (level == 13)
				{
					sprite.TextureInfo = AppMain.loadingTexture13;
				}
				else if (level == 14)
				{
					sprite.TextureInfo = AppMain.loadingTexture14;
				}
				else if (level == 15)
				{
					sprite.TextureInfo = AppMain.loadingTexture15;
				}
				else if (level == 16)
				{
					sprite.TextureInfo = AppMain.loadingTexture16;
				}
				else if (level == 17)
				{
					sprite.TextureInfo = AppMain.loadingTexture17;
				}
				else if (level == 18)
				{
					sprite.TextureInfo = AppMain.loadingTexture18;
				}
				else if (level == 19)
				{
					sprite.TextureInfo = AppMain.loadingTexture19;
				}
				else if (level == 20)
				{
					sprite.TextureInfo = AppMain.loadingTexture20;
				}
				else if (level == 21)
				{
					sprite.TextureInfo = AppMain.loadingTexture21;
				}
				else if (level == 22)
				{
					sprite.TextureInfo = AppMain.loadingTexture22;
				}
				else if (level == 23)
				{
					sprite.TextureInfo = AppMain.loadingTexture23;
				}
				else if (level == 24)
				{
					sprite.TextureInfo = AppMain.loadingTexture24;
				}
				else if (level == 25)
				{
					sprite.TextureInfo = AppMain.loadingTexture25;
				}
				else if (level == 26)
				{
					readyButton.SetPosition(700,450);
					readyButton.Text = "JOIN DD";
					sprite.TextureInfo = AppMain.loadingTexture26;
				}
			}
			else
			{
				sprite.Visible = false;
				loadingLabel.Visible = false;
				loadingSymbol.Visible = false;
				readyButton.Visible = false;
				readyButton2.Visible = false;
				levelLabel.Visible = false;
				play = false;
			}
		}
		
		public void SetLoadTime(int time)
		{
			loadTime = time;
		}
	}
}

