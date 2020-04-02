using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ListSort
{

    public class Thing
    {
        public Thing(string name, string label, int num, Version vers)
        {
            tname = name;
            tlabel = label;
            number = num;
            version = vers;
        }
        
        public string tname { get; set; }
        public string tlabel { get; set; }
        public int number { get; set; }
        public Version version { get; set; }
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
                new Thing("betty", "label1", 1, new Version("15.32.3")),
                new Thing("catherine", "label3", 2, new Version("2.2.3")),
                new Thing("abel", "label4", 3, new Version("1.23.3")),
                new Thing("dimitri", "label7", 4, new Version("1.22.3")),
                new Thing("eigil", "label99", 5, new Version("2.24.3")),
                new Thing("fritz", "label77", 6, new Version("1.27.3")),
                new Thing("gertrude", "label45", 7, new Version("1.2.4")),
                new Thing("", "label45", 8, null)
            };



            var sortedList1 = FilterHelper<Thing>.DynamicSort(thingList, SortOrder.Ascending, GetPropertyName((Thing t) => t.tlabel));
            var sortedList2 = FilterHelper<Thing>.DynamicSort(thingList, SortOrder.Ascending, GetPropertyName((Thing t) => t.tname));
            var sortedList3 = FilterHelper<Thing>.DynamicSort(thingList, SortOrder.Ascending, GetPropertyName((Thing t) => t.number));

            var sortedList4 = FilterHelper<Thing>.DynamicSort(thingList, SortOrder.Descending, GetPropertyName((Thing t) => t.tlabel));
            var sortedList5 = FilterHelper<Thing>.DynamicSort(thingList, SortOrder.Descending, GetPropertyName((Thing t) => t.tname));
            var sortedList6 = FilterHelper<Thing>.DynamicSort(thingList, SortOrder.Descending, GetPropertyName((Thing t) => t.number));


            var filteredList1 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilter.Contains, "bet" );
            var filteredList2 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilter.DoesNotContain, "bet");
            var filteredList3 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilter.DoesNotStartWith, "ab");
            var filteredList4 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilter.Is, "betty");
            var filteredList5 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilter.IsBlank, string.Empty);
            var filteredList6 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilter.IsNot, "fritz");
            var filteredList7 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilter.IsNotBlank, string.Empty);
            var filteredList8 = FilterHelper<Thing>.GenericStringFilter(thingList, GetPropertyName((Thing t) => t.tname), StringFilter.StartsWith, "cat");


            //List extension method doing the same thing but simpler syntax
            var filteredList121 = thingList.AdvancedFilter((t => t.tname), StringFilter.Contains, "bet");

            //With version filtering support
            var filteredList6666 = thingList.AdvancedFilter((t => t.version), VersionFilter.Is, thingList[0].version);
            var filteredList26676 = thingList.AdvancedFilter((t => t.version), VersionFilter.IsBlank, null);
            var filteredList66786 = thingList.AdvancedFilter((t => t.version), VersionFilter.IsEarlier, thingList[1].version);
            var filteredList66676 = thingList.AdvancedFilter((t => t.version), VersionFilter.IsEarlierMajorVersion, thingList[0].version);
            var filteredList666766 = thingList.AdvancedFilter((t => t.version), VersionFilter.IsLater, thingList[0].version);
            var filteredList66166 = thingList.AdvancedFilter((t => t.version), VersionFilter.IsLaterMajorVersion, thingList[0].version);
            var filteredList645666 = thingList.AdvancedFilter((t => t.version), VersionFilter.IsSameMajorVersion, new Version("1.2"));
            var filteredList664566 = thingList.AdvancedFilter((t => t.version), VersionFilter.IsNot, new Version("15.32.3"));
            var filteredList64666 = thingList.AdvancedFilter((t => t.version), VersionFilter.IsNotBlank, null);
            var filteredList66766 = thingList.AdvancedFilter((t => t.version), VersionFilter.IsSameOrEarlier, new Version("2.24.3"));
            var filteredList66966 = thingList.AdvancedFilter((t => t.version), VersionFilter.IsSameOrLater, new Version("2.24.3"));

        }
    }

       
       


}


