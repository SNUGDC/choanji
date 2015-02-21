using Gem;
using UnityEngine;

namespace Choanji
{
    public class TileNPCSpawner : MonoBehaviour
    {
        public TileNPCData data;

	    void Start()
	    {
		    SpawnAndDestroy();
	    }

        private void SpawnAndDestroy()
        {
	        var _pos = TileHelper.GetLocalCoor(transform);

	        var _ch = TileDB.g.ch.Instantiate();
			_ch.transform.SetParent(transform.parent.parent);

	        var _npcData = NPCDB.Get(data.id);
			if (_npcData != null) 
				_ch.renderer_.SetSkin(_npcData.skin);

	        var _ctrl = _ch.GetComponent<CharacterCtrl>();

	        if (data.dir.HasValue)
		        _ctrl.TryTurn(data.dir.Value);

			if (data.agent != CharacterAgentType.NONE)
		        CharacterAgentFactory.Add(data.agent, _ctrl);

	        if (data.dialog != null)
	        {
				var _inspectee = _ch.AddComponent<CharacterInspectee>();
				_inspectee.inspectee = new DialogInspectee { dialog = DialogHelper.MakeProvider(data.dialog) };   
	        }

			if (!_ctrl.TrySetPosition(_pos))
				D.Assert(false);

			Destroy(gameObject);
        }
    }
}
