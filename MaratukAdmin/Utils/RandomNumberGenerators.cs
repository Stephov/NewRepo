using MaratukAdmin.Entities;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace MaratukAdmin.Utils
{
    public class RandomNumberGenerators
    {
        private static Random random = new Random();

        public static string GenerateRandomNumber(int length)
        {
            StringBuilder randomNumber = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                randomNumber.Append(random.Next(0, 10));
            }
            return randomNumber.ToString();
        }
    }
}
