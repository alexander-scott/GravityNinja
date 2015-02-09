using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	//Our Maze class V2 by @AW && @AS
	public class Maze
	{
		private TextureInfo groundBlock1;
		private TextureInfo groundBlock2;
		
		private TextureInfo ceilingBlock1;
		private TextureInfo ceilingBlock2;
		
		private TextureInfo treeBlock;
		
		private int coinCount;
		private Coin[] coins;
		
		private int gemCount;
		private Gem[] gems;
		
		private int spikeCount;
		private Spikes[] spikes;
		
		private SpriteUV[,] 	sprites; //Each block is a sprite
		int[,] mazeLevel;
		private int mazeWidth, mazeHeight; //Width and height for the maze
						
		public Maze (Scene scene)
		{
			//Maze height and width
			mazeWidth = 8;
			mazeHeight = 11;
			coinCount = 20; 
			gemCount = 3;
			spikeCount = 6;
			
			//Load in the textures here
			//Ground Block Textures
			groundBlock1 = new TextureInfo("/Application/textures/Level/groundBlock1.png");
			groundBlock2 = new TextureInfo("/Application/textures/Level/groundBlock2.png");
			
			//Ceiling Block Textures
			ceilingBlock1 = new TextureInfo("/Application/textures/Level/ceilingBlock1.png");
			ceilingBlock2 = new TextureInfo("/Application/textures/Level/ceilingBlock2.png");
			
			treeBlock = new TextureInfo("/Application/textures/Level/treeBlock.png");
			
			sprites	= new SpriteUV[mazeWidth, mazeHeight]; //Initalise the sprites
						
			int[,] mazeLevel1 = { {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5},
						     	  {5, 1, 0, 3, 1, 0, 0, 0, 0, 3, 5},
							      {5, 2, 0, 4, 2, 0, 3, 1, 0, 4, 5},
							  	  {5, 1, 0, 3, 1, 0, 4, 2, 0, 3, 5},
							 	  {5, 2, 0, 4, 2, 0, 3, 1, 0, 4, 5},
							 	  {5, 1, 0, 3, 1, 0, 4, 2, 0, 3, 5},
							  	  {5, 2, 0, 0, 0, 0, 3, 1, 0, 4, 5},
							  	  {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5} };
			
			//Initislise and position spikes
			spikes = new Spikes[spikeCount];
			
			
			spikes[0] = new Spikes(scene, 2);
			spikes[1] = new Spikes(scene, 1);
			spikes[2] = new Spikes(scene, 1);
			spikes[3] = new Spikes(scene, 2);
			spikes[4] = new Spikes(scene, 1);
			
			spikes[0].setPosition(new Vector2(500.0f, 215.0f));
			spikes[1].setPosition(new Vector2(690.0f, 690.0f));
			spikes[1].getSprite().Rotate(-1.5707963268f);
			spikes[2].setPosition(new Vector2(360.0f, 620.0f));
			spikes[3].setPosition(new Vector2(50.0f, 1075.0f));
			spikes[3].getSprite().Rotate(-1.5707963268f);
			spikes[4].setPosition(new Vector2(270.0f, 1050.0f));
			
			//Initialise maze tiles
			for (int i = 0; i < mazeWidth; ++i) //Basic tile engine
			{
    			for (int j = 0; j < mazeHeight; ++j) //Goes through every number/coordinate in the array
				{
					if (mazeLevel1[i,j] == 1) //If the tile has an ID of 1 it's a rigid block
					{
						sprites[i,j] 		  = new SpriteUV(groundBlock1);
						sprites[i,j].Quad.S   = groundBlock1.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel1[i,j] == 2) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(groundBlock2);
						sprites[i,j].Quad.S   = groundBlock2.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel1[i,j] == 3) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(ceilingBlock1);
						sprites[i,j].Quad.S   = ceilingBlock1.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel1[i,j] == 4) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(ceilingBlock2);
						sprites[i,j].Quad.S   = ceilingBlock2.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel1[i,j] == 5) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(treeBlock);
						sprites[i,j].Quad.S   = treeBlock.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else
					{
						sprites[i,j] = new SpriteUV(null);
					}
				}
			}
			
			//Initialise and position coins
			coins = new Coin[coinCount];
			
			for (int i = 0; i <= coinCount - 1; i++)
			{
				coins[i] = new Coin(scene);
			}
			
			Vector2 position = new Vector2(200.0f, 1000.0f);
			
			coins[0].setPosition(new Vector2(325.0f, 280.0f));
			coins[1].setPosition(new Vector2(375.0f, 280.0f));
			coins[2].setPosition(new Vector2(425.0f, 280.0f));
			coins[3].setPosition(new Vector2(600.0f, 400.0f));
			coins[4].setPosition(new Vector2(600.0f, 400.0f));
			coins[5].setPosition(new Vector2(650.0f, 400.0f));
			coins[6].setPosition(new Vector2(700.0f, 400.0f));
			coins[7].setPosition(new Vector2(650.0f, 710.0f));
			coins[8].setPosition(new Vector2(600.0f, 760.0f));
			coins[9].setPosition(new Vector2(550.0f, 810.0f));
			coins[10].setPosition(new Vector2(325.0f, 710.0f));
			coins[11].setPosition(new Vector2(275.0f, 710.0f));
			coins[12].setPosition(new Vector2(225.0f, 710.0f));
			coins[13].setPosition(new Vector2(140.0f, 1100.0f));
			coins[14].setPosition(new Vector2(140.0f, 1150.0f));
			coins[15].setPosition(new Vector2(140.0f, 1200.0f));
			coins[16].setPosition(new Vector2(700.0f, 1150.0f));
			coins[17].setPosition(new Vector2(650.0f, 1150.0f));
			coins[18].setPosition(new Vector2(600.0f, 1150.0f));
			coins[19].setPosition(new Vector2(550.0f, 1150.0f));
			
			//Initislise and position gems
			gems = new Gem[gemCount];
			
			for(int i = 0; i <= gemCount - 1; i++)
			{
				gems[i] = new Gem(scene);
			}
			
			gems[0].setPosition(new Vector2(850.0f, 550.0f));
			gems[1].setPosition(new Vector2(220.0f, 900.0f));
			gems[2].setPosition(new Vector2(325.0f, 1250.0f));
		}
		
		public void LoadLevel(Scene scene, int levelNum)
		{
			scene.RemoveAllChildren(true);			
			
			if(levelNum == 1)
			{
				int[,] mazeLevel1 = { {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5},
						     	  	  {5, 1, 0, 3, 1, 0, 0, 0, 0, 3, 5},
							      	  {5, 2, 0, 4, 2, 0, 3, 1, 0, 4, 5},
							  	      {5, 1, 0, 3, 1, 0, 4, 2, 0, 3, 5},
							 	  	  {5, 2, 0, 4, 2, 0, 3, 1, 0, 4, 5},
							 	 	  {5, 1, 0, 3, 1, 0, 4, 2, 0, 3, 5},
							  	  	  {5, 2, 0, 0, 0, 0, 3, 1, 0, 4, 5},
							  	  	  {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5} };
				
				mazeLevel = mazeLevel1;
				mazeWidth = 8;
				mazeHeight = 11;
				sprites	= new SpriteUV[mazeWidth, mazeHeight];
			}
			else if(levelNum == 2)
			{
				int[,] mazeLevel2 = { {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5},
					                  {5, 1, 0, 3, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 5},
									  {5, 2, 0, 4, 2, 0, 5, 5, 0, 1, 0, 4, 0, 0, 0, 0, 0, 5},
									  {5, 1, 0, 3, 1, 0, 3, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 5},
									  {5, 2, 0, 4, 2, 0, 4, 2, 0, 0, 0, 4, 0, 0, 0, 0, 0, 5},
									  {5, 1, 0, 3, 1, 0, 5, 5, 5, 5, 0, 3, 0, 0, 0, 0, 0, 5},
									  {5, 2, 0, 4, 2, 0, 0, 0, 4, 2, 0, 4, 0, 0, 0, 0, 0, 5},
									  {5, 1, 0, 5, 5, 5, 5, 0, 3, 1, 0, 3, 0, 0, 0, 0, 0, 5},
									  {5, 2, 0, 0, 0, 3, 1, 0, 4, 2, 0, 4, 0, 0, 0, 0, 0, 5},
									  {5, 5, 5, 5, 0, 4, 2, 0, 3, 1, 0, 3, 0, 0, 0, 0, 0, 5},
									  {5, 0, 0, 1, 0, 3, 1, 0, 4, 2, 0, 5, 5, 5, 5, 5, 5, 5},
									  {5, 0, 0, 2, 0, 5, 5, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 5},
									  {5, 0, 0, 1, 0, 0, 0, 0, 4, 2, 5, 5, 5, 5, 5, 5, 5, 5},
									  {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5} };
				
				mazeLevel = mazeLevel2;
				mazeWidth = 14;
				mazeHeight = 18;
				sprites	= new SpriteUV[mazeWidth, mazeHeight];
			}
			
			for (int i = 0; i < mazeWidth; ++i) //Basic tile engine
			{
    			for (int j = 0; j < mazeHeight; ++j) //Goes through every number/coordinate in the array
				{
					if (mazeLevel[i,j] == 1) //If the tile has an ID of 1 it's a rigid block
					{
						sprites[i,j] 		  = new SpriteUV(groundBlock1);
						sprites[i,j].Quad.S   = groundBlock1.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel[i,j] == 2) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(groundBlock2);
						sprites[i,j].Quad.S   = groundBlock2.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel[i,j] == 3) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(ceilingBlock1);
						sprites[i,j].Quad.S   = ceilingBlock1.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel[i,j] == 4) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(ceilingBlock2);
						sprites[i,j].Quad.S   = ceilingBlock2.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel[i,j] == 5) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(treeBlock);
						sprites[i,j].Quad.S   = treeBlock.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else
					{
						sprites[i,j] = new SpriteUV(null);
					}
				}
			}
		}
		
		public void Dispose() //Dispose texture data
		{
			groundBlock1.Dispose();
			groundBlock2.Dispose();
			ceilingBlock1.Dispose();
			ceilingBlock2.Dispose();
			treeBlock.Dispose();
		}
		
		public bool HasCollidedWithPlayer(SpriteUV sprite) //Check if the a sprite has hit a part of the maze
		{
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player ); //Get sprite bounds (player bounds)
			
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = spri.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				
				if (player.Overlaps(mazeTile)) //Return true if the player overlaps with the maze
				{
				   return true;
				}
			}
			return false;
		}
		
		public bool HasHitSide(SpriteUV sprite) //Checks if the player has hit the side of the maze and not the floor
		{
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player ); //Get sprite bounds (player bounds)
			
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = spri.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				if (player.Overlaps(mazeTile)) //Return true if the player overlaps with the maze
				{
					if (checkSides(sprite, spri)) //Return true if the player has come into contact with a side
					{
					   return true;
					}
				}
			}
			return false;
		}
		
		public bool checkSides(SpriteUV sprite, SpriteUV sprite2) //Compares 2 sprites to see is the left or right side has intersected
		{
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player ); //Get sprite bounds 
			
			Bounds2 mazeTile = sprite2.GetlContentLocalBounds();
			sprite2.GetContentWorldBounds(ref mazeTile); 
				
			if (((player.Point01.X < mazeTile.Point11.X) || (player.Point11.X > mazeTile.Point01.X)))
			{ //If the left side of the player is past the right side of the tile and vica versa
				if ((player.Point01.Y) < mazeTile.Point01.Y) //If the tile is above the player
					return true;
				else
					return false;	
			}
			else
				return false;
		}
	}
}