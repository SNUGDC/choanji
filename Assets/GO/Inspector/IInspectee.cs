using Gem;

namespace Choanji
{
	public abstract class IInspectee
	{
		public bool isRunning { get; private set; }

		public ActionWrap<InspectRequest> onStart 
			= new ActionWrap<InspectRequest>();

		public ActionWrap<InspectResponse, IInspectee> onDone
			= new ActionWrap<InspectResponse, IInspectee>();

		private ActionWrap<InspectResponse, IInspectee> mDoneCallback;
	
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
			ActionWrap<InspectResponse, IInspectee> _conn = null)
		{
#if UNITY_EDITOR
			D.Assert(CanStart());
#endif

			mDoneCallback = _conn;

			onStart.val.CheckAndCall(_request);

			DoStart(_request);
		}

		protected abstract void DoStart(InspectRequest _data);

		protected void Done(InspectResponse _response)
		{
			if (mDoneCallback != null)
			{
				mDoneCallback.val(_response, this);
				mDoneCallback = null;
			}

			onDone.val.CheckAndCall(_response, this);
		}

		protected virtual void DoDone(InspectResponse _response) { }
	}
}
