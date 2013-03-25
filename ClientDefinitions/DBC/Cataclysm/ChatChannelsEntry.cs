using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class ChatChannelsEntry
    {
        public uint ChannelID;
        public uint Flags;
        public uint FactionGroup;
        public string Name;
        public string Shortcut;
    }
}
