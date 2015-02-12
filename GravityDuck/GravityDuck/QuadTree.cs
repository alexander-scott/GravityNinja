using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class QuadTree
	{
		//QuadTree[] child;
		private Rectangle[] quadSection;
		
		//FOR DEBUGGING	RMDS
		public SpriteUV[] visibleZone;
		
		private TextureInfo quadTexture;
		
		public QuadTree (Scene scene, int numberOfQuadTreesToTraverse, SpriteUV background)
		{			
			// Initialise 4 squares
			quadSection = new Rectangle[4];		
			
			// Set their size to half the background size
			for(int i = 0; i < 4; i++)
			{
				quadSection[i] = new Rectangle(new Vector2(0.0f, 0.0f), new Vector2(background.TextureInfo.TextureSizef.X/2,
				                                                          background.TextureInfo.TextureSizef.Y/2)) ;
			}
			
			//Background SCALE IS * 3
			
			// Set their position to each corner of the background		
			quadSection[0].Position = background.Position;
			quadSection[1].Position = new Vector2(background.Position.X + background.TextureInfo.TextureSizef.X/2,
			                                  	background.Position.Y);
			quadSection[2].Position = new Vector2(background.Position.X + background.TextureInfo.TextureSizef.X/2,
			                                  background.Position.Y + background.TextureInfo.TextureSizef.Y/2 );
			quadSection[3].Position = new Vector2(background.Position.X,
			                                  background.Position.Y + background.TextureInfo.TextureSizef.Y/2 );
			
			
			// FOR DEBUGGING
			visibleZone = new SpriteUV[4];
			quadTexture = new TextureInfo("/Application/textures/background.png");
			
			for(int i = 0; i < 4; i++)
			{
				visibleZone[i] = new SpriteUV();
				visibleZone[i] = new SpriteUV(quadTexture);
				visibleZone[i].Quad.S = new Vector2(quadSection[i].Width, quadSection[i].Height);
				visibleZone[i].Color = Colors.Red;		
				visibleZone[i].Position = quadSection[i].Position;
			}
			
			for(int i = 0; i < 4; i++)
				scene.AddChild(visibleZone[i]);
			
			//QuadTree[] child = new QuadTree[4];	
			//
			//foreach(QuadTree node in child)
			//{
			//	// This shall be the number of quads the player/objects will be checked through.
			//	if(numberOfQuadTreesToTraverse > 1)
			//	{
			//		numberOfQuadTreesToTraverse--;
			//		
			//		SpriteUV smallerBackground = background;
			//		
			//		for(int i = 0; i < 4; i++)
			//		{
			//			smallerBackground.Scale = new Vector2(0.5f, 0.5f);
			//			smallerBackground.Position = quadSection[i].Position;
			//			child[i] = new QuadTree(scene,
			//			                     numberOfQuadTreesToTraverse, 
			//			                     	smallerBackground);
			//		}
			//
			//		
			//		// May need to multiply the background.TextureInfo value by the background's scale value,
			//		// further testing is required.	RMDS
			//	}
			//	else
			//	{
			//		child = null; // So the quad tree doesn't have to check against a smaller quad tree.
			//	}			
			//}										
		}
		
		public void Update(SpriteUV background)
		{		
			quadSection[0].Position = background.Position;
			quadSection[1].Position = new Vector2(background.Position.X + background.TextureInfo.TextureSizef.X/2,
			                                  	background.Position.Y);
			quadSection[2].Position = new Vector2(background.Position.X + background.TextureInfo.TextureSizef.X/2,
			                                  background.Position.Y + background.TextureInfo.TextureSizef.Y/2 );
			quadSection[3].Position = new Vector2(background.Position.X,
			                                  background.Position.Y + background.TextureInfo.TextureSizef.Y/2 );
			
			// FOR DEBUGGING					
			for(int i = 0; i < 4; i++)			
				visibleZone[i].Position = quadSection[i].Position;						
		}	
				   
		//bool CheckQuadTree(List<SpriteUV> objectSprites, SpriteUV playerSprite)// This will check the player is within a quad that contains objects
		//{
		//	for(int i = 0; i < 4; i++)
		//		if(CheckQuad(quadSection[i], objectSprites, playerSprite))
		//		{
		//			return true;
		//		}
		//}
		//
		//// If there are objects that will collide off one another instead of the player,
		//// a change to this implementation is required.	RMDS
		//bool CheckQuad(Rectangle quad, List<SpriteUV> objectSprites, SpriteUV playerSprite)
		//{				
		//	
		//	
		//		if(CheckPlayerPosition(quad, playerSprite))
		//		{
		//			foreach(SpriteUV obj in objectSprites)			
		//			{
		//				if(CheckPlayerPosition(quad, obj))											
		//				{
		//					return child.CheckQuadTree(objectSprites);
		//				}	
		//			}	
		//		}	
		//	
		//	return false;
		//}
		//
		//bool CheckSpritePosition(Rectangle quad, SpriteUV playerSprite)
		//{
		//	if(playerSprite.Position.X < quad.Position.X)
		//		return false;
		//	else if(playerSprite.Position.X > quad.Position.X + quad.Size.X)
		//			return false;
		//		else if(playerSprite.Position.Y < quad.Position.Y)
		//				return false;
		//			else if(playerSprite.Position.Y > quad.Position.Y + quad.Size.Y)
		//					return false;
		//	
		//	return true;
		//}
	}
}

