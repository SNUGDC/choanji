using UnityEngine;

namespace Choanji
{
    public class TileNPCSpawner : MonoBehaviour
    {
        public TileNPCData data;

        public void SpawnAndDestroy()
        {
            Destroy(gameObject);
        }
    }
}
