using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravDuck
{
	// Comment Signatures
	
	// Alisdair Wright : AW
	// Rui Mitchell Da Silva : RMDS
	// Alex Scott : AS
	// Sam Murphy : SM
	// Nile McMorrow : NM
	
	public class AppMain
	{
		private static GameScene gameScene;
		
		public static void Main (string[] args)
		{
			Initialize ();

			while (true) 
			{
				SystemEvents.CheckEvents ();
				Director.Instance.Update();
				Director.Instance.Render();
				
				Update();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
			}
		}

		public static void Initialize ()
		{
			Director.Initialize ();	

			// Create GameSTate
			gameScene = new GameScene();
			Director.Instance.RunWithScene(gameScene, true);	
		}

		public static void Update ()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);
			gameScene.Update();
		}

		public static void Render ()
		{
		}
	}
}
