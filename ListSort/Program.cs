using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ListSort
{

    public class Thing
    {
        public Thing(string name, string label, int num)
        {
            tname = name;
            tlabel = label;
            number = num;
        }
        
        public string tname { get; set; }
        public string tlabel { get; set; }
        public int number { get; set; }
    }
    class Program
    {
        public static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            MemberExpression memberExpression = (MemberExpression)property.Body;

            return memberExpression.Member.Name;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<Thing> thingList = new List<Thing>
            {
                new Thing("betty", "label1", 1),
                new Thing("catherine", "label3", 2),
                new Thing("abel", "label4", 3),
                new Thing("dimitri", "label7", 4),
                new Thing("eigil", "label99", 5),
                new Thing("fritz", "label77", 6),
                new Thing("gertrude", "label45", 7),
                new Thing("", "label45", 8)
            };

            var sortedList1 = FilterHelper<Thing>.DynamicSort(thingList, SortOrder.Ascending, GetPropertyName((Thing t) => t.tlabel));
            var sortedList2 = FilterHelper<Thing>.DynamicSort(thingList, SortOrder.Ascending, GetPropertyName((Thing t) => t.tname));
            var sortedList3 = FilterHelper<Thing>.DynamicSort(thingList, SortOrder.Ascending, GetPropertyName((Thing t) => t.number));

            var sortedList4 = FilterHelper<Thing>.DynamicSort(thingList, SortOrder.Descending, GetPropertyName((Thing t) => t.tlabel));
            var sortedList5 = FilterHelper<Thing>.DynamicSort(thingList, SortOrder.Descending, GetPropertyName((Thing t) => t.tname));
            var sortedList6 = FilterHelper<Thing>.DynamicSort(thingList, SortOrder.Descending, GetPropertyName((Thing t) => t.number));


            var filteredList1 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilterType.Contains, "bet" );
            var filteredList2 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilterType.DoesNotContain, "bet");
            var filteredList3 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilterType.DoesNotStartWith, "ab");
            var filteredList4 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilterType.Is, "betty");
            var filteredList5 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilterType.IsBlank, string.Empty);
            var filteredList6 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilterType.IsNot, "fritz");
            var filteredList7 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilterType.IsNotBlank, string.Empty);
            var filteredList8 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilterType.StartsWith, "cat");


        }
    }

       
       


}


