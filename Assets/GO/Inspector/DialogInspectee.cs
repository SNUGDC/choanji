using Gem;

namespace Choanji
{

	public sealed class DialogInspectee : IInspectee
	{
		protected override void DoStart(InspectRequest _request)
		{
			L.W("start!");
			TheDialog.g.Play(new DialogDef());
			Done(new InspectResponse(true));
		}


	}


}