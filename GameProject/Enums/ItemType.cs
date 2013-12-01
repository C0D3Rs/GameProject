using System.ComponentModel;
namespace GameProject.Enums
{
    public enum ItemType
    {
        [Description("Broń")]
        Weapon = 1,

        [Description("Tarcza")]
        Shield = 2,

        [Description("Zbroja")]
        Armor = 3,

        [Description("Biżuteria")]
        Jewelry = 4
    }
}
