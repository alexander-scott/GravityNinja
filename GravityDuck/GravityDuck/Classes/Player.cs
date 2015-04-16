using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.Physics2D;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

namespace GravityDuck
{
	//Our Player class V2.0 by @AS
	public class Player
	{
		private static SpriteUV 	sprite; //Our players sprite
		private static TextureInfo	textureInfo; //Our players texture (currently no animation)
		
		private static bool			alive = true; //Changed if player dies
		private static bool 		falling = true; //A different calculation will be required if the player is falling
		private const float		    maxSpeed = 3.0f;
		private const float			gravityConst = 0.1f;
		private static Vector2      velocity = new Vector2(0.0f, 0.0f);
		private static Vector2      acceleration = new Vector2(0.0f, 0.0f);
		private static float     	duckRotation = 0.0f;
		private static float		gravSpeed = 0.5f, maxGrav = 10.0f, gravVelocity = 2.5f;
		private static float 		momentum = 0.0f;
		private static float 		mass = 10.0f;
		
		public Player (Scene scene, Vector2 spawnPoint)
		{	
			textureInfo = new TextureInfo("/Application/textures/ninja2.png"); //Load in our lovely duck texture
			
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef; //Might need to make smaller or bigger in the future
			sprite.Position = spawnPoint; //Starting position (will be changed)
			sprite.CenterSprite(new Vector2(0.5f,0.5f)); //Set the origin of the sprite to the centre of the duck
			sprite.Scale    = new Vector2(0.5f, 0.5f);
			sprite.Angle = 0.0f;
			alive = true; //Default alive true		

			scene.AddChild(sprite); //Add our FABULOUS duck to the scene
		}
		
		//Update player V2.1 @AS
		public void Update(Vector2 gravity, Vector2 rotate, Vector2 movement, bool invert, bool falling, Vector2 additionalForces)
		{	        
			duckRotation = -(float)FMath.Atan2(rotate.X, rotate.Y);
			sprite.Angle = duckRotation; //Rotate the duck so it's always facing upright
			
			Vector2 tempDir; //Initalise the interchangable temp vector
			if (!falling)
			{
				if(!invert)
				{
					if (additionalForces != new Vector2(0.0f, 0.0f))
						tempDir = movement*5*additionalForces; //Normal movement caused by tilting the device
					else
						tempDir = movement*5;
				}
				else
				{
					if (additionalForces != new Vector2(0.0f, 0.0f))
						tempDir = movement*5*additionalForces; //Normal movement caused by tilting the device
					else
						tempDir = movement*5;
				}
			}
			else
			{
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
					if(gravVelocity > -0.5f && gravVelocity < 0.5f)
						gravVelocity = 1f;
					else if (gravVelocity > 1f)
						gravVelocity += gravSpeed/3;
					else
						gravVelocity += gravSpeed;
				}
			}
							
			Vector2 velocityChange = new Vector2((tempDir.X * gravVelocity) + additionalForces.X, (tempDir.Y * gravVelocity) + additionalForces.Y);
			//Move the player
			sprite.Position = new Vector2(sprite.Position.X + velocityChange.X, sprite.Position.Y + velocityChange.Y);
			
			momentum = 	velocityChange.Length() * mass;
		}                  
		
		public void Bounce(float side)
		{
			if(side == 1 || side == 2)
				velocity.X = velocity.X * -1;
			else if(side == 3 || side == 4)
				velocity.Y = velocity.Y * -1;
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
			//directionVector = direction;
		}
		
		public void InvertDirection() //Inverts the current direction (makes him go the opposite way)
		{
			//directionVector = -directionVector;
		}
		
		public void SetPos(Vector2 newPos) //Allows us to move the duck to a point
		{
			sprite.Position = newPos;
		}
		
		public void SetVelocity(float newVel)
		{
			gravVelocity = newVel;
		}
		
		public void Dead()
		{
			alive = false;
		}
		
		public void resetPosition()
		{
			sprite.Position = new Vector2(175.0f, 184.0f);
		}
		
		public void setAlive()
		{
			alive = true;
		}
		
		public void setVisibility(bool visible)
		{
			if(visible)
				sprite.Visible = true;
			else
				sprite.Visible = false;
		}
		
		public bool CheckFalling() { return falling; }
				
		public float GetX() { return sprite.Position.X; }
		
		public float GetY()	{ return sprite.Position.Y; }
		
		public float GetVelocity() { return gravVelocity; }
		
		public Vector2 GetPos()	{ return sprite.Position; }
		
		public float GetMomentum() { return momentum; }
			
		public bool IsAlive(){ return alive; }
		
		public Bounds2 getBounds() {return sprite.GetlContentLocalBounds(); }
		
		public void Dispose() //Dispose texture data
		{
			textureInfo.Dispose();
		}
	}
}

