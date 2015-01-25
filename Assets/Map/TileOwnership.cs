using Gem;

namespace Choanji
{
	public abstract class TileOwnership
	{
		public LocalCoor? position { get; private set; }

		~TileOwnership()
		{
			Release();
		}

		public bool isRetained { get { return position.HasValue; } }

		public void Retain(LocalCoor p)
		{
			if (position == p)
				return;

			Release();

			if (DoRetain(p))
				position = p;
			else
				D.Assert(false);
		}


		public void Release()
		{
			if (isRetained)
			{
				DoRelease();
				position = null;
			}
		}

		protected abstract bool DoRetain(LocalCoor p);
		protected abstract void DoRelease();
	}

}