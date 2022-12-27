using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadTask2.Services
{
    public class CryptographyService
    {
        public static string CodingToBase64(string data)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(data);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
