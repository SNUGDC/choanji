using Gem;
using LitJson;

namespace Choanji
{
	public sealed class ActionOpenDialog : Action_
	{
		public readonly string dialog;
		public bool blockMove = true;

		public ActionOpenDialog(JsonData _json)
			: base(ActionType.OPEN_DIALOG)
		{
			dialog = (string) _json["dialog"];
			blockMove = _json.BoolOrDefault("block_move", true);
		}

		public override void Do(object _data)
		{
			TheCharacter.ctrl.blockMove.Add(CharacterMoveBlock.INSPECTEE);
			TheDialog.g.Open(new DialogProvider(DialogHelper.FullPath(dialog)), new DialogHandler()
			{
				onDone = delegate
				{
					TheCharacter.ctrl.blockMove.Remove(CharacterMoveBlock.INSPECTEE);
					onDone.CheckAndCall();
				}
			});
		}
	}
}