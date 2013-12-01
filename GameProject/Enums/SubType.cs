using System.ComponentModel;
namespace GameProject.Enums
{
    public enum WeaponType
    {
        [Description("Jednoręczna")]
        OneHanded = 1,

        [Description("Dwuręczna")]
        TwoHanded = 2
    }

    public enum JewelryType
    {
        [Description("Amulet")]
        Amulet = 3,

        [Description("Pierścień")]
        Ring = 4
    }

    public enum SubType
    {
        OneHanded = 1,
        TwoHanded = 2,
        Amulet = 3,
        Ring = 4
    }
}
