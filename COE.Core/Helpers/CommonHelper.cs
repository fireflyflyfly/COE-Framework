using COE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace COE.Core.Helpers
{
    public class CommonHelper
    {

        private Random rnd = new Random();

        public DateTime RandomDay
        {
            get
            {
                var start = new DateTime(2011, 1, 1);
                var range = (DateTime.Today - start).Days;
                return start.AddDays(rnd.Next(range));
            }
        }

        public string DateTimeNowId
        {
            get
            {
                return DateTime.Now.ToString("MMddHHmmss");
            }
        }

        /// <summary>
        /// This method returns a random lowercase letter
        /// Between 'a' and 'z' inclusive.
        /// </summary>
        public char GetRandomLetterAZ()
        {
            char let = (char)('a' + rnd.Next(0, 25));
            return let;
        }

        public List<T> Sort<T>(List<T> list, IFilter filter) where T : ISortableModel
        {
            PropertyInfo sortProp = typeof(T).GetProperty(filter.SortColumn);
            return new List<T>(filter.SortOrder.Equals("ASC")
                ? list.OrderBy(x => sortProp.GetValue(x))
                : list.OrderByDescending(x => sortProp.GetValue(x)));
        }
    }
}
