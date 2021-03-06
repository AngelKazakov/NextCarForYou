using System.IO;
using Microsoft.AspNetCore.Http;

namespace CarSalesSystem.Infrastructure
{
    public static class FormFileToByteArrayConverter
    {
        public static byte[] Convert(IFormFile source)
        {
            if (source != null)
            {
                MemoryStream target = new MemoryStream();
                source.CopyTo(target);
                return target.ToArray();
            }

            return null;
        }
    }
}