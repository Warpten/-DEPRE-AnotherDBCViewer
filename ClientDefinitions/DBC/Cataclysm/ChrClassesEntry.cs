using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class ChrClassesEntry
    {
        public uint ClassID;
        public uint PowerType;
        public uint PetNameToken;
        public string Name;
        public string NameFemale;
        public string NameNeutralGender;
        public string CapitalizedName;
        public uint SpellFamily;
        public uint Flags;
        public uint CinematicSequence;
        public uint Expansion;
        public uint APPerStrenth;
        public uint APPerAgility;
        public uint RAPPerAgility;
    }
}
