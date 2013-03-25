using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class CreatureFamilyEntry
    {
        public uint Id;
        public float MinScale;
        public uint MinScaleLevel;
        public float MaxScale;
        public uint MaxScaleLevel;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] SkillLine;
        public uint PetFoodMask;
        public int PetTalentType;
        public uint CategoryEnumId;
        public string Name;
        public uint IconFile;
    }
}
