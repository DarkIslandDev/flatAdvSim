using System;
using UnityEngine;

public static class FormatNums
{
    private static string[] names = new[]
    {
        "",
        "a",
        "b",
        "c",
        "d",
        "f",
        "g",
        "e",
        "f",
        "g",
        "h",
        "i",
        "j",
        "k",
        "l",
        "m",
        "n",
        "o",
        "p",
        "q",
        "r",
        "s",
        "t",
        "u",
        "v",
        "w",
        "x",
        "y",
        "z",
        "aa",
        "ab",
        "ac",
        "ad",
        "af",
        "ag",
        "ae",
        "af",
        "ag",
        "ah",
        "ai",
        "aj",
        "ak",
        "al",
        "am",
        "an",
        "ao",
        "ap",
        "aq",
        "ar",
        "as",
        "at",
        "au",
        "av",
        "aw",
        "ax",
        "ay",
        "az",
    };

    public static string FormatNum(double num)
    {
        if(num == 0) return "0";

        num = Mathf.Round((float) num);

        int i = 0;
        while(i + 1 < names.Length && num >= 10000d)
        {
            num /= 1000d;
            i++;
        }

        return num.ToString("##") + names[i];
    }
    public static string FormatNum(float num)
    {
        if(num == 0) return "0";

        num = Mathf.Round(num);

        int i = 0;
        while(i + 1 < names.Length && num >= 10000f)
        {
            num /= 1000f;
            i++;
        }

        return num.ToString("##") + names[i];
    }

    public static string FormatNum(decimal num)
    {
        if(num == 0) return "0";

        num = Decimal.Round(num);

        int i = 0;
        while(i + 1 < names.Length && num >= 10000m)
        {
            num /= 1000m;
            i++;
        }

        return num.ToString("##") + names[i];
    }
}
