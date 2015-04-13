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
		private TextureInfo loadingTexture0, loadingTexture1, loadingTexture2, loadingTexture3, loadingTexture4, loadingTexture5;
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
	
			loadingTexture0 	= new TextureInfo("/Application/textures/LoadingScreens/Level0Load.png");
			loadingTexture1 	= new TextureInfo("/Application/textures/LoadingScreens/Level1Load.png");
			loadingTexture2 	= new TextureInfo("/Application/textures/LoadingScreens/Level2Load.png");
			loadingTexture3 	= new TextureInfo("/Application/textures/LoadingScreens/Level3Load.png");
			loadingTexture4 	= new TextureInfo("/Application/textures/LoadingScreens/Level4Load.png");
			loadingTexture5 	= new TextureInfo("/Application/textures/LoadingScreens/Level5Load.png");
			
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
					sprite.TextureInfo = loadingTexture0;
				}	
				else if (levelSelected == 1)
				{
					sprite.TextureInfo = loadingTexture1;
				}
				else if (levelSelected == 2)
				{
					sprite.TextureInfo = loadingTexture2;
				}
				else if (levelSelected == 3)
				{
					sprite.TextureInfo = loadingTexture3;
				}
				else if (levelSelected == 4)
				{
					sprite.TextureInfo = loadingTexture4;
				}
				else if (levelSelected == 5)
				{
					sprite.TextureInfo = loadingTexture5;
				}
			}		
		}
		
		public void SetVisible(bool vis, int level)
		{
			visible = vis;
			if (vis)
			{
				sprite.TextureInfo = textureInfo;
				sprite.Visible = true;
				backButton.Visible = true;
				for (int i = 0; i < numOfLevels; i++)
				{
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

