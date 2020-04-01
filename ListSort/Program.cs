using System;
using System.Collections.Generic;

namespace ListSort
{

    public class Things
    {
        public Things(string name, int num)
        {
            tname = name;
            number = num;
        }
        public string tname { get; set; }
        public int number { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<Things> thingList = new List<Things>();
            thingList.Add(new Things("betty", 1));
            thingList.Add(new Things("catherine", 2));
            thingList.Add(new Things("abel", 3));

            
            var sortedList = FilterHelper<Things>.DynamicSort(thingList, SortOrder.Descending, "tname");

            var filteredList1 = FilterHelper<Things>.GenericFilter(sortedList, "tname", "bet", FilterType.Contains);
            var filteredList2 = FilterHelper<Things>.GenericFilter(sortedList, "tname", "bet", FilterType.DoesNotContain);
            var filteredList3 = FilterHelper<Things>.GenericFilter(sortedList, "tname", "ab", FilterType.DoesNotStartWith);
            var filteredList4 = FilterHelper<Things>.GenericFilter(sortedList, "tname", "betty", FilterType.Is);
            var filteredList5 = FilterHelper<Things>.GenericFilter(sortedList, "tname", "bet", FilterType.IsBlank);
            var filteredList6 = FilterHelper<Things>.GenericFilter(sortedList, "tname", "bet", FilterType.IsNot);
            var filteredList7 = FilterHelper<Things>.GenericFilter(sortedList, "tname", "", FilterType.IsNotBlank);
            var filteredList8 = FilterHelper<Things>.GenericFilter(sortedList, "tname", "cat", FilterType.StartsWith);


        }
    }

       
       


}


