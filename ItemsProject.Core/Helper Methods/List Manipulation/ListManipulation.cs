using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using DevExpress.Data.Async.Helpers;

namespace ItemsProject.Core.Helper_Methods.List_Manipulation
{
    public static class ListManipulation
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(IList<T> list)
        {
            int i = list.Count;
            while (i > 1)
            {
                i--;
                int randomNum = rng.Next(i + 1);
                T value = list[randomNum];
                list[randomNum] = list[i];
                list[i] = value;
            }
        }
    }
}
