using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 拡張メソッド
/// </summary>
public static class Extensions
{
    public static T GetMin<T, U>(this IEnumerable<T> elements, Func<T, U> selector) where U : IComparable<U>
    {
        var min = elements.FirstOrDefault();

        foreach (var e in elements)
        {
            if (selector.Invoke(e).CompareTo(selector.Invoke(min)) < 0)
            {
                min = e;
            }
        }

        return min;
    }
}
