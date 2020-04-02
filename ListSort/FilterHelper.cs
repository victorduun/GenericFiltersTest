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

    public enum StringFilter { Contains, DoesNotContain, StartsWith, DoesNotStartWith, Is, IsNot, IsBlank, IsNotBlank };
    public enum DateTimeFilterType { Is, IsNot, IsAfter, IsOnOrAfter, IsBefore, IsOnOrBefore, IsBlank, IsNotBlank };
    public enum NumberFilterType { IsLessThan, IsLessThanOrEqualTo, IsGreaterThan, IsGreaterThanOrEqualTo, Is, IsNot, IsBlank, IsNotBlank };

    public enum VersionFilter { Is, IsNot, IsEarlier, IsLater, IsSameMajorVersion, IsEarlierMajorVersion, IsLaterMajorVersion, IsSameOrLater, IsSameOrEarlier, IsBlank, IsNotBlank };
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
        public static string GetString(this StringFilter type)
        {
            return type switch
            {

                _ => "test",
            };
        }

    }

    public static class ListFilterExtensions
    {
        public static IEnumerable<TModel> AdvancedFilter<TModel, TProperty>(
            this IEnumerable<TModel> enumerable, 
            Expression<Func<TModel, TProperty>> propertyToFilter, 
            StringFilter filter, 
            string filterValue)
        {
            MemberExpression memberExpression = (MemberExpression)propertyToFilter.Body;
            string memberName = memberExpression.Member.Name;


            string query = filter switch
            {
                StringFilter.Contains => String.Format("t=>t.{0}.Contains(\"{1}\")", memberName, filterValue),
                StringFilter.DoesNotContain => String.Format("t=>!t.{0}.Contains(\"{1}\")", memberName, filterValue),
                StringFilter.DoesNotStartWith => String.Format("t=>!t.{0}.StartsWith(\"{1}\")", memberName, filterValue),
                StringFilter.Is => String.Format("t=>t.{0} == \"{1}\"", memberName, filterValue),
                StringFilter.IsBlank => String.Format("t=>String.IsNullOrEmpty(t.{0})", memberName),
                StringFilter.IsNot => String.Format("t=>t.{0} != \"{1}\"", memberName, filterValue),
                StringFilter.IsNotBlank => String.Format("t=>!String.IsNullOrEmpty(t.{0})", memberName),
                StringFilter.StartsWith => String.Format("t=>t.{0}.StartsWith(\"{1}\")", memberName, filterValue),
                _ => String.Format("t=>!String.IsNullOrEmpty(t.{0})", memberName), // Default to is not blank
            };
            return enumerable.AsQueryable().Where(query).ToList();
        }

        public static IEnumerable<TModel> AdvancedFilter<TModel, TProperty>(
            this IEnumerable<TModel> enumerable,
            Expression<Func<TModel, TProperty>> propertyToFilter,
            VersionFilter filter,
            Version filterValue)
        {
            MemberExpression memberExpression = (MemberExpression)propertyToFilter.Body;
            string memberName = memberExpression.Member.Name;

            //Sort through null/not null first to prevent later errors
            IEnumerable<TModel> nullVersions = enumerable.AsQueryable().Where($"t=>t.{memberName} == null");
            IEnumerable<TModel> validVersions = enumerable.AsQueryable().Where($"t=>t.{memberName} != null");

            return filter switch
            {
                VersionFilter.Is => validVersions.AsQueryable().Where($"t=>t.{memberName} = @0", filterValue),
                VersionFilter.IsBlank => nullVersions,
                VersionFilter.IsEarlier => validVersions.AsQueryable().Where($"t=>t.{memberName} < @0", filterValue),
                VersionFilter.IsEarlierMajorVersion => validVersions.AsQueryable().Where($"t=>t.{memberName}.Major < @0", filterValue.Major),
                VersionFilter.IsLater => validVersions.AsQueryable().Where($"t=>t.{memberName}.Major > @0", filterValue.Major),
                VersionFilter.IsLaterMajorVersion => validVersions.AsQueryable().Where($"t=>t.{memberName}.Major > @0", filterValue.Major),
                VersionFilter.IsSameMajorVersion => validVersions.AsQueryable().Where($"t=>t.{memberName}.Major = @0", filterValue.Major),
                VersionFilter.IsNot => validVersions.AsQueryable().Where($"t=>t.{memberName} != @0", filterValue),
                VersionFilter.IsNotBlank => validVersions,
                VersionFilter.IsSameOrEarlier => validVersions.AsQueryable().Where($"t=>t.{memberName} <= @0", filterValue),
                VersionFilter.IsSameOrLater => validVersions.AsQueryable().Where($"t=>t.{memberName} >= @0", filterValue),
                _ => validVersions, // Default to is not blank
            };


        }
    }

    public class FilterHelper<T>
    {



        public static List<T> DynamicSort(List<T> listToSort, SortOrder sortOrder, string attributeToSortBy)
        {
            return listToSort.AsQueryable().OrderBy(attributeToSortBy + " " + sortOrder.GetString()).ToList();

        }


        public static List<T> GenericStringFilter(List<T> listToFilter, string propertyToFilter, StringFilter filter, string filterValue)
        {
           
            string query = "";
            switch (filter)
            {
                case StringFilter.Contains:
                    query = String.Format("t=>t.{0}.Contains(\"{1}\")", propertyToFilter, filterValue);
                    break;
                case StringFilter.DoesNotContain:
                    query = String.Format("t=>!t.{0}.Contains(\"{1}\")", propertyToFilter, filterValue);
                    break;
                case StringFilter.DoesNotStartWith:
                    query = String.Format("t=>!t.{0}.StartsWith(\"{1}\")", propertyToFilter, filterValue);
                    break;
                case StringFilter.Is:
                    query = String.Format("t=>t.{0} == \"{1}\"", propertyToFilter, filterValue);
                    break;
                case StringFilter.IsBlank:
                    query = String.Format("t=>String.IsNullOrEmpty(t.{0})", propertyToFilter);
                    break;
                case StringFilter.IsNot:
                    query = String.Format("t=>t.{0} != \"{1}\"", propertyToFilter, filterValue);
                    break;
                case StringFilter.IsNotBlank:
                    query = String.Format("t=>!String.IsNullOrEmpty(t.{0})", propertyToFilter);
                    break;
                case StringFilter.StartsWith:
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
