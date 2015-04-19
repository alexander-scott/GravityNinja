using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;
using System.Xml.Serialization;
using System.Xml;

namespace GravityDuck
{ 
	//Our Background class V1.0 by @AS
	public class TitleScreen : Screen
	{
		private TextureInfo playTexture; //The background texture
		private TextureInfo playSelectTexture;
		private SpriteUV playSprite; //The background sprite
		
		private TextureInfo controlTexture;
		private TextureInfo controlSelectTexture;//The background texture
		private SpriteUV controlSprite; //The background sprite

		private Dialog dialog;
		private Sce.PlayStation.HighLevel.UI.Label	label;
		private EditableText command;
		private Button backButton;
		private Button inputButton;
		
		private bool play = false;
		
		private bool highestLevelChanged = false;
		private int newHighestLevel = 1;
		
		private Sce.PlayStation.HighLevel.GameEngine2D.Scene scene1;
		
		private Bounds2 playBox;
		private Bounds2 controlsBox;
		
		private static string SAVE_DATA = "/Documents/savedata.xml";
		
		bool options;
		
		public TitleScreen (Sce.PlayStation.HighLevel.GameEngine2D.Scene scene, Sce.PlayStation.HighLevel.UI.Scene uiScene) : base(scene)
		{
			scene1 = scene;

			textureInfo 	= new TextureInfo("/Application/textures/GravityNinja.png");
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(0.0f, 0.0f);
			
			playTexture 		= new TextureInfo("/Application/textures/play.png");
			playSelectTexture 	= new TextureInfo("/Application/textures/playSelected.png");
			playSprite 			= new SpriteUV();
			playSprite 			= new SpriteUV(playSelectTexture);
			playSprite.Quad.S 	= playTexture.TextureSizef*0.7f;
			playSprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.78f - (playTexture.TextureSizef.X/2),200);
				
			controlTexture 	= new TextureInfo("/Application/textures/cog.png");
			controlSprite 			= new SpriteUV();
			controlSprite 			= new SpriteUV(controlTexture);
			controlSprite.Quad.S 	= controlTexture.TextureSizef*0.05f;
			controlSprite.Position = new Vector2(10.0f , 10.0f);
			
			playBox.Min = playSprite.Position;
			playBox.Max = playSprite.Position + playSprite.TextureInfo.TextureSizef;
			
			controlsBox.Min = controlSprite.Position;
			controlsBox.Max = controlSprite.Position + controlSprite.TextureInfo.TextureSizef*0.05f;
			
			label = new Sce.PlayStation.HighLevel.UI.Label();
			label.SetPosition(10,90);
			label.Text = "Awaiting Input...";
			
			backButton = new Button();
			backButton.SetPosition(10, 140);
			backButton.SetSize(100,50);
			backButton.Text = "Back";
			backButton.ButtonAction += HandleButtonAction;
			
			inputButton = new Button();
			inputButton.SetPosition(380, 10);
			inputButton.SetSize(100,50);
			inputButton.Text = "Enter";
			inputButton.ButtonAction += HandleInputAction;
			
			command = new EditableText();
			command.SetPosition(10, 10);
			command.Text = "Enter Command";
			
			dialog = new Dialog();
			dialog.SetSize(500, 200);
			dialog.ShowEffect = new BunjeeJumpEffect(dialog, 0.4f);
			dialog.HideEffect = new TiltDropEffect();
			dialog.AddChildFirst(label);
			dialog.AddChildFirst(command);
			dialog.AddChildFirst(backButton);
			dialog.AddChildFirst(inputButton);
			
			scene.AddChild(sprite);
			scene.AddChild(playSprite);
			scene.AddChild(controlSprite);
			
			UISystem.SetScene(uiScene);
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
			if (!options)
			{
				if(touchBox.Overlaps(playBox) && touches.Count != 0)
				{
					play = true;
					RemoveAll();
				}
				
				if(touchBox.Overlaps(controlsBox) && touches.Count != 0)
				{
					//options = true;
					//optionScreen.Show();
					dialog.Show();
					options = true;
				}
				
				if (Input2.GamePad0.Cross.Release && playSprite.TextureInfo == playSelectTexture)
				{
					play = true;
					RemoveAll();
				}
				
				if (Input2.GamePad0.Down.Release)
				{
					playSprite.TextureInfo = playTexture;
					controlSprite.TextureInfo = controlTexture;
				}
				
				if (Input2.GamePad0.Right.Release)
				{
					controlSprite.TextureInfo = controlSelectTexture;
					playSprite.TextureInfo = playTexture;
				}
				
				if (Input2.GamePad0.Left.Release)
				{
					controlSprite.TextureInfo = controlTexture;
					playSprite.TextureInfo = playTexture;
				}
				
				if (Input2.GamePad0.Up.Release)
				{
					controlSprite.TextureInfo = controlTexture;
					playSprite.TextureInfo = playSelectTexture;
	
				}
			} else
			{
				//if (!optionScreen.CheckOptions())
				//	options = false;
			}
				
				
		}
		
		public bool CheckPlay()
		{
			return play;
		}
		
		public void RemoveAll()
		{
			scene1.RemoveChild(sprite, true);
			scene1.RemoveChild(playSprite, true);
			scene1.RemoveChild(controlSprite, true);
		}
		
		public void HandleButtonAction(object sender, TouchEventArgs e)
		{
			dialog.Hide();
			options = false;
		}
		
		public void HandleInputAction(object sender, TouchEventArgs e)
		{
			string inputString = command.Text;
			if (inputString.ToLower() == "reset")
			{
				label.Text = "Progress reset!";
				command.Text = "Enter Command";
				UpdateData(false);
				newHighestLevel = 0;
				highestLevelChanged = true;
			}
			else if (inputString.ToLower() == "unlock")
			{
				label.Text = "All levels unlocked!";
				command.Text = "Enter Command";
				UpdateData(true);
				newHighestLevel = 26;
				highestLevelChanged = true;
			}
			else
			{
				label.Text = "Invalid command.";
				command.Text = "Enter Command";
			}
			
		}
		
		
//		private void LoadingLevel()
//		{
//			sprite.TextureInfo = loadingTexture;
//			scene1.RemoveChild(playSprite, true);
//			scene1.RemoveChild(controlSprite, true);
//			scene1.RemoveChild(hiscoreSprite, true);
//		}
		
		public new void Dispose()
		{
			textureInfo.Dispose();
			playTexture.Dispose();
			controlTexture.Dispose();
				
		}
		
		public bool HighestLevelChanged()
		{
			return highestLevelChanged;
		}
		
		public int NewHighestLevel()
		{
			highestLevelChanged = false;
			return newHighestLevel;
		}
		
		public static bool UpdateData(bool choice)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(@SAVE_DATA);
					
			for(int i = 0; i < 27; i++)
			{		
				XmlNode currentNode = doc.SelectSingleNode("game/level[@id=\"" + (i + 1).ToString() + "\"]");
				
				if(currentNode != null)
				{
					if(choice)
						currentNode.ChildNodes.Item(0).InnerText = "unlocked";
					else
						currentNode.ChildNodes.Item(0).InnerText = "locked";
					for(int j = 0; j < 5; j++)
					{
						currentNode.ChildNodes.Item(1 + (j * 2)).InnerText = "";
						currentNode.ChildNodes.Item(2 + (j * 2)).InnerText = "";	
					}		
				}
				//// Reset all values
				//doc.SelectSingleNode("game/level[@id=\"" + (i + 1).ToString() + "\"]").ChildNodes.Item(0).InnerText = "";
				//doc.SelectSingleNode("game/level[@id=\"" + (i + 1).ToString() + "\"]").ChildNodes.Item(1).InnerText = "";
					
		        doc.Save(@SAVE_DATA);			
			}
			
			return true;
		}
	}
}