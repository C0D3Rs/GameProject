using GameProject.Models.Entities;
using GameProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Helpers
{
    public class CharacterHelpers
    {
        public static CharacterResourcesViewModel GetCharacterResources(HttpContextBase context)
        {
            if (!context.Items.Contains("CharacterResources"))
            {
                return null;
            }

            return context.Items["CharacterResources"] as CharacterResourcesViewModel;
        }
        public static string GetHtmlBattleResult(string battleResult)
        {
            return battleResult.Replace("\n", "<br />")
                //Character
            .Replace("<characterattack>", " ").Replace("</characterattack>", " <br/ >") //characterattack
            .Replace("<characterfinalattack>", "").Replace("</characterfinalattack>", "<br />") //characterfinalattack
            .Replace("<charactermissattack>", "").Replace("</charactermissattack>", "<br />") //charactermissattack
            .Replace("<characterwinthebattle>", "Wygrałeś bitwę.").Replace("</characterwinthebattle>", "<br />") //characterwinthebattle

            //Character - tagi oznaczone w walce.
                // -- nazwa charactera podczas ataku
            .Replace("<characternamewhenattack>", "").Replace("</characternamewhenattack>", " zdaje obrażenia, ")
                // -- nazwa monstera, kiedy character wykonuje atak
            .Replace("<characterattackmonstername>", "").Replace("</characterattackmonstername>", " zostaje zraniony za ")
                // -- dotyczy DMG charactera, podczas ataku charactera
            .Replace("<characterattackdmg>", "").Replace("</characterattackdmg>", " PKT ŻYCIA.")

             //Character - tagi oznaczone podczas ostatniego ataku ( final dmg )
                // -- nazwa charactera podczas finałowego ostatniego ataku
            .Replace("<characternamewhenfinalattack>", "").Replace("</characternamewhenfinalattack>", " zadał ostateczny atak przeciwnikowi wynoszący ")
                // -- dotyczy DMG charactera, podczas finałowego ataku
            .Replace("<characterattackfinaldmg>", "").Replace("</characterattackfinaldmg>", " PKT ŻYCIA.")
                // -- nazwa charactera podczas ataku charactera na monstera kiedy ten robi UNIK
            .Replace("<characternamewhenmonsterdododge>", "").Replace("</characternamewhenmonsterdododge>", " zadaje obrażenia, ")
                // -- nazwa monstera podczas ataku charactera na monstera kiedy ten robi UNIK
            .Replace("<charactermonsternamewhenmonsterdododge>", "").Replace("</charactermonsternamewhenmonsterdododge>", " unika ciosu. ")

            //Monster
            .Replace("<monsterattack>", "").Replace("</monsterattack>", "<br />") //monsterattack
            .Replace("<monsterfinalattack>", "").Replace("</monsterfinalattack>", "<br />") //monsterfinalattack
            .Replace("<monstermissattack>", "").Replace("</monstermissattack>", "<br />") //monstermissattack
            .Replace("<monsterwinthebattle>", "Przegrałeś bitwę.").Replace("</monsterwinthebattle>", "<br />") //monsterwinthebattle

           //Monster - tagi oznaczone w walce. 
                // -- nazwa monstera podczas ataku monstera na charactera
           .Replace("<monsterattackmonstername>", "").Replace("</monsterattackmonstername>", " zadaje obrażenia, ")
                // -- nazwa charactera podczas ataku monstera 
           .Replace("<monsterattackcharactername>", "").Replace("</monsterattackcharactername>", " zostaje zraniony za ")
                // -- dmg monstera podczas ataku monstera na charactera
           .Replace("<monsterattackdmg>", "").Replace("</monsterattackdmg>", " PKT ŻYCIA.")
                // -- nazwa monstera podczas ostatecznego jego ataku ( final dmg )
           .Replace("<monsterfinalattackmonstername>", "").Replace("</monsterfinalattackmonstername>", " zadał Tobie ostateczny atak wynoszący ")
                // -- DMG monstera podczas ataku finalnego ataku monstera na charactera
           .Replace("<monsterfinalattackdmg>", "").Replace("</monsterfinalattackdmg>", " PKT ŻYCIA.")
                // -- nazwa monstera podczas UNIKu charactera
           .Replace("<monstermissattackmonstername>", "").Replace("</monstermissattackmonstername>", " zadaje obrażenia, ")
                // -- nazwa charactera podczas ataku monstera na charactera, gdy character robi UNIK
           .Replace("<monstermissattackcharactername>", "").Replace("</monstermissattackcharactername>", " unika ciosu.")


            //Round
            .Replace("<round>", "Runda: ").Replace("</round>", "<br />") //round
                //Round value
            .Replace("<roundvalue>", "").Replace("</roundvalue>", "")

             //Podsumowanie
             .Replace("<summation>", "<br />Podsumowanie <br />").Replace("</summation>", "<br /><br />")

             //Podsumowanie - dotyczy NAPISU PODSUMOWANIE
             .Replace("<summationname>", "").Replace("</summationname>", "")

             //Podsumowanie - nazwa characteru
             .Replace("<summationcharactername>", "").Replace("</summationcharactername>", " ")
                //Podsumowanie - spadające życie charactera. Wartość cały czas się zmienia.
             .Replace("<summationcharacterlifefalldown>", "").Replace("</summationcharacterlifefalldown>", "")
                //Podsumowanie - życie charactera po slashu "/". Jest ono stałe.
             .Replace("<summationcharacterlifeconstantly>", "").Replace("</summationcharacterlifeconstantly>", "<br />")

             //Podsumowanie - nazwa monstera
            .Replace("<summationmonstername>", "").Replace("</summationmonstername>", " ")
                //Podsumowanie - spadające życie monstera. Wartość się zmienia.
             .Replace("<summationmonsterlifefalldown>", "").Replace("</summationmonsterlifefalldown>", "")
                //Podsumowanie - stałe życie monstera które nie ulega zmianie. Występuje ono po slashu "/"
             .Replace("<summationmonsterlifeconstantly>", "").Replace("</summationmonsterlifeconstantly>", "");

        }
    }
}
