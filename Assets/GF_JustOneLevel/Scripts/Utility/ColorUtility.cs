using UnityEngine;
using System.Collections;
using System;

public static class ColorUtility  {
    /// <summary>
    /// 将RGB字符串转换为Color对象
    /// </summary>
    /// <param name="rgb"></param>
    /// <returns></returns>
    public static Color ToColor(this string rgb)
    {
        rgb = rgb.Replace("#", "");

        string R = "FF";
        string G = "FF";
        string B = "FF";
        string A = "FF";

        if(rgb.Length == 8)
        {
            R = rgb.Substring(0, 2);
            G = rgb.Substring(2, 2);
            B = rgb.Substring(4, 2);
            A = rgb.Substring(6, 2);
        }
        else if (rgb.Length == 6)
        {
            R = rgb.Substring(0, 2);
            G = rgb.Substring(2, 2);
            B = rgb.Substring(4, 2);
        }
        else if(rgb.Length == 3)
        {
            R = rgb.Substring(0, 1);
            G = rgb.Substring(1, 1);
            B = rgb.Substring(2, 1);
        }
        else
        {
            throw new Exception("该字符串不是RBG格式");
        }
        return new Color(Convert.ToInt16(R, 16) / 255f, Convert.ToInt16(G, 16) / 255f, Convert.ToInt16(B, 16) / 255f, Convert.ToInt16(A, 16) / 255f);
    }
}
