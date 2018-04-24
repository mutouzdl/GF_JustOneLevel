using System;

/// <summary>
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
public static class WebUtility {
    public static string EscapeString (string stringToEscape) {
        return Uri.EscapeDataString (stringToEscape);
    }

    public static string UnescapeString (string stringToUnescape) {
        return Uri.UnescapeDataString (stringToUnescape);
    }
}