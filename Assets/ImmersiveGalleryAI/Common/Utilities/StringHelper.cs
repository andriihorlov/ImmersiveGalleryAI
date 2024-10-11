using System.Reflection;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.Utilities
{
    public static class StringHelper
    {
        public static string GetColoredString(this string s, Color color)
        {
            string colorName = "";
            PropertyInfo[] props = color.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static);

            foreach (PropertyInfo prop in props)
            {
                if ((Color) prop.GetValue(null, null) == color)
                {
                    colorName = prop.Name;
                }
            }

            if (colorName == "") colorName = color.ToString();

            return $"<color={colorName}> {s} </color>";
        }

        public static string GetBoldString(this string s)
        {
            return $"<b= {s} </b>";
        }
        
        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string[] words = input.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }
            return string.Join(" ", words);
        }
    }
}