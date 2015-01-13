using Gem;

namespace Choanji
{
	public abstract class TileOwnership
	{
		private static readonly Point INVALID = new Point(-1, -1);
		private Point mPosition = INVALID;

		public Point? position
		{
			get { return (mPosition != INVALID) ? mPosition : default(Point?); }
			set
			{
				if (value.HasValue) Retain(value.Value);
				else Release();
			}
		}

		~TileOwnership()
		{
			Release();
		}

		public bool isRetained { get { return mPosition.x >= 0; } }

		public void Retain(Point _p)
		{
			if (mPosition == _p)
				return;

			Release();

			if (_p.x < 0)
			{
				L.E(L.M.SHOULD_POS("position x", _p.x));
				return;
			}

			if (DoRetain(_p))
				mPosition = _p;
		}


		public void Release()
		{
			if (isRetained)
			{
				DoRelease();
				mPosition = INVALID;
			}
		}

		protected abstract bool DoRetain(Point _p);
		protected abstract void DoRelease();
	}

}