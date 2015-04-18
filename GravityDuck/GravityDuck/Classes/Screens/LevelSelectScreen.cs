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
	public class LevelSelectScreen : Screen
	{
		//public TextureInfo loadingTexture0, loadingTexture1, loadingTexture2, loadingTexture3, loadingTexture4, loadingTexture5, loadingTexture6, loadingTexture7, loadingTexture8, loadingTexture9, loadingTexture10, loadingTexture11, loadingTexture12, loadingTexture13, loadingTexture14, loadingTexture15, loadingTexture16, loadingTexture17, loadingTexture18, loadingTexture19, loadingTexture20, loadingTexture21, loadingTexture22, loadingTexture23, loadingTexture24, loadingTexture25, loadingTexture26;
		private Button[] levelButtons;
		private Button backButton;
		private bool visible = false;
		private bool play = false;
		private bool back = false;
		private static int numOfLevels = 27;
		private static int highestUnlockedLevel = 0;
		
		public int levelSelected = 0;
		
		public LevelSelectScreen (Sce.PlayStation.HighLevel.GameEngine2D.Scene scene, Sce.PlayStation.HighLevel.UI.Scene uiScene, int maxLevel) : base(scene)
		{
			highestUnlockedLevel = maxLevel;
			textureInfo 	= new TextureInfo("/Application/textures/LevelSelectBackground.png");
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(0.0f, 0.0f);
//	
//			loadingTexture0 	= AppMain.loadingTexture0;
//			loadingTexture1 	= AppMain.loadingTexture1;
//			loadingTexture2 	= AppMain.loadingTexture2;
//			loadingTexture3 	= AppMain.loadingTexture3;
//			loadingTexture4 	= AppMain.loadingTexture4;
//			loadingTexture5 	= AppMain.loadingTexture5;
//			loadingTexture6 	= AppMain.loadingTexture6;
//			loadingTexture7 	= AppMain.loadingTexture7;
//			loadingTexture8 	= AppMain.loadingTexture8;
//			loadingTexture9 	= new TextureInfo("/Application/textures/LoadingScreens/Level9Load.png");
//			loadingTexture10 	= new TextureInfo("/Application/textures/LoadingScreens/Level10Load.png");
//			loadingTexture11 	= new TextureInfo("/Application/textures/LoadingScreens/Level11Load.png");
//			loadingTexture12 	= new TextureInfo("/Application/textures/LoadingScreens/Level12Load.png");
//			loadingTexture13 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture14 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture15 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture16 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture17 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");//
//			//loadingTexture18	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture19 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture20 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture21 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture22 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture23 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture24 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture25 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
//			//loadingTexture26 	= new TextureInfo("/Application/textures/LoadingScreens/Level13Load.png");
			
//			Sequence seq = new Sequence();
//			TintTo fadeIn = new TintTo (new Vector4 (1.0f, 1.0f, 1.0f, 1.0f), 1.0f);
//			TintTo fadeOut = new TintTo (new Vector4 (1.0f, 1.0f, 1.0f, 0.0f), 1.0f);
//			fadeIn.Tween = new DTween(f => f);
//			fadeOut.Tween = new DTween(f => f);
//			seq.Add(fadeIn);
//			seq.Add(fadeOut);
//			RepeatForever flash = new RepeatForever();
//			flash.InnerAction = seq;
			
			levelButtons = new Button[numOfLevels];
			
			for (int i = 0; i < numOfLevels; i++)
			{
				levelButtons[i] = new Button();
				levelButtons[i].Text = i.ToString();
				levelButtons[i].Name = i.ToString();
				levelButtons[i].ButtonAction += HandleButtonAction;
				if (i <= 8)
				{
					levelButtons[i].SetPosition(108f * i + 18, 180f);
				}
				if (i > 8 && i <= 17)
				{
					levelButtons[i].SetPosition(108f * (i-9) + 18, 280f);
				}
				if (i > 17 && i <= 26)
				{
					levelButtons[i].SetPosition(108f * (i-18) + 18, 380f);
				}
				if (i < highestUnlockedLevel)
						levelButtons[i].BackgroundFilterColor = new UIColor(0.0f, 255.0f, 0.0f, 1.0f);
				if (i == highestUnlockedLevel)
					levelButtons[i].BackgroundFilterColor = new UIColor(0.0f, 191.0f, 255.0f, 1.0f);
				
				levelButtons[i].SetSize(65,65);
				uiScene.RootWidget.AddChildFirst(levelButtons[i]);
			}
			
			backButton = new Button();
			backButton.SetPosition(10, 485);
			backButton.SetSize(100, 50);
			backButton.Text = "BACK";
			backButton.BackgroundFilterColor = new UIColor(255.0f, 255.0f, 0.0f, 1.0f);
			backButton.ButtonAction += HandleBackButtonAction;
			uiScene.RootWidget.AddChildLast(backButton);
				
			scene.AddChild(sprite);
			UISystem.SetScene(uiScene);
		}
		
		public void ReOrderZ(Sce.PlayStation.HighLevel.GameEngine2D.Scene scene)
		{
			sprite 			= null;
			sprite 			= new SpriteUV(textureInfo);
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(0.0f, 0.0f);
			scene.AddChild(sprite);
		}
		
		public void Update()
		{
			back = false;
		}
		
		public bool Selected()
		{
			return play;
		}
		
		public bool BackPressed()
		{
			return back;
		}
		
		public void HandleBackButtonAction(object sender, TouchEventArgs e)
		{
			back = true;
		}
		
		public void HandleButtonAction(object sender, TouchEventArgs e)
		{
			string temp = sender.ToString();
			string b = string.Empty;
			for (int i=0; i< temp.Length; i++)
			{
			    if (Char.IsDigit(temp[i]))
			        b += temp[i];
			}
			
			if (b.Length>0)
			    levelSelected = int.Parse(b);
			
			if (levelSelected <= highestUnlockedLevel)
			{
				play = true;
				//backButton.Dispose();
				backButton.Visible = false;
				for (int i = 0; i < numOfLevels; i++)
				{
					//levelButtons[i].Dispose();
					levelButtons[i].Visible = false;
				}
			}
			if (levelSelected <= highestUnlockedLevel)
			{
				if (levelSelected == 0)
				{
					sprite.TextureInfo = AppMain.loadingTexture0;
				}	
				else if (levelSelected == 1)
				{
					sprite.TextureInfo = AppMain.loadingTexture1;
				}
				else if (levelSelected == 2)
				{
					sprite.TextureInfo = AppMain.loadingTexture2;
				}
				else if (levelSelected == 3)
				{
					sprite.TextureInfo = AppMain.loadingTexture3;
				}
				else if (levelSelected == 4)
				{
					sprite.TextureInfo = AppMain.loadingTexture4;
				}
				else if (levelSelected == 5)
				{
					sprite.TextureInfo = AppMain.loadingTexture5;
				}
				else if (levelSelected == 6)
				{
					sprite.TextureInfo = AppMain.loadingTexture6;
				}
				else if (levelSelected == 7)
				{
					sprite.TextureInfo = AppMain.loadingTexture7;
				}
				else if (levelSelected == 8)
				{
					sprite.TextureInfo = AppMain.loadingTexture8;
				}
				else if (levelSelected == 9)
				{
					sprite.TextureInfo = AppMain.loadingTexture9;
				}
				else if (levelSelected == 10)
				{
					sprite.TextureInfo = AppMain.loadingTexture10;
				}
				else if (levelSelected == 11)
				{
					sprite.TextureInfo = AppMain.loadingTexture11;
				}
				else if (levelSelected == 12)
				{
					sprite.TextureInfo = AppMain.loadingTexture12;
				}
				else if (levelSelected == 13)
				{
					sprite.TextureInfo = AppMain.loadingTexture13;
				}
				else if (levelSelected == 14)
				{
					sprite.TextureInfo = AppMain.loadingTexture14;
				}
				else if (levelSelected == 15)
				{
					sprite.TextureInfo = AppMain.loadingTexture15;
				}
				else if (levelSelected == 16)
				{
					sprite.TextureInfo = AppMain.loadingTexture16;
				}
				else if (levelSelected == 17)
				{
					sprite.TextureInfo = AppMain.loadingTexture17;
				}
				else if (levelSelected == 18)
				{
					sprite.TextureInfo = AppMain.loadingTexture18;
				}
				else if (levelSelected == 19)
				{
					sprite.TextureInfo = AppMain.loadingTexture19;
				}
				else if (levelSelected == 20)
				{
					sprite.TextureInfo = AppMain.loadingTexture20;
				}
				else if (levelSelected == 21)
				{
					sprite.TextureInfo = AppMain.loadingTexture21;
				}
				else if (levelSelected == 22)
				{
					sprite.TextureInfo = AppMain.loadingTexture22;
				}
				else if (levelSelected == 23)
				{
					sprite.TextureInfo = AppMain.loadingTexture23;
				}
				else if (levelSelected == 24)
				{
					sprite.TextureInfo = AppMain.loadingTexture24;
				}
				else if (levelSelected == 25)
				{
					sprite.TextureInfo = AppMain.loadingTexture25;
				}
				else if (levelSelected == 26)
				{
					sprite.TextureInfo = AppMain.loadingTexture26;
				}
			}		
		}
		
		public void SetVisible(bool vis, int level)
		{
			visible = vis;
			highestUnlockedLevel = level;
			if (vis)
			{
				sprite.TextureInfo = textureInfo;
				sprite.Visible = true;
				backButton.Visible = true;
				for (int i = 0; i < numOfLevels; i++)
				{
					if (i < level)
						levelButtons[i].BackgroundFilterColor = new UIColor(0.0f, 255.0f, 0.0f, 1.0f);
					if (i == level)
						levelButtons[i].BackgroundFilterColor = new UIColor(0.0f, 191.0f, 255.0f, 1.0f);
					if (i > level)
						levelButtons[i].BackgroundFilterColor = new UIColor(0.0f, 0.0f, 0.0f, 1.0f);
					levelButtons[i].Visible = true;
				}
				
			}
			else
			{
				sprite.Visible = false;
				backButton.Visible = false;
				for (int i = 0; i < numOfLevels; i++)
				{
					levelButtons[i].Visible = false;
				}
				play = false;
				back = false;
			}
		}
	}
}

