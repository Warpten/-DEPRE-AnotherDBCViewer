using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class LightEntry
    {
        public uint Id;
        public uint MapId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 3)]
        public float[] Position;
        public float FallOffStart;
        public float FallOffEnd;
        public uint SkyAndFogParams; // All params are related to LightParams.dbc
        public uint WaterParams;
        public uint SunsetParams;
        public uint OtherParams;
        public uint DeathParams;
        public uint Unk0;
        public uint Unk1;
        public uint Unk2;
    }
}
