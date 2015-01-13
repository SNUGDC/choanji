using Gem;

namespace Choanji
{

	public sealed class DialogInspectee : IInspectee
	{
		protected override bool DoStart(InspectRequest _data)
		{
			L.W("start!");
			TheDialog.g.Play(new DialogDef());
			return true;
		}


	}


}