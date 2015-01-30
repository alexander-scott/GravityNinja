using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using Sce.PlayStation.HighLevel.Physics2D;

using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

namespace GravDuck
{
	public abstract class PhysicsEntity : PhysicsBody
	{
		// All entities exhibit this properties
		protected SpriteUV 	sprite;
		
		public PhysicsEntity ()
		{			
		}		
		
		public virtual void Update(float dt, Vector2 gravityVector)
		{
		}
		
		public virtual void Move(float x, float y)
		{
		}
		
		public virtual void Rotate(float x, float y)
		{				
		}
		
		public virtual void Animate()
		{
		}
		
		public virtual void PlaySound(SoundPlayer sound, float volume)
		{
			sound.Play();
			sound.Volume = volume;
		}
		
		//public virtual bool Collision(SpriteUV sprite1, SpriteUV sprite2) // Collision detection
		//{			
		//	// NEEDS IMPLEMENTING	RMDS
		//}
		
		public virtual void SortCollision(PhysicsEntity entity){}
		
		public virtual SpriteUV GetSprite(){ return sprite; }
	}
}

