using System.ComponentModel;
namespace GameProject.Enums
{
    public enum CharacterClass
    {
        [Description("Wojownik")]
        Warrior = 1,

        [Description("Łucznik")]
        Archer = 3,

        [Description("Mag")]
        Mage = 2,
    }
}
