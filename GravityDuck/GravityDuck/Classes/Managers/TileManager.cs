using System;

using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public static class TileManager
	{
		private static Dictionary<string, SpriteUV> tileList = new Dictionary<string, SpriteUV>();
		
		public static void AddTileType(SpriteUV sprite, string key)
		{
			if (!tileList.ContainsKey(key))
			{
				tileList.Add(key, sprite);	
			}
		}
		
		public static SpriteUV GetTileType(string key)
		{
			if (tileList.ContainsKey(key))
			{
				return tileList[key];
			}
			
			return null;
		}
	}
}