using Gem;

namespace Choanji
{
	public class InspectRequest
	{
		public Connection<InspectResponse, IInspectee> onDone;
	}

	public class InspectResponse
	{
		public InspectResponse(bool _sucess)
		{
			success = _sucess;
		}

		public readonly bool success;
		public bool cancel { get { return !success; }}
	}
}
