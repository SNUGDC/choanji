using System.Collections.Generic;
using Choanji;
using UnityEngine;

namespace Tiled2Unity
{
    interface ICustomTiledImporter
    {
        void HandleMapProperties(GameObject gameObject, IDictionary<string, string> customProperties);

        void CustomizeMap(GameObject prefab);

		bool CustomizeGO(GameObject go, IDictionary<string, string> customProperties, TileData _tileData);
	}
}

// Examples
/*
[Tiled2Unity.CustomTiledImporter]
class CustomImporterAddComponent : Tiled2Unity.ICustomTiledImporter
{
    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props)
    {
        // Simply add a component to our GameObject
        if (props.ContainsKey("AddComp"))
        {
            gameObject.AddComponent(props["AddComp"]);
        }
    }


    public void CustomizePrefab(GameObject prefab)
    {
        // Do nothing
    }
}
*/
