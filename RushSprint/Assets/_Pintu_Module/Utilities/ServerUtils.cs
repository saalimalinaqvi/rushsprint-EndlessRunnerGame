using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LNGT
{
    public static class ServerUtils
    {
        public static bool IsEmpty(string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        public static bool IsEmailValid(string email)
        {
            Regex regex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
            return regex.IsMatch(email.ToLower());
        }

        public static string GetFirstLetterCapital(string msg)
        {
            return string.IsNullOrEmpty(msg) ? null : (msg.Substring(0, 1).ToUpper() + msg.Substring(1, msg.Length - 1).ToLower());
        }

        public static string ToTitleCase(string value)
        {
            // Convert to proper case.
            CultureInfo culture_info = new CultureInfo(1);
            TextInfo text_info = culture_info.TextInfo;
            value = text_info.ToTitleCase(value);
            return value;
        }

        public static bool IsValidLink(string addressable_link)
        {
            return !addressable_link.Contains(";");
        }

        internal static string FormatToPNG(string url)
        {
            return string.Format("{0}.png", url);
        }


        public static string GetValue(string data)
        {
            return Regex.Replace(data, @"(\.\d+)?", "");
        }
    }


    [Serializable]
    public class DownloadedImageData<T>
    {
        public T image;
        public object data = null;

        public DownloadedImageData()
        {

        }

        public DownloadedImageData(T image, object data)
        {
            this.image = image;
            this.data = data;
        }

    }
}