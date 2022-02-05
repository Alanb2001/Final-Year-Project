using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VoxelTerrain
{
    public static class Serialisation
    {
        public static string saveFolderName = "voxelGameSaves";

        public static string SaveLocation(string worldName)
        {
            string saveLocation = saveFolderName + "/" + worldName + "/";

            if (!Directory.Exists(saveLocation))
            {
                Directory.CreateDirectory(saveLocation);
            }

            return saveLocation;
        }

        public static string FileName(WorldPos chunkLocation)
        {
            string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
            return fileName;
        }
        
        public static void SaveChunk(Chunk chunk)
        {
            string saveFile = SaveLocation(chunk.world.worldName);
            saveFile += FileName(chunk.pos);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, chunk._blocks);
            stream.Close();
        }

        public static bool Load(Chunk chunk)
        {
            string saveFile = SaveLocation(chunk.world.worldName);
            saveFile += FileName(chunk.pos);

            if (!File.Exists(saveFile))
            {
                return false;
            }
            
            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveFile, FileMode.Open);

            chunk._blocks = (Block[,,])formatter.Deserialize(stream);
            stream.Close();
            return true;
        }
    }
}
