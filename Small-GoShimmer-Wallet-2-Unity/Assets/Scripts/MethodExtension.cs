using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public static class MethodExtension
{
    // Start is called before the first frame update
    public static IEnumerable<string> SplitByLine(this string str)
    {
        return Regex
            .Split(str, @"(\s(\r)+)?(\n)+((\r)+)?")
            .Select(i => i.Trim())
            .Where(i => !string.IsNullOrEmpty(i));
    }

    public static float GetValue(this ScrollRect scrollRect, float value)
    {
        return scrollRect.horizontal ?
                scrollRect.horizontalNormalizedPosition :
                scrollRect.verticalNormalizedPosition;
    }

    public static void SetValue(this ScrollRect scrollRect, float value)
    {
        if (scrollRect.horizontal)
        {
            scrollRect.horizontalNormalizedPosition = value;
        }
        else
        {
            scrollRect.verticalNormalizedPosition = value;
        }
    }
}
