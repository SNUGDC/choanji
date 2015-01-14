namespace Choanji
{
	public class InspectRequest
	{}

	public class InspectResponse
	{
		public InspectResponse(bool _success)
		{
			success = _success;
		}

		public readonly bool success;
		public bool cancel { get { return !success; }}
	}
}
