using Gem;
using UnityEngine;

namespace Choanji
{

	public class CharacterInspectee : MonoBehaviour
	{

		public IInspectee inspectee
		{
			set
			{
				if (value == null)
				{
					mInspectee = null;
					return;
				}

				if ((mInspectee != null)
				    && (mInspectee.inspectee == value))
				{
					return;
				}


				if (mCh.curMap != null)
				{
					mInspectee = new TileInspectee(value) {states = mCh.curMap.states};
				}
			}
		}

		private CharacterCtrl mCh;
		private TileInspectee mInspectee;

		private Connection<Coor, TileState> mOnTileRetain;

		void Awake()
		{
			mCh = GetComponent<CharacterCtrl>();

			mOnTileRetain = new Connection<Coor, TileState>((_pos, _state) =>
			{
				if (mInspectee != null) mInspectee.Retain(_pos);
			});

			mOnTileRetain.Conn(mCh.doTileRetain);

		}
	}

}