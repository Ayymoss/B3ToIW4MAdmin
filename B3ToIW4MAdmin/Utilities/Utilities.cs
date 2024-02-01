using System.Net;

namespace B3ToIW4MAdmin.Utilities;

public static class Utilities
{
    public static int? ConvertToIP(this string str)
    {
        var success = IPAddress.TryParse(str, out var ip);
        return success && ip.GetAddressBytes().Count(_byte => _byte == 0) != 4
            ? BitConverter.ToInt32(ip.GetAddressBytes(), 0)
            : null;
    }

    public static string CapClientName(this string name, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "-";
        }

        return name.Length > maxLength ? $"{name[..(maxLength - 3)]}..." : name;
    }
}
