using System.Collections.Generic;
using UnityEngine;

namespace PollenModule
{
    ﻿using System.Collections;

    public static class StringColorExtensions
    {
        public static string AddColor(this string text, Color col) => $"<color={ColorHexFromUnityColor(col)}>{text}</color>";
        public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";
    }
}
