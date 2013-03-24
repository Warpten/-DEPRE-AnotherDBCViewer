namespace FileStructures.DBC.Cataclysm
{
    public sealed class MapDifficultyEntry
    {
        public uint Id;
        public uint MapId;
        public uint Difficulty;
        public string AreaTriggerText;
        public uint ResetTime;
        public uint MaxPlayers;
        public string DifficultyString;
    }
}
