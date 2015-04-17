using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	//Our Background class V1.0 by @AS
	public class Background
	{
		private TextureInfo backgroundTexture; //The background texture
		private SpriteUV sprite; //The background sprite
		
		public Background (Scene scene, Vector2 centrePos)
		{
			backgroundTexture 	= new TextureInfo("/Application/textures/LevelBackgrounds/bground.png");
			
			sprite 			= new SpriteUV();
			sprite 			= new SpriteUV(backgroundTexture);
			sprite.Quad.S 	= backgroundTexture.TextureSizef;
			//sprite.Pivot 	= new Vector2(sprite.Quad.S.X, sprite.Quad.S.Y);
			sprite.CenterSprite(new Vector2(0.5f,0.5f));
			sprite.Position = centrePos;
			sprite.Angle = 0.0f;
			
			scene.AddChild(sprite);
		}
		
		public void Update(Vector2 centrePos, Vector2 rotation)
		{
			sprite.Position = centrePos;
			sprite.Angle = -(float)FMath.Atan2(rotation.X, rotation.Y);
		}
		
		public void UpdateTexture(int level)
		{
			if (level >= 0 && level <= 3)
				backgroundTexture = new TextureInfo("/Application/textures/LevelBackgrounds/bground.png");
			if (level >= 4 && level <= 7)
				backgroundTexture = new TextureInfo("/Application/textures/LevelBackgrounds/bground2.png");
			if (level >= 8 && level <= 11)
				backgroundTexture = new TextureInfo("/Application/textures/LevelBackgrounds/bground3.png");
			if (level >= 12 && level <= 15)
				backgroundTexture = new TextureInfo("/Application/textures/LevelBackgrounds/bground4.png");
			if (level >= 16 && level <= 19)
				backgroundTexture = new TextureInfo("/Application/textures/LevelBackgrounds/bground5.png");
			if (level >= 20 && level <= 23)
				backgroundTexture = new TextureInfo("/Application/textures/LevelBackgrounds/bground6.png");
			if (level >= 24 && level <= 26)
				backgroundTexture = new TextureInfo("/Application/textures/LevelBackgrounds/bground7.png");
//			
//			
//			
//			if (level == 0 || level == 5 || level == 10 || level == 15 || level == 20 || level == 25)
//				backgroundTexture = new TextureInfo("/Application/textures/LevelBackgrounds/bground.png");
//			if (level == 1 || level == 6 || level == 11 || level == 16 ||level ==  21 || level == 26)
//				backgroundTexture = new TextureInfo("/Application/textures/LevelBackgrounds/bground2.png");
//			if (level == 2 || level == 7 ||level ==  12 ||level ==  17 || level == 22)
//				backgroundTexture = new TextureInfo("/Application/textures/LevelBackgrounds/bground3.png");
//			if (level == 3 || level == 8 || level == 13 || level == 18 || level == 23)
//				backgroundTexture = new TextureInfo("/Application/textures/LevelBackgrounds/bground4.png");
//			if (level == 4 || level == 9 || level == 14 || level == 19 || level == 24)
//				backgroundTexture = new TextureInfo("/Application/textures/LevelBackgrounds/bground5.png");
			sprite.TextureInfo = backgroundTexture;
		}
		
		public void Dispose()
		{
			backgroundTexture.Dispose();
		}
		
		public void SetVisible(bool visible)
		{
			if (visible)
				sprite.Visible = true;
			else
				sprite.Visible = false;
		}
	}
}