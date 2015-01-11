using System.Xml.Serialization;
using Gem;

namespace Choanji
{

	public class TileOccupy
	{
		private static readonly Point INVALID = new Point(-1, -1);
		private readonly Grid<TileState> mStates;
		private Point mPosition = INVALID;

		public TileOccupy(Grid<TileState> _states)
		{
			mStates = _states;
		}

		public TileOccupy(Grid<TileState> _states, Point _p)
			: this(_states)
		{
			Occupy(_p);
		}

		~TileOccupy()
		{
			Unoccupy();
		}

		public bool isOccupied { get { return mPosition.x >= 0; } }

		public void Occupy(Point _p)
		{
			if (mPosition == _p)
				return;

			Unoccupy();

			if (_p.x < 0)
			{
				L.E(L.M.SHOULD_POS("position x", _p.x));
				return;
			}

			mPosition = _p;
			mStates[mPosition].Occupy();
		}

		public void Unoccupy()
		{
			if (isOccupied)
			{
				mStates[mPosition].Unoccupy();
				mPosition = INVALID;
			}
		}
	}

}