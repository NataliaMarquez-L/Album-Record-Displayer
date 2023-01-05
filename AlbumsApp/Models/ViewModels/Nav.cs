using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsApp.Models.ViewModels
{
    public class Nav
    {
        /// <summary>
        /// A static helper method (in a static clas that returns the string "active" for bootstrap CSS styling
        /// if the parameters match.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static string Active(string value, string current)
        {
            return (value == current) ? "active" : string.Empty;
        }
    }
}
