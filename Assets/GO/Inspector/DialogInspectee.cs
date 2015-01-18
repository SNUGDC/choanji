namespace Choanji
{
	public sealed class DialogInspectee : IInspectee
	{
		public DialogProvider dialog;

		protected override void DoStart(InspectRequest _request)
		{
			TheDialog.g.Open(dialog, new DialogHandler
			{
				onDone = delegate {
					Done(new InspectResponse(true));
				}
			});
		}
	}

}