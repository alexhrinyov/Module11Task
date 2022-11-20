using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AlexHrinyovBot2.Services
{
    public class MessageHandler
    {
        List<int> numList = new List<int>();


        /// <summary>
        /// Обработка сообщения в случае, если сделан выбор операции
        /// </summary>
        /// <param name="OperationType"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Handle(string OperationType, string text)
        {

            switch (OperationType)
            {
                case "cn":
                    return $"Number of characters = {text.Length}";
                case "sn":
                    return $"{Calculate(text)}";
                default:
                    return "Введите информацию.";
            }



        }
        /// <summary>
        /// Вычисление суммы чисел, которые есть в строке
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Calculate(string text)
        {
            string tempresString = "";
            numList.Clear();
            int i = 0;
            foreach (var item in text)
            {
                i++;
                if (item != ' ')
                {
                    if (char.IsDigit(item))
                    {

                        tempresString += item;
                    }
                    else
                    {
                        return "Something wrong. Type numbers with ' '. ";
                    }

                    if (i == text.Length)
                    {

                        numList.Add(int.Parse(tempresString));
                    }
                }
                if (item == ' ')
                {
                    numList.Add(int.Parse(tempresString));
                    tempresString = "";
                }

            }
            return "Sum of numbers = " + numList.Sum().ToString();
        }
    }
}
