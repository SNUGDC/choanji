using System;
using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	[Serializable]
	public class SCData
	{
		public SC type;
		public Sprite uiIcon;
	}

	public class SCDB : MonoBehaviour
	{
		public static SCDB g;

		public List<SCData> db;

		public SCData this[SC _type]
		{
			get
			{
				if ((int) _type >= db.Count)
				{
					L.E(L.M.KEY_NOT_EXISTS(_type));
					return null;
				}

				return db[(int) _type];
			}
		}
	}
}