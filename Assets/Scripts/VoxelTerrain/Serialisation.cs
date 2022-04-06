using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VoxelTerrain
{
    public static class Serialisation
    {
        private const string SaveFolderName = "voxelGameSaves";

        private static string SaveLocation(string worldName)
        {
            var saveLocation = SaveFolderName + "/" + worldName + "/";

            if (!Directory.Exists(saveLocation))
            {
                Directory.CreateDirectory(saveLocation);
            }

            return saveLocation;
        }

        private static string FileName(WorldPos chunkLocation)
        {
            var fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
            return fileName;
        }
        
        public static void SaveChunk(Chunk chunk)
        {
            var save = new Save(chunk);
            if (save.blocks.Count == 0)
            {
                return;
            }
            
            var saveFile = SaveLocation(chunk.world.worldName);
            saveFile += FileName(chunk.pos);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, save);
            stream.Close();
        }

        public static bool Load(Chunk chunk)
        {
            var saveFile = SaveLocation(chunk.world.worldName);
            saveFile += FileName(chunk.pos);

            if (!File.Exists(saveFile))
            {
                return false;
            }
            
            IFormatter formatter = new BinaryFormatter();
            var stream = new FileStream(saveFile, FileMode.Open);

            var save = (Save)formatter.Deserialize(stream);
            foreach (var block in save.blocks)
            {
                chunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
            }
            stream.Close();
            return true;
        }
    }
}
