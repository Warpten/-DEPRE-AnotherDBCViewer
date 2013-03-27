using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class ItemSetEntry
    {
        public uint SetId;
        public string Name;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 17)] // Code says 10, comment says 17
        public uint[] ItemId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 8)]
        public uint[] SpellId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 8)]
        public uint[] SetSpellsThreshold;
        public uint ReqSkillId;
        public uint ReqSkillValue;
    }
}
