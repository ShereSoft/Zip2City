using System;
using System.Collections.Generic;
using System.Linq;

public partial class Zip2City
{
    static Dictionary<int, string[]> Dict = new Dictionary<int, string[]>();

    static Zip2City()
    {
        var zips = DecodeZipCodes(ZipCodes);

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
        };

        var cityStateConsolidated = new ulong[consolidated.Sum(d => d.Length)];

        Array.Copy(CS00000, 0, cityStateConsolidated, 0, CS00000.Length);

        for (int i = 1; i < consolidated.Length; i++)
        {
            Array.Copy(consolidated[i], 0, cityStateConsolidated, consolidated.Take(i).Sum(d => d.Length), consolidated[i].Length);
        }

        var cityStates = DecodeCityStates(cityStateConsolidated);

        for (int i = 0; i < zips.Length; i++)
        {
            Dict.Add(int.Parse(zips[i]), cityStates[i]);
        }
    }

    /// <summary>
    /// Gets the default city and state by ZIP code.
    /// </summary>
    /// <param name="zipcode">A valid USPS ZIP Code</param>
    /// <returns>A string array containing the default city and state. Null if no data is found for the specified ZIP code.</returns>
    public static string[] GetDefaultCityState(string zipcode)
    {
        if (zipcode == null)
        {
            throw new ArgumentNullException(nameof(zipcode));
        }

        if (zipcode.Length != 5 || !zipcode.All(Char.IsNumber))
        {
            throw new ArgumentException($"'{zipcode}' is not a ZIP code.");
        }

        var key = int.Parse(zipcode);

        Dict.TryGetValue(key, out var citystate);
        return citystate;
    }

    static string[] DecodeZipCodes(ulong[] data)
    {
        var zips = new string[40852];
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
        var names = new string[40852][];
        var chars = new char[31];
        int nc = 0;
        int cc = 0;

        foreach (var d in data)
        {
            for (int i = 59; i >= 0; i -= 5)
            {
                var c = (d << i) >> 59;

                if (c == 31)
                {
                    names[nc++] = new[] { String.Intern(new String(chars, 0, cc - 2)), String.Intern(new String(chars, cc - 2, 2)) };
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
