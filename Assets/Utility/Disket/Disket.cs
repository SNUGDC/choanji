using System;
using System.IO;
using Gem;
using LitJson;
using UnityEngine;

namespace Choanji
{
	public static class Disket
	{
		public static bool isLoaded { get { return data != null; } }
		public static string filename { get; private set; }
		public static SaveData data { get; private set; }

		private static FullPath FullPath(string _filename)
		{
			return new FullPath("Resources/Save/" + _filename + ".json");
		}

		public static bool Load(string _filename)
		{
			if (isLoaded)
			{
				L.E("trying to load again.");
				return true;
			}

			SaveData _data;
			if (!JsonHelper.ObjectWithRaw(FullPath(_filename), out _data))
				return false;

			filename = _filename;
			data = _data;

			return true;
		}

		public static void LoadOrDefault(string _filename)
		{
			if (Load(_filename))
				return;

			if (!Load("default"))
			{
				L.E("load default failed.");
				return;
			}

			filename = _filename;
		}

		public static bool Save()
		{
			if (!isLoaded)
			{
				L.E("save before load.");
				return false;
			}

			data = new SaveData(
				new SaveData.User { gold = Wallet.gold },
				new SaveData.Character { skin = TheCharacter.g.renderer_.GetSkinKey() },
				new SaveData.World { key = TheWorld.bluePrint.name, pos = TheCharacter.g.position });

			try
			{
				using (var _file = File.CreateText(FullPath(filename)))
				{
					var _writer = new JsonWriter(_file);
					JsonMapper.ToJson(data, _writer);
				}
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return false;
			}

			return true;
		}
	}
}