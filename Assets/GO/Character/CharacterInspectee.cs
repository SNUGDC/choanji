using Gem;
using UnityEngine;

namespace Choanji
{

	public class CharacterInspectee : MonoBehaviour
	{
		public CharacterCtrl ch;

		public IInspectee inspectee
		{
			set
			{
				if (value == null)
				{
					mInspectee = null;
					mOnStart.Dis();
					return;
				}

				if ((mInspectee != null)
				    && (mInspectee.inspectee == value))
				{
					return;
				}


				if (ch.curMap != null)
				{
					mInspectee = new TileInspectee(value) {states = ch.curMap.states};
					mOnStart.Conn(value.onStart);
					mOnDone.Conn(value.onDone);
				}
			}
		}

		private TileInspectee mInspectee;
		private Connection<Coor, TileState> mOnTileRetain;
		private Connection<InspectRequest> mOnStart;
		private Connection<InspectResponse, IInspectee> mOnDone;

		void Awake()
		{
			mOnTileRetain = new Connection<Coor, TileState>((_pos, _state) =>
			{
				if (mInspectee != null) mInspectee.Retain(_pos);
			});

			mOnTileRetain.Conn(ch.doTileRetain);

			mOnStart = new Connection<InspectRequest>(OnInspectStart);
			mOnDone = new Connection<InspectResponse, IInspectee>(OnInspectDone);
		}

		void OnInspectStart(InspectRequest _req)
		{
			var _chReq = _req as CharacterInspectRequest;
			if (_chReq == null)
			{
				L.W(L.M.CALL_INVALID);
				return;
			}

			var _reqPos = _chReq.ch.position;
			ch.TryTurn((Direction)(_reqPos - ch.ch.position));

			ch.blockMove.Add(CharacterMoveBlock.INSPECTEE);
		}

		void OnInspectDone(InspectResponse _res, IInspectee _inspectee)
		{
			ch.blockMove.Remove(CharacterMoveBlock.INSPECTEE);
		}
	}

}