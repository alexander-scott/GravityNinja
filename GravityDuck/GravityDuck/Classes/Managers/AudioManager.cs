using System;
using System.Collections.Generic;

using Sce.PlayStation.Core.Audio;

namespace GravityDuck
{
	public static class AudioManager
	{
		//Stores all the sounds loaded
		private static Dictionary<string, Sound> soundList = new Dictionary<string, Sound>();
		
		//Stores all the music loaded
		private static Dictionary<string, Bgm> musicList = new Dictionary<string, Bgm>();
		
		private static BgmPlayer musicPlayer = null;
		private static List<SoundPlayer> soundPlayers = new List<SoundPlayer>();
		
		//Add music to musicList
		public static void AddMusic(string filename, string key)
		{
			if (!musicList.ContainsKey(key))
			{
				Bgm music = new Bgm(filename);
				musicList.Add(key, music);
			}
		}
		
		//Add sound to soundList
		public static void AddSound(string filename, string key)
		{
			if (!soundList.ContainsKey(key))
			{
				Sound sound = new Sound(filename);
				soundList.Add(key, sound);
			}
		}
		
		//Remove music from musicList
		public static void RemoveMusic(string key)
		{
			musicList[key].Dispose();
			musicList.Remove(key);	
		}
		
		//Remove sound from soundList
		public static void RemoveSound(string key)
		{
			soundList[key].Dispose();
			soundList.Remove(key);	
		}
		
		//Play selected music from musicList
		public static bool PlayMusic(string key, bool isLooping, float volume, float playbackRate)
		{
			if (musicList.ContainsKey(key))
			{
				musicPlayer = musicList[key].CreatePlayer(); 
				musicPlayer.Volume = volume;
				musicPlayer.Loop = isLooping;
				musicPlayer.PlaybackRate = playbackRate;
				musicPlayer.Play();
				return true;
			}
			else 
			{
				return false;	
			}
		}
		
		//Play selected sound from soundList
		public static bool PlaySound(string key, bool isLooping, float volume, float playbackRate)
		{
			if (soundList.ContainsKey(key))
			{
				SoundPlayer soundPlayer = soundList[key].CreatePlayer();
				soundPlayer.Volume = volume;
				soundPlayer.Loop = isLooping;
				soundPlayer.PlaybackRate = playbackRate;
				try
				{
					soundPlayer.Play();
				}
				catch
				{
					Console.WriteLine("CAN'T PLAY THE SOUND: " +key);
				}
				foreach(SoundPlayer sounds in soundPlayers)
					sounds.Dispose();
				soundPlayers.Clear();
				soundPlayers.Add(soundPlayer);
				return true;
			}
			else
			{
				return false;	
			}
		}
		
		//Stops any music currently playing
		public static void StopMusic()
		{
			if (musicPlayer != null)
			{
				musicPlayer.Stop();
				musicPlayer.Dispose();
				musicPlayer = null;
			}
		}
		
		//Stops any sounds currently playing
		public static void StopSounds()
		{
			for (int i = 0; i < soundPlayers.Count; i++)
			{
				soundPlayers[i].Stop();
				soundPlayers[i].Dispose();
				soundPlayers.Remove(soundPlayers[i]);
			}
		}
		
		//Pause the music
		public static void PauseMusic()
		{
			if (musicPlayer != null && musicPlayer.Status != BgmStatus.Paused)
			{
				musicPlayer.Pause();	
			}
		}
		
		//Resume the music
		public static void ResumeMusic()
		{
			if (musicPlayer != null && musicPlayer.Status != BgmStatus.Playing)
			{
				musicPlayer.Resume();	
			}
		}
	}
}