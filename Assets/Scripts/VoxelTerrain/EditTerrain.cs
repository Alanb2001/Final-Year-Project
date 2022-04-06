using UnityEngine;

namespace VoxelTerrain
{
    public static class EditTerrain
    {
        private static WorldPos GetBlockPos(Vector3 pos)
        {
            var blockPos = new WorldPos(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
            return blockPos;
        }

        private static WorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
        {
            var pos = new Vector3(MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
                MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
                MoveWithinBlock(hit.point.z, hit.normal.z, adjacent));
            return GetBlockPos(pos);
        }

        static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
        {
            if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
            {
                if (adjacent)
                {
                    pos += (norm / 2);
                }
                else
                {
                    pos -= (norm / 2);
                }
            }
            return pos;
        }

        public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
        {
            Chunk chunk = hit.collider.GetComponent<Chunk>();
            if (chunk == null)
            {
                return false;
            }

            WorldPos pos = GetBlockPos(hit, adjacent);
            
            chunk.world.SetBlock(pos.x, pos.y, pos.z, block);

            return true;
        }

        public static bool GetBlock(RaycastHit hit, bool adjacent = false)
        {
            var chunk = hit.collider.GetComponent<Chunk>();
            if (chunk == null)
            {
                return false;
            }

            var pos = GetBlockPos(hit, adjacent);
            
            chunk.world.GetBlock(pos.x, pos.y, pos.z);

            return true;
        }
    }
}
