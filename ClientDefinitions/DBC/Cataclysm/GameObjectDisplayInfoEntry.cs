using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class GameObjectDisplayInfoEntry
    {
        public int DisplayId;
        public string FileName;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 10)]
        public int[] Unk0;
        public float MinX;
        public float MinY;
        public float MinZ;
        public float MaxX;
        public float MaxY;
        public float MaxZ;
        public uint Transport;
        public float Unk1;
        public float Unk2;
    }
}
