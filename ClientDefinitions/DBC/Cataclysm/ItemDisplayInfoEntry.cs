using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class ItemDisplayInfoEntry
    {
        public uint DisplayId;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 6)]
        public string[] IconName;
        public uint Unk0;
        public uint Unk1;
        public uint Unk2; // Some bool
        public uint Unk3; // Flags ? Hex representation
        public uint Unk4;
        public uint Unk5;
        public uint Unk6;
        public uint Unk7;
        [StoragePresence(StoragePresenceOption.Include, ArraySize = 8)]
        public string[] UnkStr; // Model name ?
        public uint Unk9;
        public uint Unk10;
    }
}
