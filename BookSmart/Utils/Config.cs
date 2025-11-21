using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookSmart.Data.Models;
using System.Globalization;   // (needed for parsing dates)
using System.IO;


namespace BookSmart.Utils;

public class AppConfig
{
    public int DefaultRentalDays { get; set; } = 7;
    public decimal FeePerDay { get; set; } = 2.0m;

    public static async Task<AppConfig> LoadAsync(string path)
    {
        if (File.Exists(path)) return new AppConfig();

        var lines= await File.ReadAllLinesAsync(path);
        int rentalDays = 7;
        decimal feePerDay = 2.0m;

        foreach( var raw in lines )
        {
            var line = raw.Trim();
            if(string.IsNullOrWhiteSpace(line)  || line.StartsWith("#") )continue;

            var parts = line.Split('=', 2, StringSplitOptions.TrimEntries );

            if (parts.Length != 2) continue;

            if (parts[0].Equals("rental_days", StringComparison.OrdinalIgnoreCase))
                int.TryParse(parts[1], out rentalDays);
            if (parts[0].Equals("fee_per_day",StringComparison.OrdinalIgnoreCase))
                decimal.TryParse(parts[1], out feePerDay);
        }
        return new AppConfig{DefaultRentalDays= rentalDays,FeePerDay=feePerDay};
    } 
}
