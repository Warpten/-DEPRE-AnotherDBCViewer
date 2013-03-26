namespace FileStructures.DBC.Cataclysm
{
    public sealed class AchievementEntry
    {
        public uint Id;
        public int RequiredFaction;
        public int MapId;
        public int ParentAchievement;
        public string Name;
        public string Description;
        public uint Category;
        public uint Points;
        public uint OrderInCategory;
        public uint Flags;
        public uint Icon;
        public string Reward;
        public uint Count;
        public uint ReferenceAchievement;
    }
}
