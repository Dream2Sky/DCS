using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.dcs.common
{
    public class RandomManager
    {
        /// <summary>
        /// 生成 a-z 0-9 的 count 位 随机序列
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GenerateRandom(int count)
        {
            string[] array = new string[36] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "a",
                "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            string result = string.Empty;

            for (int i = 0; i < count; i++)
            {
                var rand = new Random(35).Next(0, 35);
                result += array[rand];
            }

            return result;
        }
    }
}
