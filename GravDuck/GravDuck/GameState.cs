using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravDuck
{
	public class GameScene : Scene
	{
		// Private		

		private BgmPlayer musicPlayer = new Bgm("/Application/audio/fight.mp3").CreatePlayer();
		
		//private SpriteUV loseBackground = new SpriteUV(new TextureInfo("/Application/textures/winLoseScreens/Lose/loseScreen.png"));
		//private SpriteUV restart = new SpriteUV(new TextureInfo("/Application/textures/winLoseScreens/Lose/restartY.png")); 
		//private SpriteUV quit = new SpriteUV(new TextureInfo("/Application/textures/winLoseScreens/Lose/saveQuitY.png")); 
		private static TouchStatus  currentTouchStatus;
		
		// All entities within the GameScene
		private static Sce.PlayStation.HighLevel.GameEngine2D.Label scoreLabel;
		private static Sce.PlayStation.HighLevel.GameEngine2D.Label highScoreLabel;
		private static Sce.PlayStation.HighLevel.GameEngine2D.Label waveLabel;
		
		private Player 				player;
		
		// Two TextureInfo variables so that more than one variable can be passed into entities
		private TextureInfo 		textureInfo1;
		private TextureInfo 		textureInfo2;
		
		private SpriteUV 			background;
		
		private SpriteUV			gravityArrow;
		
		// Score
		private static int			highScore = 0;
		private static int 			currentScore = 0;
		
		private static float        xComponent = 0.0f;
		private static float		yComponent = 0.0f;
		
		private Vector2 gravityVector = new Vector2(0.0f, 0.0f);
		
		private bool		release = false;
		
		// Public
		public GameScene()
		{
			this.Camera2D.SetViewFromViewport();						
				
			
			Matrix4 camera = this.Camera2D.GetTransform();

			//this.Camera2D.SetTouchPlaneMatrix(new Matrix4(new Vector4(1.0f, 0.0f, 0.0f, 0.0f),	  new Vector4(0.0f, 1.0f, 1.0f, 0.0f),
			//										new Vector4(0.0f, 0.0f, 1.0f, 0.0f),	new Vector4(480.0f, 272.0f, 0.0f, 2.0f)));
			
			this.Camera2D.SetViewY(new Vector2(0.0f, Director.Instance.GL.Context.GetViewport().Height * 0.5f),
			                       new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f,Director.Instance.GL.Context.GetViewport().Height*0.5f));
			
			camera = this.Camera2D.GetTransform();
			
			
			
			// Setup all entities and sprites    (0.0f * (background.Scale.X - 1.0f)
			
			// Background
			textureInfo1 		= new TextureInfo("/Application/textures/Arena.png");
			background 			= new SpriteUV();
			background 			= new SpriteUV(textureInfo1);
			background.Quad.S 	= textureInfo1.TextureSizef;
			background.Scale 	= new Vector2(5.0f, 5.0f);
			background.Pivot 	= new Vector2(background.Quad.S.X/2, background.Quad.S.Y/2);
			background.Position = new Vector2((Director.Instance.GL.Context.GetViewport().Width*0.5f) + background.Quad.S.X*1.75f,
			                                  (Director.Instance.GL.Context.GetViewport().Height*0.5f) - ((background.Quad.S.Y/2) - 55.0f));
			AddChild(background);
			
			
			textureInfo1 		= new TextureInfo("/Application/textures/Arrow.png");
			gravityArrow 			= new SpriteUV();
			gravityArrow 			= new SpriteUV(textureInfo1);
			gravityArrow.Quad.S 	= textureInfo1.TextureSizef;
			gravityArrow.Scale 		= new Vector2(1.0f, 1.0f);
			gravityArrow.Pivot 		= new Vector2(gravityArrow.Quad.S.X/2, gravityArrow.Quad.S.Y);
			gravityArrow.Position 	= new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f,
			                                  (Director.Instance.GL.Context.GetViewport().Height*0.5f) - gravityArrow.Quad.S.Y);
			AddChild(gravityArrow);
			
			
			// Player
			SpriteUV[,] playerSprites = new SpriteUV[2,2];
			playerSprites[0,0] = new SpriteUV(new TextureInfo("/Application/textures/PlayerWalk1.png"));
			playerSprites[0,1] = new SpriteUV(new TextureInfo("/Application/textures/PlayerWalk2.png"));
			playerSprites[1,0] = new SpriteUV(new TextureInfo("/Application/textures/PlayerFire1.png"));
			playerSprites[1,1] = new SpriteUV(new TextureInfo("/Application/textures/PlayerFire2.png"));
			
			textureInfo2 		= new TextureInfo("/Application/textures/Trans.png");
			player 				= new Player(this, playerSprites, textureInfo2);			
			
			// All scene objects
			textureInfo1	= new TextureInfo("/Application/textures/Trans.png");
			SetUpSceneObjects();
			

			// Labels
			scoreLabel = new Sce.PlayStation.HighLevel.GameEngine2D.Label();
			//scoreLabel.Color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
			scoreLabel.Position = new Vector2(670.0f, 450.0f);
			scoreLabel.Scale = new Vector2(2.5f, 2.5f);
			scoreLabel.Text = "Score: 0";
			AddChild(scoreLabel);
			
			highScoreLabel = new Sce.PlayStation.HighLevel.GameEngine2D.Label();
			//highScoreLabel.Color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
			highScoreLabel.Position = new Vector2(600.0f, 500.0f);
			highScoreLabel.Scale = new Vector2(2.5f, 2.5f);
			highScoreLabel.Text = "Highscore: " + highScore;
			AddChild(highScoreLabel);
			
			waveLabel = new Sce.PlayStation.HighLevel.GameEngine2D.Label();
			//waveLabel.Color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
			waveLabel.Position = new Vector2(50.0f, 500.0f);
			waveLabel.Scale = new Vector2(2.5f, 2.5f);
			waveLabel.Text = "Wave: ";
			this.AddChild(waveLabel);
			
			// Music
			musicPlayer.Volume = 0.0f;
			musicPlayer.LoopStart = 3.245d; 
			musicPlayer.LoopEnd = 54.425d;
			musicPlayer.Loop = true;
			musicPlayer.Play();
			
			Scheduler.Instance.ScheduleUpdateForTarget(this, 1, false);
		}
		
		public void Dispose()
		{
			// Cleanup
			textureInfo1.Dispose();
			textureInfo2.Dispose();
		}
		
		public void SetUpSceneObjects()
		{
		}

		public void Update(float dt)
		{	
			gravityVector = new Vector2(-xComponent * 0.01f, -FMath.Cos((xComponent * 0.5f) * FMath.PI) * 0.01f);		
			
			SortLabels();	
			
			if(release)
				player.Update(dt, gravityVector);
			else
				player.Update(dt, new Vector2(0.0f, 0.0f));
			
			CheckInput();
		}
		
		public void CheckInput()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData(0);
			

			xComponent += gamePadData.AnalogLeftX/100.0f;
			
			
			if(xComponent < 0.0f)
				yComponent += gamePadData.AnalogLeftX/-100.0f;
			else
				yComponent += gamePadData.AnalogLeftX/100.0f;
			
			
		//	if(gamePadData.AnalogLeftX < 0.0f)
		//	{
		//		if(xComponent < 0.0f)
		//			yComponent += gamePadData.AnalogLeftX/-100.0f;
		//		else
		//			yComponent += gamePadData.AnalogLeftX/100.0f;
		//	}
		//	else			
		//		yComponent += gamePadData.AnalogLeftX/100.0f;
				
			 
			if (xComponent > 1.0f)
				xComponent = 1.0f;
			else if (xComponent < -1.0f)
					xComponent = -1.0f;
			else{}
			
			if(yComponent < 0.0f)
				yComponent = 0;
			else if (yComponent > 1.0f)
					yComponent = 1.0f;
			else{}
		
			
			gravityArrow.Angle = (-xComponent * 0.5f) * FMath.PI;
			
			
			
			//xComponent = gamePadData.AnalogLeftX; 
			//
			//
			//if(gamePadData.AnalogLeftX < 0)
			//	yComponent = gamePadData.AnalogLeftX * -1.0f;
			//else
			//	yComponent = gamePadData.AnalogLeftX;
		
									
			//this.Camera2D.SetViewY(new Vector2(xComponent * Director.Instance.GL.Context.GetViewport().Width,
			//                                   (Director.Instance.GL.Context.GetViewport().Height * 0.5f) * (1.0f - (1.0f * xComponent))),
			//                    					new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f,
			//            								Director.Instance.GL.Context.GetViewport().Height*0.5f));	
			
						this.Camera2D.SetViewY(new Vector2(FMath.Sin(xComponent) * Director.Instance.GL.Context.GetViewport().Width,
			                                  (1.0f - yComponent) * Director.Instance.GL.Context.GetViewport().Height),
			                   					new Vector2(Director.Instance.GL.Context.GetViewport().Width*0.5f,
			           								Director.Instance.GL.Context.GetViewport().Height*0.5f));
			
			if((gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				release = true;
			}
			
			if((gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				bool d = true;
			}
		}
		
		public void CheckBoundaries()
		{	
		}
		
		public void CheckCollisions()
		{		
		}
		
		
		public void SortLabels()
		{
			currentScore = player.GetScore();
			
			if(currentScore > highScore)
				highScore = currentScore;
			
			scoreLabel.Text = "Score: " + currentScore;
			highScoreLabel.Text ="Highscore: " + highScore;
		}
		
		public void LoseScreen()
		{
		}
		
		public void Reset()
		{         			          
		}
		
		public int GetHighScore(){ return highScore; }
		
		public int GetScore(){ return currentScore; }
	}
}

	
