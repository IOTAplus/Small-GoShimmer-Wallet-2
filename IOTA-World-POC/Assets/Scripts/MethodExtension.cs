using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

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
}
