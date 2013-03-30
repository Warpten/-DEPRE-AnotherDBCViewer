using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class ObjectEffectEntry
    {
        public uint Id;
        public string Name;
        public uint ObjectEffectGroupId;
        public uint TriggerType;
        public uint EventType;
        public uint EffectRecType;
        public uint EffectRecId; // SoundEntries.dbc
        public uint Attachment;
        public float OffsetX;
        public float OffsetY;
        public float OffsetZ;
        public uint ObjectEffectModifierId;
    }
}
