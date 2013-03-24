using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class SpellRadiusEntry
    {
        public uint Id;
        public float RadiusMin;
        public float RadiusPerLevel;
        public float RadiusMax;
    }
}
