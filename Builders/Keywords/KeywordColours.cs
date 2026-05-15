#region

using UnityEngine;

#endregion

namespace Stormriders.Builders.Keywords;

public static class KeywordColours
{
    public static readonly Color Orange = Color(255, 202, 87);
    public static readonly Color White = Color(255, 255, 255);
    public static readonly Color Gray = Color(166, 166, 166);
    public static readonly Color DarkPurple = Color(74, 57, 148);
    public static readonly Color LightPurple = Color(100, 67, 158);
    public static readonly Color Blue = Color(91, 206, 250);
    public static readonly Color Pink = Color(245, 169, 184);

    public static readonly Color TextShadow = Color(120, 95, 185);
    public static readonly Color Scavenge = Color(72, 195, 157);
    public static readonly Color Trample = Color(56, 235, 164);
    public static readonly Color CatColor = Color(220, 60, 60);
    public static readonly Color EtherealColor = new(0.35f, 0.35f, 0.45f);
    public static readonly Color EquipColor = Color(180, 44, 84);

    public static readonly Color Flavour = Color(188, 188, 224);
    
    private static Color Color(int r, int g, int b)
    {
        Color color = new(
            r / 255F,
            g / 255F,
            b / 255F
        );

        return color;
    }
}