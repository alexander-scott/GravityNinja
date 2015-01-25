using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

namespace GravDuck
{
	//Our Duck class V1.0 by @AS
	public class Player
	{
		private static SpriteUV 	sprite; //Our players sprite
		private static TextureInfo	textureInfo; //Our players texture (currently no animation)
		
		private static bool			alive; //Bool to check if player died or not
		private static float 		rotationAngle = 0.0f, movementAngle = 0.0f; //Default angles
		private static float 		speed = 3.0f, maxSpeed = 5.0f, velocity = 0.0f; //Default speed of the player and velocity
		private static bool 		inverted = false; //This becomes true if the slope is too steep for the player
		private static Vector2 		directionVector; //This is the direction the player will move. It will change
													 //relative to the angle of the maze
		public Player (Scene scene)
		{	
			textureInfo  = new TextureInfo("/Application/textures/duck_PNG5021.png"); //Load in our lovely duck texture
			
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef; //Might need to make smaller or bigger in the future
			sprite.Position = new Vector2(100.0f, 101.0f); //Starting position (will be changed)
			sprite.CenterSprite(new Vector2(0.5f,0.5f)); //Set the origin of the sprite to the centre of the duck
			alive = true; //Default alive true			
			
			scene.AddChild(sprite); //Add our FABULOUS duck to the scene
		}
		
		public void Update(Vector2 direction)
		{			
			sprite.Position = new Vector2(sprite.Position.X + speed, sprite.Position.Y);
		}
					
		public SpriteUV Sprite
		{
			get
			{
				return sprite;
			}
		}
		
		public float GetX()
		{
			return sprite.Position.X;
		}
		
		public float GetY()
		{
			return sprite.Position.Y;
		}
		
		public Vector2 GetPos()
		{
			return sprite.Position;
		}
		
		public bool IsAlive(){ return alive; }
	}
}

