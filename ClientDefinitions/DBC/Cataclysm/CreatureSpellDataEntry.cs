using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class CreatureSpellDataEntry
    {
        public uint Id;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public uint[] SpellId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 4)]
        public uint[] Availability;
    }
}
