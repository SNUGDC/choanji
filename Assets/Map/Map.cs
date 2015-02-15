using Gem;
using UnityEngine;

namespace Choanji
{
	public class Map
	{
		public Map(MapStatic _static, GameObject _go)
		{
			static_ = _static;
			dynamic = new MapDynamic(static_);
			go = _go;
		}

		public Point size { get { return static_.meta.size; } }

		private Coor? mPosition;
		public Coor position
		{
			get
			{
				if (mPosition == null)
				{
					var _posVector = (Vector2)go.transform.position;
					_posVector.y -= size.y;
					mPosition = new Point(_posVector);
				}
				return mPosition.Value;
			}
		}

		public readonly MapStatic static_;
		public readonly MapDynamic dynamic;
		public readonly GameObject go;

		public LocalCoor Position(Coor _coor)
		{
			return new LocalCoor(this, _coor);
		}

		public LocalCoor Convert(WorldCoor _coor)
		{
			return new LocalCoor(this, _coor - position);
		}

	}
}