using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Carwale.Utility
{
    public static class RegexManipulator
    {
        public static string RemoveStyleAttribute(string input)
        {
            return Regex.Replace(input, @"style=""[^\""]*""", "");
        }
        
        public static string RemoveColgroupTags(string input)
        {
            return Regex.Replace(input, @"<colgroup(.*?)</colgroup>", "");
        }

        public static string RemoveEmbed(string input)
        {
            return Regex.Replace(input, @"<embed(.*?)(\/embed>|\/>)", "");
        }

        public static string RemoveFormTag(string input)
        {
            return Regex.Replace(input, @"<form((.|\n)*?)<\/form>", "");
        }

        public static string RemoveAmpProhibitedTags(string input)
        {
            return Regex.Replace(input, @"(style=""[^\""]*"")|(<form((.|\n)*?)<\/form>)|(<colgroup(.*?)<\/colgroup>)|(<embed(.*?)(\/embed>|\/>))|(align=""(.*)""|align='(.*)')|(frameborder=""[0-9]*"")|(border=""[0-9]*"")", "");
        }
    }
}
