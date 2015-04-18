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
	//Our Background class V2.0 by @AS
	public class LevelComplete : Screen
	{
		private TextureInfo stars1Texture; //The background texture
		private TextureInfo stars2Texture; //The background texture
		private TextureInfo stars3Texture; //The background texture
		private SpriteUV starsSprite; //The background sprite
		
		private static Button backToLevelSelect;
		private static Button replayLevel;
		private static Button nextLevel;
		
		private enum State {WAITING, BACKTOLEVELSELECT, REPLAYLEVEL, NEXTLEVEL};
		private static State nextState;
				
		private bool play = false;

		public LevelComplete (Sce.PlayStation.HighLevel.GameEngine2D.Scene scene, Sce.PlayStation.HighLevel.UI.Scene uiScene) : base(scene)
		{
			textureInfo 	= new TextureInfo("/Application/textures/levelComplete.png");
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);
			sprite.Quad.S 	= textureInfo.TextureSizef;
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
			
			CustomButtonImageSettings customNextLevelImg = new CustomButtonImageSettings();
			customNextLevelImg.BackgroundNormalImage = new ImageAsset("/Application/assets/buttons/NewNext.png");
			customNextLevelImg.BackgroundPressedImage = new ImageAsset("/Application/assets/buttons/NewNext.png");
			customNextLevelImg.BackgroundDisabledImage = new ImageAsset("/Application/assets/buttons/NewNext.png");
			
			CustomButtonImageSettings customReplayImg = new CustomButtonImageSettings();
			customReplayImg.BackgroundNormalImage = new ImageAsset("/Application/assets/buttons/NewReplay.png");
			customReplayImg.BackgroundPressedImage = new ImageAsset("/Application/assets/buttons/NewReplay.png");
			customReplayImg.BackgroundDisabledImage = new ImageAsset("/Application/assets/buttons/NewReplay.png");
			
			CustomButtonImageSettings customBackImg = new CustomButtonImageSettings();
			customBackImg.BackgroundNormalImage = new ImageAsset("/Application/assets/buttons/NewHome.png");
			customBackImg.BackgroundPressedImage = new ImageAsset("/Application/assets/buttons/NewHome.png");
			customBackImg.BackgroundDisabledImage = new ImageAsset("/Application/assets/buttons/NewHome.png");
			
			backToLevelSelect = new Button();
			backToLevelSelect.SetPosition(750, 480);
			//backToLevelSelect.Text = "B";
			backToLevelSelect.SetSize(60,60);
			//backToLevelSelect.BackgroundNinePatchMargin = new NinePatchMargin(10,10,10,10);
			backToLevelSelect.CustomImage = customBackImg;
			backToLevelSelect.Style = ButtonStyle.Custom;
			//backToLevelSelect.BackgroundFilterColor = new UIColor(255.0f, 255.0f, 0.0f, 1.0f);
			backToLevelSelect.ButtonAction += HandleBackButton;
			backToLevelSelect.Visible = false;
			uiScene.RootWidget.AddChildLast(backToLevelSelect);
			
			replayLevel = new Button();
			replayLevel.SetPosition(820, 480);
			//replayLevel.Text = "R";
			replayLevel.SetSize(60,60);
			replayLevel.CustomImage = customReplayImg;
			replayLevel.Style = ButtonStyle.Custom;
			//replayLevel.BackgroundFilterColor = new UIColor(255.0f, 255.0f, 0.0f, 1.0f);
			replayLevel.ButtonAction += HandleReplayButton;
			replayLevel.Visible = false;
			uiScene.RootWidget.AddChildLast(replayLevel);
			
			nextLevel = new Button();
			nextLevel.SetPosition(890, 480);
			//nextLevel.Text = "N";
			nextLevel.SetSize(60,60);
			nextLevel.CustomImage = customNextLevelImg;
			nextLevel.Style = ButtonStyle.Custom;
			//nextLevel.BackgroundFilterColor = new UIColor(255.0f, 255.0f, 0.0f, 1.0f);
			nextLevel.ButtonAction += HandleNextLevelButton;
			nextLevel.Visible = false;
			uiScene.RootWidget.AddChildLast(nextLevel);
			
			UISystem.SetScene(uiScene);
			nextState = State.WAITING;
		}
		
		public void Update()
		{
			//CheckInput();
		}
		
		public void Show(float playerX, float playerY, int stars, int level)
		{
			sprite.Position = new Vector2(playerX - (Director.Instance.GL.Context.GetViewport().Width/2), playerY-270);
			sprite.Visible = true;
			starsSprite.Position = new Vector2(sprite.Position.X + (sprite.TextureInfo.TextureSizef.X/2) - (starsSprite.TextureInfo.TextureSizef.X/2), sprite.Position.Y + 90);
			
			if(stars==1)
				starsSprite.TextureInfo = stars1Texture;
			if(stars==2)
				starsSprite.TextureInfo = stars2Texture;
			if(stars==3)
				starsSprite.TextureInfo = stars3Texture;
			
			starsSprite.Visible = true;
			backToLevelSelect.Visible = true;
			if(level != 26)
			nextLevel.Visible = true;
			replayLevel.Visible = true;
		}
		
		public void ReOrderZ(Sce.PlayStation.HighLevel.GameEngine2D.Scene scene)
		{
			sprite = null;
			sprite 			= new SpriteUV(textureInfo);
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(0.0f, 0.0f);
			scene.AddChild(sprite);
			sprite.Visible = false;
			
			starsSprite = null;
			starsSprite 			= new SpriteUV(stars1Texture);
			starsSprite.Quad.S 	= stars1Texture.TextureSizef;
			starsSprite.Position = new Vector2(0.0f, 0.0f);
			scene.AddChild(starsSprite);
			starsSprite.Visible = false;
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
		
		public void HandleNextLevelButton(object sender, TouchEventArgs e)
		{
		 	nextState = State.NEXTLEVEL;
		}
		
		public void HandleReplayButton(object sender, TouchEventArgs e)
		{
		 	nextState = State.REPLAYLEVEL;
		}
		
		public void HandleBackButton(object sender, TouchEventArgs e)
		{
		 	nextState = State.BACKTOLEVELSELECT;
		}
		
		public int GetState()
		{
			return (int)nextState;
		}
		
		public void HideScreen()
		{
			sprite.Visible = false;
			starsSprite.Visible = false;
			
			backToLevelSelect.Visible = false;
			nextLevel.Visible = false;
			replayLevel.Visible = false;
			nextState = State.WAITING;
		}
		
//		
//		private void RemoveAll()
//		{
//			scene1.RemoveChild(sprite, true);
//		}
//		
		public new void Dispose()
		{
			textureInfo.Dispose();
			stars1Texture.Dispose();
			stars2Texture.Dispose();
			stars3Texture.Dispose();
		}
	}
}