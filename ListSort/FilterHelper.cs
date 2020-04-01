using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ListSort
{
    public enum SortOrder { Ascending, Descending };

    public enum FilterType { Contains, DoesNotContain, StartsWith, DoesNotStartWith, Is, IsNot, IsBlank, IsNotBlank };
    public static class EnumExtensionMethods
    {
        public static string GetString(this SortOrder order)
        {
            return order switch
            {
                SortOrder.Ascending => "asc",
                SortOrder.Descending => "desc",
                _ => "error",
            };
        }
        public static string GetString(this FilterType type)
        {
            return type switch
            {

                _ => "test",
            };
        }

    }


    public class FilterHelper<T>
    {



        public static List<T> DynamicSort(List<T> listToSort, SortOrder sortOrder, string attributeToSortBy)
        {
            return listToSort.AsQueryable().OrderBy(attributeToSortBy + " " + sortOrder.GetString()).ToList();

        }

        public static List<T> GenericFilter(List<T> listToFilter, string propertyToFilter, string filterValue, FilterType filter)
        {
           
            string query = "";
            switch (filter)
            {
                case FilterType.Contains:
                    query = String.Format("t=>t.{0}.Contains(\"{1}\")", propertyToFilter, filterValue);
                    break;
                case FilterType.DoesNotContain:
                    query = String.Format("t=>!t.{0}.Contains(\"{1}\")", propertyToFilter, filterValue);
                    break;
                case FilterType.DoesNotStartWith:
                    query = String.Format("t=>!t.{0}.StartsWith(\"{1}\")", propertyToFilter, filterValue);
                    break;
                case FilterType.Is:
                    query = String.Format("t=>t.{0} == \"{1}\"", propertyToFilter, filterValue);
                    break;
                case FilterType.IsBlank:
                    query = String.Format("t=>String.IsNullOrEmpty(t.{0})", propertyToFilter);
                    break;
                case FilterType.IsNot:
                    query = String.Format("t=>t.{0} != \"{1}\"", propertyToFilter, filterValue);
                    break;
                case FilterType.IsNotBlank:
                    query = String.Format("t=>!String.IsNullOrEmpty(t.{0})", propertyToFilter);
                    break;
                case FilterType.StartsWith:
                    query = String.Format("t=>t.{0}.StartsWith(\"{1}\")", propertyToFilter, filterValue);
                    break;
                default:
                    query = String.Format("t=>!String.IsNullOrEmpty(t.{0})", propertyToFilter); // Default to is not blank
                    break;
            }
            return listToFilter.AsQueryable().Where(query).ToList();
        }


    }
}
