using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class TaxiPathNodeEntry
    {
        public uint Id;
        public uint Path;
        public uint Index;
        public uint MapId;
        public float X;
        public float Y;
        public float Z;
        public uint ActionFlag;
        public uint Delay;
        public uint ArrivalEventId;
        public uint DepartureEventId;
    }
}
