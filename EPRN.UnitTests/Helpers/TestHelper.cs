using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPRN.UnitTests.Helpers
{
    internal static class TestHelper
    {
        public static bool CompareList<T>(List<T> list1, List<T> list2, bool? orderDescending = null)
        {
            foreach (var item in list1)
            {
                if (!list2.Contains(item))
                    return false;
            }

            return true;
        }

        public static bool CompareOrderedList<T, TKey>(List<T> orderedList, List<T> list2, Func<T, TKey> orderPredicate)
            where T : class
        {
            var orderList2 = list2.OrderBy(orderPredicate).ToList();

            foreach (var item in orderedList)
            {
                var i = orderedList.IndexOf(item);

                if (orderList2[i] != item)
                    return false;
            }

            return true;
        }

        public static bool CompareDescendingOrderedList<T, TKey>(List<T> descendingOrderedList, List<T> list2, Func<T, TKey> orderPredicate)
            where T : class
        {
            var orderList2 = list2.OrderByDescending(orderPredicate).ToList();

            foreach (var item in descendingOrderedList)
            {
                var i = descendingOrderedList.IndexOf(item);

                if (orderList2[i] != item)
                    return false;
            }

            return true;
        }
    }
}
