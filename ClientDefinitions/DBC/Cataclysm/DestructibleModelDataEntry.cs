using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class DestructibleModelDataEntry
    {
        public int Id;
        public int DamagedDisplayId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 3)]
        public int[] DamagedUnk;
        public int DestroyedDisplayId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public int[] DestroyedUnk;
        public int RebuildingDisplayId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public int[] RebuildingUnk;
        public int SmokeDisplayId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public int[] SmokeUnk;
        public int Unk0;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 3)]
        public int[] Unk1;
    }
}
