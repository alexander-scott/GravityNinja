using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravDuck
{
	//Our Maze class V1.0 by @AS
	public class Maze
	{
		private TextureInfo testBlock; //Texture for each block used (currently just 1)
		private SpriteUV[,] 	sprites; //Each block is a sprite
		private int mazeWidth = 15, mazeHeight = 15; //Width and height for the maze
						
		public Maze (GameScene scene)
		{
			testBlock = new TextureInfo("/Application/textures/TestBlock.png"); //Load in the textures here
			sprites	= new SpriteUV[mazeWidth,mazeHeight]; //Initalise the sprites
						
			int[,] tileMap ={ {1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0}, //Basic layout for a map
						      {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //Slight bug:
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //For some reason the layout is rotated
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //90 degrees to the left when rendered
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //so when creating the array this needs
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //to be taken into account.
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //I'll fix it later
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, };
			
			for (int i = 0; i < mazeWidth; ++i) //Basic tile engine
			{
    			for (int j = 0; j < mazeHeight; ++j) //Goes through every number/coordinate in the array
				{
					if (tileMap[i,j] == 1) //If the tile has an ID of 1 it's a rigid block
					{
						sprites[i,j] 			= new SpriteUV(testBlock);
						sprites[i,j].Quad.S 		= testBlock.TextureSizef;
						sprites[i,j].Position = new Vector2(50.0f*i, 50.0f*j); //Place them at intervals of 50
					}														   //as 50x50 is their size
					else //Else place the block off-screen
					{
						sprites[i,j] 			= new SpriteUV(testBlock);
						sprites[i,j].Quad.S 		= testBlock.TextureSizef;
						sprites[i,j].Position = new Vector2(-30.0f, -30.0f);
					}
				}
			}
			
			foreach(SpriteUV sprite in sprites) //Add these blocks to our scene
				scene.AddChild(sprite);
		}
		
		public void Dispose() //Dispose texture data
		{
			testBlock.Dispose();
		}
		
		public bool HasCollidedWithPlayer(SpriteUV sprite)
		{
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player );
			
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = sprite.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile);
				
				if (mazeTile.Overlaps(player))
				{
				   return true;
				
				}
			}
			return false;
				
		}
	}
}

