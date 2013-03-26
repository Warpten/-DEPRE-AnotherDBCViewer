using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class SpellClassOptionsEntry
    {
        public uint Id;
        public uint ModalNextSpell;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 3)]
        public uint[] SpellFamilyFlags;
        public uint SpellFamilyName;
        public string Description;
    }
}
