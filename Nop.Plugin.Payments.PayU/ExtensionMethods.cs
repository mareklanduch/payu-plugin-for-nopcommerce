using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.PayU
{
    public static class ExtensionMethods
    {
        public static string ConvertToMd5(this string text)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(text);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public static string ConvertToSha256(this string text)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(text));
            foreach (var b in crypto)
            {
                hash.Append(b.ToString("x2"));
            }
            return hash.ToString();
        }

        public static string ConvertToSha384(this string text)
        {
            var crypt = new SHA384Managed();
            var hash = new StringBuilder();
            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(text));
            foreach (var b in crypto)
            {
                hash.Append(b.ToString("x2"));
            }
            return hash.ToString();
        }

        public static string ConvertToSha512(this string text)
        {
            var crypt = new SHA512Managed();
            var hash = new StringBuilder();
            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(text));
            foreach (var b in crypto)
            {
                hash.Append(b.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
