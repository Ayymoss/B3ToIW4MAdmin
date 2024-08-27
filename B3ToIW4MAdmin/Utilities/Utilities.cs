using System.Net;

namespace B3ToIW4MAdmin.Utilities;

public static class Utilities
{
    public static int? ConvertToIp(this string str)
    {
        if (!IPAddress.TryParse(str, out var ip)) return null;

        return ip.GetAddressBytes().Count(b => b is 0) != 4
            ? BitConverter.ToInt32(ip.GetAddressBytes(), 0)
            : null;
    }

    public static string CapClientName(this string name, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(name)) return "-";
        return name.Length > maxLength ? $"{name[..(maxLength - 3)]}..." : name;
    }
}
