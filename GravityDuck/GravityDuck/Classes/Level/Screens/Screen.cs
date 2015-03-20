using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public class Screen
	{
		protected TextureInfo textureInfo;
		protected SpriteUV sprite;
		
		public Screen (Scene scene)
		{
			
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();		
		}
	}
}

