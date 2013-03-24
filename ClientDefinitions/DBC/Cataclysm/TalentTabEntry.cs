using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class TalentTabEntry
    {
        public uint TalentTabId;
        public string Name;
        public uint SpellIcon;
        public uint ClassMask;
        public uint PetTalentMask;
        public uint TabPage;
        public string InternalName;
        public string Description;
        public uint RolesMask;

        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] MasterySpells;
    }
}
