using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class FactionTemplateEntry
    {
        public uint Id;
        public uint Faction;
        public int Flags;
        public int FactionGroupMask;
        public int FriendlyGroupMask;
        public int HostileGroupMask;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public int[] EnemyFaction;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public int[] FriendlyFaction;
    }
}
