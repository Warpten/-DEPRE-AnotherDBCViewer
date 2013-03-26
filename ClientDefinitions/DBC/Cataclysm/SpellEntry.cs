using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class SpellEntry
    {
        public uint Id;
        public uint Attributes;
        public uint AttributesEx;
        public uint AttributesEx2;
        public uint AttributesEx3;
        public uint AttributesEx4;
        public uint AttributesEx5;
        public uint AttributesEx6;
        public uint AttributesEx7;
        public uint AttributesEx8;
        public uint AttributesEx9;
        public uint AttributesEx10;
        public uint CastingTimeIndex;
        public uint DurationIndex;
        public uint PowerType;
        public uint RangeIndex;
        public float Speed;

        [StoragePresence(StoragePresenceOption.Include, ArraySize = 2)]
        public uint[] SpellVisual;

        public uint SpellIconID;
        public uint ActiveIconID;
        public string SpellName;
        public string Rank;
        public string Description;
        public string ToolTip;
        public uint SchoolMask;
        public uint RuneCostID;
        public uint SpellMissileID;
        public uint SpellDescriptionVariableID;
        public uint SpellDifficultyId;
        public float Unknown3;
        public uint SpellScalingId;
        public uint SpellAuraOptionsId;
        public uint SpellAuraRestrictionsId;
        public uint SpellCastingRequirementsId;
        public uint SpellCategoriesId;
        public uint SpellClassOptionsId;
        public uint SpellCooldownsId;
        public uint Unknown4;
        public uint SpellEquippedItemsId;
        public uint SpellInterruptsId;
        public uint SpellLevelsId;
        public uint SpellPowerId;
        public uint SpellReagentsId;
        public uint SpellShapeshiftId;
        public uint SpellTargetRestrictionsId;
        public uint SpellTotemsId;
        public uint ResearchProject;
    }
}
