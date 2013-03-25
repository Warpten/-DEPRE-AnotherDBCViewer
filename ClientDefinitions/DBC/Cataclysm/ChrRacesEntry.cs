using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class ChrRacesEntry
    {
        public uint RaceId;
        public uint Flags;
        public uint FactionId;
        public uint ExplorationSoundId;
        public uint ModelMale;
        public uint ModelFemale;
        public string ClientPrefix;
        public uint BaseLanguage;
        public uint CreatureType;
        public uint ResSicknessSpellId;
        public uint SplashSoundId;
        public string ClientFileString;
        public uint CinematicSequence;
        public uint Alliance;
        public string Name;
        public string NameFemale;
        public string NameNeutralGender;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] FacialHairCustomization;
        public uint HairCustomization;
        public uint Expansion;
        public uint UnkCataclysm1;
        public uint UnkCataclysm2;
        public uint UnkCataclysm3;
    }
}
