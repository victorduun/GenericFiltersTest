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

    public enum StringFilterType { Contains, DoesNotContain, StartsWith, DoesNotStartWith, Is, IsNot, IsBlank, IsNotBlank };
    public enum DateTimeFilterType { Is, IsNot, IsAfter, IsOnOrAfter, IsBefore, IsOnOrBefore, IsBlank, IsNotBlank };
    public enum DoubleFilterType { Is, IsNot, IsAfter, IsOnOrAfter, IsBefore, IsOnOrBefore, IsBlank, IsNotBlank };
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
        public static string GetString(this StringFilterType type)
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

        public static List<T> GenericStringFilter(List<T> listToFilter, string propertyToFilter, StringFilterType filter, string filterValue)
        {
           
            string query = "";
            switch (filter)
            {
                case StringFilterType.Contains:
                    query = String.Format("t=>t.{0}.Contains(\"{1}\")", propertyToFilter, filterValue);
                    break;
                case StringFilterType.DoesNotContain:
                    query = String.Format("t=>!t.{0}.Contains(\"{1}\")", propertyToFilter, filterValue);
                    break;
                case StringFilterType.DoesNotStartWith:
                    query = String.Format("t=>!t.{0}.StartsWith(\"{1}\")", propertyToFilter, filterValue);
                    break;
                case StringFilterType.Is:
                    query = String.Format("t=>t.{0} == \"{1}\"", propertyToFilter, filterValue);
                    break;
                case StringFilterType.IsBlank:
                    query = String.Format("t=>String.IsNullOrEmpty(t.{0})", propertyToFilter);
                    break;
                case StringFilterType.IsNot:
                    query = String.Format("t=>t.{0} != \"{1}\"", propertyToFilter, filterValue);
                    break;
                case StringFilterType.IsNotBlank:
                    query = String.Format("t=>!String.IsNullOrEmpty(t.{0})", propertyToFilter);
                    break;
                case StringFilterType.StartsWith:
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
