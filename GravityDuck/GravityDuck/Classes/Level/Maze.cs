using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	//Our Maze class V1.1 by @AW
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
		
		
		public bool birdWalking(SpriteUV aSprite) //Gets the Vector that the player
        {				
			Bounds2 aBounds = aSprite.GetlContentLocalBounds();
			aSprite.GetContentWorldBounds(ref aBounds ); 
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 bSprite = spri.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref bSprite); //Get all of the maze bounds
			
	            Vector2 amin = aBounds.Point01; //Get the min and max boundaries of the sprites
	            Vector2 amax = aBounds.Point10;
	            Vector2 bmin = bSprite.Point01;
	            Vector2 bmax = bSprite.Point10;
	
	            float left = (bmin.X - amax.X); //Calculate the floats between each side of the sprites
	            float right = (bmax.X - amin.X);
	            float top = (bmin.Y - amax.Y);
	            float bottom = (bmax.Y - amin.Y);
	 
	            if (FMath.Abs(top) > bottom)
	                return true;		
			}	
			return false;
        }
		
		public Vector2 checkVertical(SpriteUV aSprite, SpriteUV bSprite) //Gets the Vector that the player
        {																	  //needs to be moved in
			Bounds2 aBounds = aSprite.GetlContentLocalBounds();
			aSprite.GetContentWorldBounds(ref aBounds ); //Get sprite A bounds
			
			Bounds2 bBounds = bSprite.GetlContentLocalBounds();
			bSprite.GetContentWorldBounds(ref bBounds ); //Get sprite B bounds
			
            Vector2 amin = new Vector2(aBounds.Point01.X, aBounds.Point01.Y); //Get the min and max boundaries of the sprites
            Vector2 amax = new Vector2(aBounds.Point10.X, aBounds.Point10.Y);
            Vector2 bmin = new Vector2(bBounds.Point01.X, bBounds.Point01.Y);
            Vector2 bmax = new Vector2(bBounds.Point10.X, bBounds.Point10.Y);
 
            Vector2 mtd = new Vector2(); //Intalise the vector
  
            float left = (bmin.X - amax.X); //Calculate the floats between each side of the sprites
            float right = (bmax.X - amin.X);
            float top = (bmin.Y - amax.Y);
            float bottom = (bmax.Y - amin.Y);
 
            //Work out which direction has the smallest distance (aka the sides that connected)
            if (FMath.Abs(left) < right)
                mtd.X = left + 0.00001f;
            else
                mtd.X = right - 0.00001f;
 
            if (FMath.Abs(top) < bottom)
                mtd.Y = top + 0.00001f;
            else
                mtd.Y = bottom - 0.00001f;
 
            //Set the axis that didn't touch to 0
            if (FMath.Abs(mtd.X) < FMath.Abs(mtd.Y))
                mtd.Y = 0;
            else
                mtd.X = 0; 
			return mtd;
        }
		
		public Vector2 checkHorizontal(SpriteUV aSprite, SpriteUV bSprite) //Gets the Vector that the player
        {																	  //needs to be moved in
			Bounds2 aBounds = aSprite.GetlContentLocalBounds();
			aSprite.GetContentWorldBounds(ref aBounds ); //Get sprite A bounds
			
			Bounds2 bBounds = bSprite.GetlContentLocalBounds();
			bSprite.GetContentWorldBounds(ref bBounds ); //Get sprite B bounds
			
            Vector2 amin = new Vector2(aBounds.Point01.X, aBounds.Point01.Y ); //Get the min and max boundaries of the sprites
            Vector2 amax = new Vector2(aBounds.Point10.X, aBounds.Point10.Y );
            Vector2 bmin = new Vector2(bBounds.Point01.X, bBounds.Point01.Y );
            Vector2 bmax = new Vector2(bBounds.Point10.X, bBounds.Point10.Y );
 
            Vector2 mtd = new Vector2(); //Intalise the vector
  
            float left = (bmin.X - amax.X); //Calculate the floats between each side of the sprites
            float right = (bmax.X - amin.X);
            float top = (bmin.Y - amax.Y);
            float bottom = (bmax.Y - amin.Y);
 
            //Work out which direction has the smallest distance (aka the sides that connected)
            if (FMath.Abs(left) < right)
			{
				mtd.X = left + 0.00001f;
			}
            else
			{
				mtd.X = right - 0.00001f;
			}
     
            if (FMath.Abs(top) < bottom)
                mtd.Y = top + 0.00001f;
            else
                mtd.Y = bottom - 0.00001f;
 
            //Set the axis that didn't touch to 0
            if (FMath.Abs(mtd.X) < FMath.Abs(mtd.Y))
                mtd.Y = 0;
            else
                mtd.X = 0; 
			
			return mtd;
        }
		
		public bool CheckCollision(SpriteUV sprite) //Check if the a sprite has hit a part of the maze
		{
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player ); //Get sprite bounds (player bounds)
			
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = sprite.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				
				if (mazeTile.Overlaps(player)) //Return true if the player overlaps with the maze
				{
				   return true;
				   
				}
			}
			return false;
		}
		public Vector2 VerticalCollision(SpriteUV sprite) //Check if the a sprite has hit a part of the maze
		{
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player ); //Get sprite bounds (player bounds)
			
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = sprite.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				
				if (mazeTile.Overlaps(player)) 
				{
				   return checkVertical(spri, sprite); //Return the direction that the player needs to be moved
				   
				}
			}
			return new Vector2(0.0f, 0.0f); //If theres no overlap then don't move the player
		}
		public Vector2 HorizontalCollision(SpriteUV sprite) //Check if the a sprite has hit a part of the maze
		{
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player ); //Get sprite bounds (player bounds)
			
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = sprite.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				
				if (mazeTile.Overlaps(player)) 
				{
				   return checkHorizontal(spri, sprite); //Return the direction that the player needs to be moved
				   
				}
			}
			return new Vector2(0.0f, 0.0f); //If theres no overlap then don't move the player
		}
		
		
		public bool circleCollision(Vector2 centre, float radius)
		{
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 bBounds = spri.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref bBounds );
				
				Vector2 bmin = new Vector2(bBounds.Point01.X, bBounds.Point01.Y );
            	Vector2 bmax = new Vector2(bBounds.Point10.X, bBounds.Point10.Y );
				
				float cx = FMath.Abs(centre.X - bBounds.Center.X - ((bBounds.Point11.X - bBounds.Point01.X)/2));
				float xDist = ((bBounds.Point11.X - bBounds.Point01.X)/2) + radius;
				if (cx > xDist)
					return false;
				
				float cy = FMath.Abs(centre.Y - bBounds.Center.Y - ((bBounds.Point00.Y - bBounds.Point01.Y)/2));
				float yDist = ((bBounds.Point11.Y - bBounds.Point10.Y)/2) + radius;
				if (cy > yDist)
					return false;
				
				if (((cx <= (bBounds.Point11.X - bBounds.Point01.X)/2)) || (cy <= (bBounds.Point00.Y - bBounds.Point01.Y)/2))
				{
					return true;
				}
				
				float xCornerDist = cx - ((bBounds.Point11.X - bBounds.Point01.X)/2);
			    float yCornerDist = cy - ((bBounds.Point00.Y - bBounds.Point01.Y)/2);
			    float xCornerDistSq = xCornerDist * xCornerDist;
			    float yCornerDistSq = yCornerDist * yCornerDist;
			    float maxCornerDistSq = radius * radius;
			    	return xCornerDistSq + yCornerDistSq <= maxCornerDistSq;
			}
			return false;
			
			
		}
	}
}