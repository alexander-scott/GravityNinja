using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class BlackHole : Obstacle
	{
		private const float radialDistance = 200.0f;
		private const float forceModifier = 3.0f; 
		
		public BlackHole(Scene scene) : base(scene)
		{
			textureInfo = new TextureInfo("/Application/textures/Level/blackHole.png");

			sprite          = new SpriteUV(textureInfo);
			sprite.Quad.S   = textureInfo.TextureSizef;
			
			scene.AddChild(sprite);
		}
		
		public Vector2 CalculateForce(Player player)
		{
			Vector2 vectorTo = player.GetPos() - (sprite.Position + new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2));
			float distance = vectorTo.Length();
			
			// Direction of force will be towards the Black Hole
			vectorTo = vectorTo.Normalize();
			vectorTo = vectorTo.Multiply(-1.0f);
			
			float forcePropToDist = distance / radialDistance;
			
			Vector2 force = new Vector2(vectorTo.X * ((1 - forcePropToDist) * forceModifier), vectorTo.Y * ((1 - forcePropToDist) * forceModifier));
			
			return force;
		}
		
		public bool CheckPlayerPos(Player player)
		{
			Vector2 vectorTo = player.GetPos() - (sprite.Position + new Vector2(sprite.Quad.S.X/2, sprite.Quad.S.Y/2));
			float distance = vectorTo.Length();
			
			if(distance <= radialDistance)
				return true;
			
			return false;
		}
	}
}

