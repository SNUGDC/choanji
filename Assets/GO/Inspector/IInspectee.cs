using Gem;

namespace Choanji
{
	public abstract class IInspectee
	{
		public bool isRunning { get; private set; }

		private ActionWrap<InspectResponse, IInspectee> mOnDone;
	
		~IInspectee()
		{
			if (isRunning)
				Done(new InspectResponse(false));
		}

		public virtual bool CanStart()
		{
			return !isRunning;
		}

		public void Start(
			InspectRequest _request, 
			Connection<InspectResponse, IInspectee> _conn = null)
		{
#if UNITY_EDITOR
			D.Assert(CanStart());
#endif

			if (_conn != null)
			{
				mOnDone = new ActionWrap<InspectResponse, IInspectee>();
				_conn.Conn(mOnDone);
			}

			DoStart(_request);
		}

		protected abstract void DoStart(InspectRequest _data);

		protected virtual void Done(InspectResponse _response)
		{
			if (mOnDone != null)
			{
				mOnDone.val(_response, this);
				mOnDone = null;
			}
		}
	}
}
