using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;



namespace GravityDuck
{
	public class QuadTree
	{
		QuadTree child;
		Rectangle[] quadSection;
		
		public QuadTree (int numberOfQuadTreesToTraverse, SpriteUV background)
		{	
			// This shall be the number of quads the player/objects will be checked through.
			if(numberOfQuadTreesToTraverse > 1)
			{
				numberOfQuadTreesToTraverse--;
				child = new QuadTree(numberOfQuadTreesToTraverse, 
				                     Vector2(background.TextureInfo.TextureSizef.X/2,
				        				background.TextureInfo.TextureSizef.Y/2));
				
				// May need to multiply the background.TextureInfo value by the background's scale value,
				// further testing is required.	RMDS
			}
			else
			{
				child = null; // So the quad tree doesn't have to check against a smaller quad tree.
			}
			
			// Initialise 4 squares
			Rectangle[] quadSection = new Rectangle[4];		
			
			// Set their size to half the background size
			foreach(Rectangle quad in quadSection)
				quad = new Rectangle(Vector2D(0.0f, 0.0f), Vector2D(background/2, background/2)) ;
			
			// Set their position to each corner of the background		
			quadSection[0].Position = background.Position;
			quadSection[1].Position = Vector2(background.Position.X + background.TextureInfo.TextureSizef.X/2,
			                                  	background.Position.Y);
			quadSection[2].Position = Vector2(background.Position.X + background.TextureInfo.TextureSizef.X/2,
			                                  background.Position.Y + background.TextureInfo.TextureSizef.Y/2 );
			quadSection[3].Position = Vector2(background.Position.X,
			                                  background.Position.Y + background.TextureInfo.TextureSizef.Y/2 );		
		}
		
		void Update(SpriteUV background)
		{		
			quadSection[0].Position = background.Position;
			quadSection[1].Position = Vector2(background.Position.X + background.TextureInfo.TextureSizef.X/2,
			                                  	background.Position.Y);
			quadSection[2].Position = Vector2(background.Position.X + background.TextureInfo.TextureSizef.X/2,
			                                  background.Position.Y + background.TextureInfo.TextureSizef.Y/2 );
			quadSection[3].Position = Vector2(background.Position.X,
			                                  background.Position.Y + background.TextureInfo.TextureSizef.Y/2 );	
		}	
				   
		bool CheckQuadTree(List<SpriteUV> objectSprites, SpriteUV playerSprite)// This will check the player is within a quad that contains objects
		{
			for(int i = 0; i < 4; i++)
				if(CheckQuad(quadSection[i], objectSprites, playerSprite))
				{
					return true;
				}
		}
		
		// If there are objects that will collide off one another instead of the player,
		// a change to this implementation is required.	RMDS
		bool CheckQuad(Rectangle quad, List<SpriteUV> objectSprites, SpriteUV playerSprite)
		{				
			
			
				if(CheckPlayerPosition(quad, playerSprite))
				{
					foreach(SpriteUV obj in objectSprites)			
					{
						if(CheckPlayerPosition(quad, obj))											
						{
							return child.CheckQuadTree(objectSprites);
						}	
					}	
				}	
			
			return false;
		}
		
		bool CheckSpritePosition(Rectangle quad, SpriteUV playerSprite)
		{
			if(playerSprite.Position.X < quad.Position.X)
				return false;
			else if(playerSprite.Position.X > quad.Position.X + quad.Size.X)
					return false;
				else if(playerSprite.Position.Y < quad.Position.Y)
						return false;
					else if(playerSprite.Position.Y > quad.Position.Y + quad.Size.Y)
							return false;
			
			return true;
		}
	}
}
*/
