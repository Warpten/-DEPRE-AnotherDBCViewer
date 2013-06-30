namespace FileStructures.DBC.Cataclysm
{
    public sealed class ResearchSiteEntry
    {
        public uint Id;
        public int MapId;
        public int QuestPOIPointGroupId; // field 4 QuestPOIPoint.dbc 
        public string Name;
        public int Unk0; // Flags ?
    }
}
