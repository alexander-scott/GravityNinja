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
		
		private SpriteUV[,] 	sprites; //Each block is a sprite
		private int mazeWidth = 8, mazeHeight = 11; //Width and height for the maze
						
		public Maze (Scene scene)
		{
			//Load in the textures here
			//Ground Block Textures
			groundBlock1 = new TextureInfo("/Application/textures/Level/groundBlock1.png");
			groundBlock2 = new TextureInfo("/Application/textures/Level/groundBlock2.png");
			
			//Ceiling Block Textures
			ceilingBlock1 = new TextureInfo("/Application/textures/Level/ceilingBlock1.png");
			ceilingBlock2 = new TextureInfo("/Application/textures/Level/ceilingBlock2.png");
			
			treeBlock = new TextureInfo("/Application/textures/Level/treeBlock.png");
			
			sprites	= new SpriteUV[mazeWidth,mazeHeight]; //Initalise the sprites
						
			int[,] tileMap ={ {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5}, //Basic layout for a map
						      {5, 1, 0, 3, 1, 0, 0, 0, 0, 3, 5}, //Slight bug:
							  {5, 2, 0, 4, 2, 0, 3, 1, 0, 4, 5}, //For some reason the layout is rotated
							  {5, 1, 0, 3, 1, 0, 4, 2, 0, 3, 5}, //90 degrees to the left when drawn
							  {5, 2, 0, 4, 2, 0, 3, 1, 0, 4, 5}, //so when creating the array this needs
							  {5, 1, 0, 3, 1, 0, 4, 2, 0, 3, 5}, //to be taken into account.
							  {5, 2, 0, 0, 0, 0, 3, 1, 0, 4, 5}, //I'll fix it later
							  {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5} };
				
			for (int i = 0; i < mazeWidth; ++i) //Basic tile engine
			{
    			for (int j = 0; j < mazeHeight; ++j) //Goes through every number/coordinate in the array
				{
					if (tileMap[i,j] == 1) //If the tile has an ID of 1 it's a rigid block
					{
						sprites[i,j] 		  = new SpriteUV(groundBlock1);
						sprites[i,j].Quad.S   = groundBlock1.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(tileMap[i,j] == 2) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(groundBlock2);
						sprites[i,j].Quad.S   = groundBlock2.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(tileMap[i,j] == 3) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(ceilingBlock1);
						sprites[i,j].Quad.S   = ceilingBlock1.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(tileMap[i,j] == 4) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(ceilingBlock2);
						sprites[i,j].Quad.S   = ceilingBlock2.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(tileMap[i,j] == 5) //Else place the block off-screen
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
				Bounds2 mazeTile = sprite.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				
				if (mazeTile.Overlaps(player)) //Return true if the player overlaps with the maze
				{
				   return true;
				}
			}
			return false;
		}
	}
}

