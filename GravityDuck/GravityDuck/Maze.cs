using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	//Our Maze class V2.0 by @AS
	public class Maze
	{
		private TextureInfo testBlock; //Texture for each block used (currently just 1)
		private SpriteUV[,] 	sprites; //Each block is a sprite
		private int mazeWidth = 15, mazeHeight = 15; //Width and height for the maze
						
		public Maze (Scene scene)
		{
			testBlock = new TextureInfo("/Application/textures/metalBlock.png"); //Load in the textures here
			sprites	= new SpriteUV[mazeWidth,mazeHeight]; //Initalise the sprites
			
			//http://www.mapeditor.org/			
			int[,] tileMap ={ {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0}, //Basic layout for a map
						      {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0}, //Slight bug:
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0}, //For some reason the layout is rotated
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0}, //90 degrees to the left when drawn
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0}, //so when creating the array this needs
							  {1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0}, //to be taken into account.
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, //I'll fix it later
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
							  {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
							  {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0}, };
				
			for (int i = 0; i < mazeWidth; ++i) //Basic tile engine
			{
    			for (int j = 0; j < mazeHeight; ++j) //Goes through every number/coordinate in the array
				{
					if (tileMap[i,j] == 1) //If the tile has an ID of 1 it's a rigid block
					{
						sprites[i,j] 			= new SpriteUV(testBlock);
						sprites[i,j].Quad.S 		= testBlock.TextureSizef;
						sprites[i,j].Position = new Vector2(50.0f*i, 50.0f*j); //Place them at intervals of 50
						sprites[i,j].CenterSprite(new Vector2(0.5f,0.5f));
						scene.AddChild(sprites[i,j]);
					}														   //as 50x50 is their size
					else //Else initalise but do not draw the tiles
					{
						sprites[i,j] 			= new SpriteUV();
						//sprites[i,j].Quad.S 		= testBlock.TextureSizef;
						//sprites[i,j].Position = new Vector2(-500.0f, -500.0f); //Temporary fix
					}
				}
			}
		}
		
		public void Dispose() //Dispose texture data
		{
			testBlock.Dispose();
		}
		
		public Vector2 CheckCollision(SpriteUV sprite, bool reg) //Check if the a sprite has hit a part of the maze
		{
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player ); //Get sprite bounds (player bounds)
			
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = sprite.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				
				if (mazeTile.Overlaps(player)) //Return true if the player overlaps with the maze
				{
				   return sideCollided(sprite.Position, spri, reg);
				   
				}
			}
			return new Vector2(0.0f, 0.0f);
		}
		
		public Vector2 CheckCollision2(SpriteUV sprite, bool reg) //Check if the a sprite has hit a part of the maze
		{
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player ); //Get sprite bounds (player bounds)
			
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = sprite.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				
				if (mazeTile.Overlaps(player)) //Return true if the player overlaps with the maze
				{
				   return sideCollided(sprite.Position, spri, reg);
				   
				}
			}
			return new Vector2(0.0f, 0.0f);
		}
		
		public bool HasCollidedWithPlayer(SpriteUV sprite) //Check if the a sprite has hit a part of the maze
		{
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player ); //Get sprite bounds (player bounds)
			
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = sprite.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				
				if (mazeTile.Overlaps(player)) 
				{
				   return true; //Return the direction that the player needs to be moved
				   
				}
			}
			return false; //If theres no overlap then don't move the player
		}
		
		public Vector2 minimumTranslation(SpriteUV aSprite, SpriteUV bSprite) //Gets the Vector that the player
        {																	  //needs to be moved in
			Bounds2 aBounds = aSprite.GetlContentLocalBounds();
			aSprite.GetContentWorldBounds(ref aBounds ); //Get sprite A bounds
			
			Bounds2 bBounds = bSprite.GetlContentLocalBounds();
			bSprite.GetContentWorldBounds(ref bBounds ); //Get sprite B bounds
			
            Vector2 amin = aBounds.Point01; //Get the min and max boundaries of the sprites
            Vector2 amax = aBounds.Point10;
            Vector2 bmin = bBounds.Point01;
            Vector2 bmax = bBounds.Point10;
 
            Vector2 mtd = new Vector2(); //Intalise the vector
  
            float left = (bmin.X - amax.X); //Calculate the floats between each side of the sprites
            float right = (bmax.X - amin.X);
            float top = (bmin.Y - amax.Y);
            float bottom = (bmax.Y - amin.Y);
 
            //Work out which direction has the smallest distance (aka the sides that connected)
            if (FMath.Abs(right) > left)
			{
                mtd.X = right;
				Console.WriteLine("right");
			}
            else if (FMath.Abs(left) > right)
			{
                mtd.X = left;
				Console.WriteLine("left");
			}
			 
            if (FMath.Abs(bottom) < top)
			{
                mtd.Y = bottom;
				Console.WriteLine("bottom");
			}
            else if (FMath.Abs(top) < bottom)
			{
                mtd.Y = top;
				Console.WriteLine("top");
			}
			 
            //Set the axis that didn't touch to 0
            if (FMath.Abs(mtd.X) < FMath.Abs(mtd.Y))
                mtd.Y = 0;
            else
                mtd.X = 0; 
			
			return mtd;
        }
		
//		public int sideCollided(Vector2 playerPos, SpriteUV sprite)
//		{
//			Vector2 distance;
//			int side = 0;
//			distance = playerPos - sprite.Position;
//				
//				if (distance.X > 0 && distance.X < 50)
//					side = side + 1; //RIGHT SIDE HIT
//				else if (distance.X < 0)
//					side = side + 1; //LEFT SIDE HIT
//				if (distance.Y > 0)
//					side = side + 10; //BOTTOM HIT
//				else if (distance.Y < 0)
//					side = side + 10;//TOP HIT
//			
//			Console.WriteLine(side);
//			return side;
//		}
		
		public Vector2 sideCollided(Vector2 playerPos, SpriteUV sprite, bool regular)
		{
			Vector2 distance;
			if (regular)
				distance = playerPos - sprite.Position;
			else
				distance = playerPos - sprite.Position;
			
			return distance;
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

