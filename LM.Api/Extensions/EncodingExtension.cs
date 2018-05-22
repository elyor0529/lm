﻿using System.Text;
using System.Web;

namespace LM.Api.Extensions
{
    public static class EncodingExtension
    {
        public static string Base64ForUrlEncode(this string str)
        {
            byte[] encbuff = Encoding.UTF8.GetBytes(str);

            return HttpServerUtility.UrlTokenEncode(encbuff);
        }

        public static string Base64ForUrlDecode(this string str)
        {
            byte[] decbuff = HttpServerUtility.UrlTokenDecode(str);

            return Encoding.UTF8.GetString(decbuff);
        }
    }
}
