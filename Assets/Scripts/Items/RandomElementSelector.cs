using System;
using System.Collections.Generic;

public class RandomElementSelector
{
    private static Random random = new Random();

    public static T GetRandomElement<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new ArgumentException("The list cannot be null or empty.");
        }

        int index = random.Next(list.Count);
        return list[index];
    }
}
