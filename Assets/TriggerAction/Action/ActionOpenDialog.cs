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
			if (_json.IsString)
			{
				dialog = (string)_json;
			}
			else
			{
				dialog = (string)_json["dialog"];
				blockMove = _json.BoolOrDefault("block_move", true);
			}
		}

		public override void Do(object _data)
		{
			var _isBlocked = blockMove;

			if (blockMove)
				TheCharacter.ctrl.blockMove.Add(CharacterMoveBlock.INSPECTEE);

			TheDialog.g.Open(DialogHelper.MakeProvider(dialog), new DialogHandler()
			{
				onDone = delegate
				{
					if (_isBlocked)
						TheCharacter.ctrl.blockMove.Remove(CharacterMoveBlock.INSPECTEE);
					onDone.CheckAndCall();
				}
			});
		}
	}
}