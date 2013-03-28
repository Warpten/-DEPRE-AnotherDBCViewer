using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class TaxiNodesEntry
    {
        public uint Id;
        public uint MapId;
        public float X;
        public float Y;
        public float Z;
        public string Name;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] MountNpcId;
        public uint Unk0;
        public float Unk1;
        public float Unk2;
    }
}
