using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

namespace GravityDuck
{
	//Our Duck class V1.0 by @AS
	public class Player
	{
		private static SpriteUV 	sprite; //Our players sprite
		private static TextureInfo	textureInfo; //Our players texture (currently no animation)
		
		private static bool			alive = true; //Changed if player dies
		private static bool 		falling = true; //A different calculation will be required if the player is falling
		private static float 		rotationAngle = 0.0f, movementAngle = 0.0f; //Default angles
		private static float 		speed = 0.08f, maxSpeed = 3.0f, velocity = 0.05f; //Normal movement attributes
		private static float		gravSpeed = 0.05f, maxGrav = 5.0f, gravVelocity = 0.3f; //Falling attributes
		private static Vector2 		directionVector = new Vector2(1.0f, 0.0f); //This is the direction the player will move. It will change
																			   //relative to the angle of the maze
		public Player (Scene scene)
		{	
			textureInfo = new TextureInfo("/Application/textures/duck.png"); //Load in our lovely duck texture
			
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef; //Might need to make smaller or bigger in the future
			sprite.Position = new Vector2(120.0f, 600.0f); //Starting position (will be changed)
			sprite.CenterSprite(new Vector2(0.5f,0.5f)); //Set the origin of the sprite to the centre of the duck
			alive = true; //Default alive true	
			
			scene.AddChild(sprite); //Add our FABULOUS duck to the scene
		}
		
		public void Update(Vector2 gravity)
		{			
			if(gravVelocity < 0.3f) //If not falling
			{
				if(velocity < maxSpeed) //Move forward
					velocity += speed;
				sprite.Position = new Vector2(sprite.Position.X + (directionVector.X) * velocity, sprite.Position.Y + (directionVector.Y) * velocity);
			}
			else //Else falling
			{
				if(gravVelocity < maxGrav)
					gravVelocity += gravSpeed;
				sprite.Position = new Vector2(sprite.Position.X + (gravity.X * gravVelocity), sprite.Position.Y + (gravity.Y * gravVelocity));
			}
		}
				
		public void SetFalling(bool fall) //Allows us to set whether the duck is falling or not
		{
			if(!fall) //If he isn't falling
			{
				falling = fall; //Set the bool
				gravVelocity = 0.0f; //Reset the gravity velocity so the duck isn't affected by gravity
			}
			else //If he is falling
			{
				if (gravVelocity < 0.1f) //Set the gravity back to the default value so the duck will fall
					gravVelocity = 0.3f;
				falling = fall; //Set the bool
			}
		}
		
		//Return statements
		public SpriteUV Sprite
		{
			get
			{
				return sprite;
			}
		}
		
		public void SetDirection(Vector2 direction) //Allows us to set the direction the player will move in
		{
			directionVector = direction;
		}
		
		public void InvertDirection() //Inverts the current direction (makes him go the opposite way)
		{
			directionVector = -directionVector;
		}
		
		public void SetPos(Vector2 newPos) //Allows us to move the duck to a point
		{
			sprite.Position = newPos;
		}
		
		public bool CheckFalling() { return falling; }
				
		public float GetX() { return sprite.Position.X; }
		
		public float GetY()	{ return sprite.Position.Y; }
		
		public Vector2 GetDirection() { return directionVector; }
		
		public Vector2 GetPos()	{ return sprite.Position; }
		
		public bool IsAlive(){ return alive; }
		
		public void Dispose() //Dispose texture data
		{
			textureInfo.Dispose();
		}
	}
}

