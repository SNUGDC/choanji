using Gem;

namespace Choanji
{
	public abstract class IInspectee
	{
		public bool isRunning { get; private set; }

		private readonly ActionWrap<InspectResponse, IInspectee> mOnDone
			= new ActionWrap<InspectResponse, IInspectee>();

		~IInspectee()
		{
			if (isRunning)
				Done(new InspectResponse(false));
		}

		public bool Start(InspectRequest _data)
		{
			if (isRunning)
			{
				L.W(L.M.CALL_RETRY("inspect"));
				return false;
			}

			var _ret = DoStart(_data);

			if (_ret)
			{
				if (_data.onDone != null)
					_data.onDone.Conn(mOnDone);
			}

			return _ret;
		}

		protected abstract bool DoStart(InspectRequest _data);

		protected virtual void Done(InspectResponse _response)
		{
			if (mOnDone.val != null)
			{
				mOnDone.val.CheckAndCall(_response, this);
				mOnDone.val = null;
			}
		}
	}
}
