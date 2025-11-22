using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static Board;

public static class RegionIdParser
{
    public static RegionId Parse(string name) {
        if (string.IsNullOrWhiteSpace(name)) {
            return RegionId.Unknown;
        }
        return name switch {
            "Attica" => RegionId.Attica,
            "Thessaly" => RegionId.Thessaly,
            "Laconia" => RegionId.Laconia,
            "Aetolia" => RegionId.Aetolia,
            "Epirus" => RegionId.Epirus,
            "Macedonia" => RegionId.Macedonia,
            "Chalkidiki" => RegionId.Chalkidiki,
            "Acarnania" => RegionId.Acarnania,
            "Locris" => RegionId.Locris,
            "Phocis" => RegionId.Phocis,
            "Euboea" => RegionId.Euboea,
            "Boeotia" => RegionId.Boeotia,
            "Corinthia" => RegionId.Corinthia,
            "Achaea" => RegionId.Achaea,
            "Elis" => RegionId.Elis,
            "Arcadia" => RegionId.Arcadia,
            "Messenia" => RegionId.Messenia,
            "Argolis" => RegionId.Argolis,
            "Crete" => RegionId.Crete,
            _ => RegionId.Unknown
        };
    }
    public static string IdToString(RegionId id) {
        return id switch {
            RegionId.Attica => "Attica",
            RegionId.Thessaly => "Thessaly",
            RegionId.Laconia => "Laconia",
            RegionId.Aetolia => "Aetolia",
            RegionId.Epirus => "Epirus",
            RegionId.Macedonia => "Macedonia",
            RegionId.Chalkidiki => "Chalkidiki",
            RegionId.Acarnania => "Acarnania",
            RegionId.Locris => "Locris",
            RegionId.Phocis => "Phocis",
            RegionId.Euboea => "Euboea",
            RegionId.Boeotia => "Boeotia",
            RegionId.Corinthia => "Corinthia",
            RegionId.Achaea => "Achaea",
            RegionId.Elis => "Elis",
            RegionId.Arcadia => "Arcadia",
            RegionId.Messenia => "Messenia",
            RegionId.Argolis => "Argolis",
            RegionId.Crete => "Crete",
            _ => "Unknown"
        };
    }
}
