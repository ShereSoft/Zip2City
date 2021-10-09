﻿using System;
using System.Collections.Generic;
#if !NET20
using System.Linq;
#endif

public partial class Zip2City
{
    static Dictionary<int, string[][]> CitiesStatesByZip = new Dictionary<int, string[][]>();

    /// <summary>
    /// Performs initialization
    /// </summary>
    static Zip2City()
    {
        var consolidated = new[]
        {
            CS00000,
            CS00500,
            CS01000,
            CS01500,
            CS02000,
            CS02500,
            CS03000,
            CS03500,
            CS04000,
            CS04500,
            CS05000,
            CS05500,
            CS06000,
            CS06500,
            CS07000,
            CS07500,
            CS08000,
            CS08500,
            CS09000,
            CS09500,
            CS10000,
            CS10500,
            CS11000,
            CS11500,
            CS12000,
            CS12500,
            CS13000,
            CS13500,
            CS14000,
            CS14500,
            CS15000,
            CS15500,
            CS16000,
            CS16500,
            CS17000,
            CS17500,
            CS18000,
            CS18500,
            CS19000,
            CS19500,
            CS20000,
            CS20500,
            CS21000,
            CS21500,
            CS22000,
            CS22500,
            CS23000,
            CS23500,
            CS24000,
            CS24500,
            CS25000,
            CS25500,
            CS26000,
            CS26500,
            CS27000,
            CS27500,
            CS28000,
            CS28500,
            CS29000,
            CS29500,
            CS30000,
            CS30500,
            CS31000,
            CS31500,
            CS32000,
            CS32500,
            CS33000,
            CS33500,
            CS34000,
            CS34500,
            CS35000,
        };

        const int entryCount = 35285;
        var cityStateConsolidated = new ulong[entryCount];
        
        const int batchSize = 500;
        int loopLimit = 35285 / batchSize;

        for (int i = 0, j = 0; i < loopLimit; i++, j+= batchSize)
        {
            Array.Copy(consolidated[i], 0, cityStateConsolidated, j, batchSize);
        }

        int destIdx = batchSize * loopLimit;
        var lastBatch = consolidated[consolidated.Length - 1];

        Array.Copy(lastBatch, 0, cityStateConsolidated, destIdx, lastBatch.Length);


        var zips = DecodeZipCodes(ZipCodes);
        var cityStates = DecodeCityStates(cityStateConsolidated);

        for (int i = 0; i < zips.Length; i++)
        {
            var intzip = int.Parse(zips[i]);
            var entry = new[] { cityStates[i][0], cityStates[i][1] };

            if (CitiesStatesByZip.TryGetValue(intzip, out var list))
            {
                var resized = new string[list.Length + 1][];

                if (cityStates[i][2] != null)
                {
                    Array.Copy(list, 0, resized, 1, list.Length);
                    resized[0] = entry;

                    CitiesStatesByZip[intzip] = resized;
                }
                else
                {
                    Array.Copy(list, resized, list.Length);
                    resized[list.Length] = entry;

                    CitiesStatesByZip[intzip] = resized;
                }
            }
            else
            {
                CitiesStatesByZip.Add(intzip, new[] { entry });
            }
        }
    }

    /// <summary>
    /// Gets the default city and state by zip code.
    /// </summary>
    /// <param name="zipcode">A valid zip Code</param>
    /// <returns>A string array for the default city and state. Null if no data is found for the specified zip code.</returns>
    public static string[] GetDefaultCityState(string zipcode)
    {
        foreach (var citystate in GetAllCityStates(zipcode))
        {
            return citystate;
        }

        return null;
    }

    /// <summary>
    /// Gets the cities and states by zip code.
    /// </summary>
    /// <param name="zipcode">A valid zip Code</param>
    /// <returns>A list of string arrays for cities and states. Null if no data is found for the specified zip code.</returns>
    public static IEnumerable<string[]> GetAllCityStates(string zipcode)
    {
        if (zipcode == null)
        {
            throw new ArgumentNullException(nameof(zipcode));
        }

#if !NET20
        if (zipcode.Length != 5 || !zipcode.All(Char.IsNumber))
        {
            throw new ArgumentException($"'{zipcode}' is not a ZIP code.");
        }
#else
        if (zipcode.Length != 5)
        {
            var allNumber = true;

            foreach (var c in zipcode)
            {
                if (!Char.IsNumber(c))
                {
                    allNumber = false;
                    break;
                }
            }

            if (!allNumber)
            {
                throw new ArgumentException($"'{zipcode}' is not a ZIP code.");
            }
        }
#endif

        var key = int.Parse(zipcode);

        CitiesStatesByZip.TryGetValue(key, out var citiestates);

        if (citiestates != null)
        {
            foreach (var citystate in citiestates)
            {
                yield return citystate;
            }
        }
    }

    /// <summary>
    /// Gets a valid set of city, state and zip code at random
    /// </summary>
    /// <returns>A valid set of city, state and zip code</returns>
    public static string[] GetRandomCityStateZip()
    {
        var random = Math.Abs(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
        var index = random % CitiesStatesByZip.Count;
        int i = 0;
        string[][] cityState = null;

        foreach (var key in CitiesStatesByZip.Keys)
        {
            if (i++ == index)
            {
                i = key;
                cityState = CitiesStatesByZip[key];
                break;
            }
        }

        if (cityState == null)
        {
            var enumerator = CitiesStatesByZip.Keys.GetEnumerator();
            enumerator.MoveNext();

            i = enumerator.Current;
            cityState = CitiesStatesByZip[i];
        }

        index = random % cityState.Length;

        return new[] { cityState[index][0], cityState[index][1], i.ToString("00000") };
    }

    static string[] DecodeZipCodes(ulong[] data)
    {
        var zips = new string[56289];
        var chars = new char[5];
        int zc = 0;
        int cc = 0;

        foreach (var d in data)
        {
            for (int i = 60; i >= 0; i -= 4)
            {
                var c = (d << i) >> 60;

                if (c > 0)
                {
                    if (cc == 5)
                    {
                        zips[zc++] = new String(chars, 0, 5);
                        cc = 0;
                    }

                    chars[cc++] = (char)((long)c + 47);
                }
            }
        }

        zips[zc] = new String(chars, 0, 5);

        return zips;
    }

    static string[][] DecodeCityStates(ulong[] data)
    {
        var names = new string[56289][];
        var chars = new char[31];
        int nc = 0;
        int cc = 0;

        foreach (var d in data)
        {
            for (int i = 59; i >= 0; i -= 5)
            {
                var c = (d << i) >> 59;

                if (c == 29 || c == 31)
                {
                    names[nc++] = new[] { String.Intern(new String(chars, 0, cc - 2)), String.Intern(new String(chars, cc - 2, 2)), c == 31 ? "" : null };
                    cc = 0;
                }
                else if (c == 28)
                {
                    chars[cc++] = ' ';
                }
                else if (c == 30)
                {
                    foreach (var rc in names[nc - 1][cc == 0 ? 0 : 1])
                    {
                        chars[cc++] = rc;
                    }
                }
                else if (c > 0)
                {
                    chars[cc++] = (char)((long)c + 64);
                }
            }
        }

        return names;
    }
}