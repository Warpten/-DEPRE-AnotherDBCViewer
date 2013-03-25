using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class CreatureDisplayInfoEntry
    {
        public uint Id;
        public uint ModelId;
        public uint SoundId;
        public uint ExtraDisplayInformation;
        public float Scale;
        public uint Opacity;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 3)]
        public uint[] Skin;
        public uint PortraitTextureName;
        public uint SizeClass;
        public uint BloodId;
        public uint NPCSoundId;
        public uint ParticleColorId;
        public uint CreatureGeosetData;
        public uint ObjectEffectPackageId;
        public uint AnimReplacementSetId;		
    }
}
