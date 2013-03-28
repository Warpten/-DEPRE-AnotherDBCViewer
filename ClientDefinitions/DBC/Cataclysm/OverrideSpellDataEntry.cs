using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class OverrideSpellDataEntry
    {
        public uint Id;

        [StoragePresence(StoragePresenceOption.Include, ArraySize = 10)]
        public uint[] Spells;

        public uint Flags;
        public string Name;
    }
}
