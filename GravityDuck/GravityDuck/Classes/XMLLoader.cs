using System;
using System.Xml;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	public static class XMLLoader
	{
		private static string tileName;
		private static Vector2 position;
		private static float rotation;
		
		public static void LoadLevel(LevelLoader level, string filePath)
		{
			// Create an XML reader for this file.
			using (XmlReader reader = XmlReader.Create(filePath))
			{
    			while (reader.Read())
   				{
					// Only detect start elements.
					if (reader.IsStartElement())
					{
	    				// Get element name and switch on it.
	   					switch (reader.Name)
	    				{
							case "map":
								string widthStr = reader["width"];
								string heightStr = reader["height"];
							
								int width = Convert.ToInt32(widthStr);
								int height = Convert.ToInt32(heightStr);
							
								level.SetWidth(width);
								level.SetHeight(height);
							break;
							
							case "tileset":
								tileName = reader["name"];
							break;
							
							case "image":
								string imageSource = reader["source"];
							
								imageSource = "/Application/" + imageSource;
							
								TextureInfo textureInfo = new TextureInfo(imageSource);
							
								SpriteUV sprite = new SpriteUV(textureInfo);
								sprite.Quad.S = textureInfo.TextureSizef;
								TileManager.AddTileType(sprite, tileName);
							break;
							
							case "tile":
								string tileStr = reader["gid"];
								int tile = Convert.ToInt32(tileStr);
								level.AddTile(tile);
							break;
							
							case "object":
								string xStr = reader["x"];
								string yStr = reader["y"];
							
								float x = (float)Convert.ToDouble(xStr);
								float yTemp = (float)Convert.ToDouble(yStr);
							
								float levelHeight = level.GetLevelHeight();
								float y = levelHeight - yTemp - 30.0f;
							
								position = new Vector2(x, y);
							break;
							
							case "property":
								if(reader["name"] == "Rotation")
								{
									string rotationStr = reader["value"];
									rotation = Convert.ToInt32(rotationStr);
								}
								else if(reader["name"] == "Type")
								{
									string typeName = reader["value"];
									level.AddObject(typeName, position.X, position.Y, rotation);
								}
							break;
						}
	    			}
				}
			}
		}
	}
}