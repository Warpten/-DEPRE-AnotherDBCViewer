using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class ScalingStatDistributionEntry
    {
        public uint Id;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 10)]
        public int[] StatModifier;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 10)]
        public int[] Modifier;
        public uint Unk0;
        public uint MaxLevel;
    }
}
