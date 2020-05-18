using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityScript.Steps;

public static class Extensions
{
    /// <summary>
    /// Generic logging utility that calls Debug.Log() for each item in the IEnumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    public static void LogContents<T>(this T enumerable) where T: IEnumerable
    {
        uint index = 0;
        foreach(var item in enumerable)
        {
            Debug.Log($"[{index}]:\t{item}");
        }
    }
}
