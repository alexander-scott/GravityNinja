using System;

namespace GravityDuck
{
	public class Highscore
	{
		private int level;
		private int score;
		private string playerName;
		
		public Highscore (int level, int score, string playerName)
		{
			this.level = level;
			this.score = score;
			this.playerName = playerName;
		} 
		
		public int GetLevel() { return level; }
		public int GetScore() { return score; }
		public string GetPlayerName() { return playerName; }
		
		public void SetScore(int score) { this.score = score; }
		public void SetPlayerName(string playerName) { this.playerName = playerName; }
	}
}

