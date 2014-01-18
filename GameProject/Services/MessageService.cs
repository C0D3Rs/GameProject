using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.Services
{
    public class MessageService
    {
        public string GetHtmlSystemRaport(string message)
        {
            return message.Replace("<maindescription>", "<div class=\"main-description\">").Replace("</maindescription>", "</div>")
                .Replace("<successdescription>", "<div class=\"success-description\">").Replace("</successdescription>", "</div>")
                .Replace("<lostdescription>", "<div class=\"lost-description\">").Replace("</lostdescription>", "</div>")

                // Runda
                .Replace("<roundvalue>", "<div class=\"round\">Runda ").Replace("</roundvalue>", "</div>")

                // Postać atakuje potwora
                .Replace("<characternamewhenattack>", "<div class=\"character-attack\">").Replace("</characternamewhenattack>", " zdaje obrażenia, ")
                .Replace("<characterattackmonstername>", "").Replace("</characterattackmonstername>", " zostaje zraniony za ")
                .Replace("<characterattackdmg>", "").Replace("</characterattackdmg>", " PKT ŻYCIA.</div>")

                // Postać zadaje ostatnie obrażenia potworowi
                .Replace("<characternamewhenfinalattack>", "<div class=\"character-final-attack\"").Replace("</characternamewhenfinalattack>", " zadał ostateczny atak przeciwnikowi wynoszący ")
                .Replace("<characterattackfinaldmg>", "").Replace("</characterattackfinaldmg>", " PKT ŻYCIA.</div>")

                // Postać chybia podczas ataku
                .Replace("<characternamewhenmonsterdododge>", "<div class=\"character-attack-miss\">").Replace("</characternamewhenmonsterdododge>", " zadaje obrażenia, ")
                .Replace("<charactermonsternamewhenmonsterdododge>", "").Replace("</charactermonsternamewhenmonsterdododge>", " unika ciosu.</div>")

                // Potwór atakuje postać
                .Replace("<monsterattackmonstername>", "<div class=\"monster-attack\">").Replace("</monsterattackmonstername>", " zadaje obrażenia, ")
                .Replace("<monsterattackcharactername>", "").Replace("</monsterattackcharactername>", " zostaje zraniony za ")
                .Replace("<monsterattackdmg>", "").Replace("</monsterattackdmg>", " PKT ŻYCIA.</div>")

                // Potwór zadaje ostatnie obrażenia postaci
                .Replace("<monsterfinalattackmonstername>", "<div class=\"monster-final-attack\"").Replace("</monsterfinalattackmonstername>", " zadał Tobie ostateczny atak wynoszący ")
                .Replace("<monsterfinalattackdmg>", "").Replace("</monsterfinalattackdmg>", " PKT ŻYCIA.</div>")

                // Potwór chybia podczas ataku
                .Replace("<monstermissattackmonstername>", "<div class=\"monster-attack-miss\">").Replace("</monstermissattackmonstername>", " zadaje obrażenia, ")
                .Replace("<monstermissattackcharactername>", "").Replace("</monstermissattackcharactername>", " unika ciosu.</div>")

                // Podsumowanie
                .Replace("<summationcharactername>", "<div class=\"character-summation\">").Replace("</summationcharactername>", " ")
                .Replace("<summationcharacterlifefalldown>", "").Replace("</summationcharacterlifefalldown>", "")
                .Replace("<summationcharacterlifeconstantly>", "/").Replace("</summationcharacterlifeconstantly>", "</div>")

                .Replace("<summationmonstername>", "<div class=\"monster-summation\">").Replace("</summationmonstername>", " ")
                .Replace("<summationmonsterlifefalldown>", "").Replace("</summationmonsterlifefalldown>", "")
                .Replace("<summationmonsterlifeconstantly>", "/").Replace("</summationmonsterlifeconstantly>", "</div>");
        }
    }
}
