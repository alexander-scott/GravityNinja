using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

namespace GravityDuck
{
	//Our Duck class V2.0 by @AS
	public class Player
	{
		private static SpriteUV 	sprite; //Our players sprite
		private static TextureInfo	textureInfo; //Our players texture (currently no animation)
		
		private static bool			alive = true; //Changed if player dies
		private static bool 		falling = true; //A different calculation will be required if the player is falling
		private static float 		speed = 0.08f, velocity = 0.05f; //Normal movement attributes
		private static float		gravSpeed = 0.1f, maxGrav = 6.0f, gravVelocity = 0.5f; //Falling attributes
		private static Vector2 		directionVector = new Vector2(1.0f, 0.0f); //This is the direction the player will move. It will change
		public static float 		duckRotation = 0.0f;				       //relative to the angle of the maze
		public Player (Scene scene)
		{	
			textureInfo = new TextureInfo("/Application/textures/duck.png"); //Load in our lovely duck texture
			
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef; //Might need to make smaller or bigger in the future
			sprite.Position = new Vector2(190.0f, 330.0f); //Starting position (will be changed)
			sprite.CenterSprite(new Vector2(0.5f,0.5f)); //Set the origin of the sprite to the centre of the duck
			sprite.Scale    = new Vector2(0.5f, 0.5f);
			sprite.Angle = 0.0f;
			alive = true; //Default alive true	
			
			scene.AddChild(sprite); //Add our FABULOUS duck to the scene
		}
		
		//Player update V2.0 @AS
		public void Update(Vector2 gravity, Vector2 rotate, bool invert) //Pass in the gravity, duck rotation and whether
		{																 //the rebound vector is inverted to the right or up
			duckRotation = -(float)FMath.Atan2(rotate.X, rotate.Y);
			sprite.Angle = duckRotation; //Rotate the duck so it's always facing upright (TODO)
			
			Vector2 tempDir; //Interchangable temp vector
			if (gravVelocity>-1.0f) //If he isn't moving backwards from a collision
				tempDir = new Vector2(gravity.X, gravity.Y); //Normal gravity
			else
			{
				if (invert) //If inverted
					tempDir = new Vector2(gravity.X, -gravity.Y); //Rebound in the Y axis (SIDES)
				else
					tempDir = new Vector2(-gravity.X, gravity.Y); //Else rebound in the X axis (FLOOR)
			}
		
			if(gravVelocity < maxGrav) //Increase the gravity velocity
			{
				if(gravVelocity > -1.0f && gravVelocity < 1.0f)
					gravVelocity = 1f;
				else if (gravVelocity > 4f)
					gravVelocity += gravSpeed/2;
				else
					gravVelocity += gravSpeed;
			}
							
			//Move the duck
			sprite.Position = new Vector2(sprite.Position.X + ((tempDir.X) * gravVelocity), sprite.Position.Y + ((tempDir.Y) * gravVelocity));	
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
		
		public void SetVelocity(float newVel)
		{
			gravVelocity = newVel;
		}
		
		public bool CheckFalling() { return falling; }
				
		public float GetX() { return sprite.Position.X; }
		
		public float GetY()	{ return sprite.Position.Y; }
		
		public Vector2 GetDirection() { return directionVector; }
		
		public Vector2 GetPos()	{ return sprite.Position; }
		
		public float GetVelocity() { return gravVelocity; }
		
		public bool IsAlive(){ return alive; }
		
		public void Dispose() //Dispose texture data
		{
			textureInfo.Dispose();
		}
	}
}

