using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class WorldSafeLocsEntry
    {
        public uint Id;
        public uint MapId;
        public float X;
        public float Y;
        public float Z;
        public string Name;
    }
}
