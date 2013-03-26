namespace FileStructures.DBC.Cataclysm
{
    // All Gt* DBC store data for 100 levels, some by 100 per class/race
    // GT_MAX_LEVEL    100
    // gtOCTClassCombatRatingScalar.dbc stores data for 32 ratings, look at MAX_COMBAT_RATING for real used amount
    // GT_MAX_RATING   32

    public sealed class gtBarberShopCostBaseEntry
    {
        public uint Level;
        public float Cost;
    }

    public sealed class gtCombatRatingsEntry
    {
        public uint Level;
        public float Ratio;
    }

    public sealed class gtChanceToMeleeCritBaseEntry
    {
        public uint Level;
        public float Base;
    }

    public sealed class gtChanceToMeleeCritEntry
    {
        public uint Level;
        public float Ratio;
    }

    public sealed class gtChanceToSpellCritBaseEntry
    {
        public float Base;
    }

    public sealed class gtChanceToSpellCritEntry
    {
        public float Ratio;
    }

    public sealed class gtOCTClassCombatRatingScalarEntry
    {
        public float Ratio;
    }

    public sealed class gtOCTRegenMPEntry
    {
        public float Ratio;
    }

    public sealed class gtOCTHpPerStaminaEntry
    {
        public float Ratio;
    }

    public sealed class gtRegenMPPerSptEntry
    {
        public float Ratio;
    }

    public sealed class gtSpellScalingEntry
    {
        public uint Index;
        public float Multiplier;
    }

    public sealed class gtOCTBaseHPByClassEntry
    {
        public float Ratio;
    }

    public sealed class gtOCTBaseMPByClassEntry
    {
        public float Ratio;
    }
}
