using DBFilesClient.NET;

namespace FileStructures.DBC.Cataclysm
{
    public sealed class ImportPriceArmorEntry
    {
        public uint InventoryType;
        public float ClothFactor;
        public float LeatherFactor;
        public float MailFactor;
        public float PlateFactor;
    }

    public sealed class ImportPriceQualityEntry
    {
        public uint QualityId;
        public float Factor;
    }

    public sealed class ImportPriceShieldEntry
    {
        public uint Id;
        public float Factor;
    }

    public sealed class ImportPriceWeaponEntry
    {
        public uint Id;
        public float Factor;
    }
}
