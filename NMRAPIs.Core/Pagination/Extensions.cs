using Microsoft.EntityFrameworkCore;
using NMRAPIs.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NMRAPIs.Core.Pagination
{
    public static class Extensions
    {
        #region TypeExtensions
        public static bool IsNullableType(this Type propertyType)
        {
            return propertyType.IsGenericType
                && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsCollectionType(this Type propertyType)
        {
            return propertyType.IsGenericType
                && typeof(IEnumerable<>)
                    .MakeGenericType(propertyType.GetGenericArguments())
                    .IsAssignableFrom(propertyType);
        }
        #endregion

        #region StringExtensions
        public static bool IsEmpty(this string text)
        {
            return string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
        }
        #endregion

        #region IQueryableExtensions

        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string key, bool ascending = true)
        {
            // Validate the attribute. If it does not have a value return the query
            if (key.IsEmpty())
            {
                return query;
            }

            var lambda = CreateExpression(query, typeof(TSource), key, ascending);

            // Return the result
            return query.Provider.CreateQuery<TSource>(lambda);
        }

        public static Task<PagedEntity<T>> WithPagingAsync<T>(
            this IQueryable<T> query,
            PaginationRequest request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return query.WithPagingAsync(
                request.PageIndex.GetValueOrDefault(1),
                request.PageSize.GetValueOrDefault(10),
                request.Filters,
                request.OrderBy,
                request.OrderByAscending,
                cancellationToken);
        }
        public static async Task<PagedEntity<T>> WithPagingAsync<T>(
            this IQueryable<T> query,
            int pageNum,
            int pageSize,
            ICollection<ParametrizedFilter> filters = null,
            string orderBy = null,
            bool ascending = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            query = await query.WithParameterizedFilterAsync(filters);

            var rowsCount = await query.CountAsync(cancellationToken);

            if (rowsCount <= pageSize || pageNum <= 0)
            {
                pageNum = 1;
            }

            var excludedRows = (pageNum - 1) * pageSize;

            return new PagedEntity<T>
            {
                Results = await query.OrderBy(orderBy, ascending).Skip(excludedRows).Take(pageSize).ToListAsync(cancellationToken),
                TotalCount = rowsCount
            };
        }

        private static Expression CreateExpression(this IQueryable query, Type type, string propertyName, bool ascending = true)
        {
            var currentType = type;

            foreach (var part in propertyName.Split('.'))
            {
                var prop = currentType.GetProperty(part, BindingFlags.Instance | BindingFlags.Public);
                if (prop == null)
                {
                    return query.Expression;
                }

                currentType = prop.PropertyType;
            }

            var expression = query.Expression;
            var parameter = Expression.Parameter(type, "x");

            var selector = propertyName.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);

            var orderByName = ascending ? nameof(Queryable.OrderBy) : nameof(Queryable.OrderByDescending);

            expression = Expression.Call(
                typeof(Queryable),
                orderByName,
                new[] { query.ElementType, selector.Type },
                expression,
                Expression.Quote(Expression.Lambda(selector, parameter)));

            return expression;
        }

        public static async Task<IQueryable<TSource>> WithParameterizedFilterAsync<TSource>(this IQueryable<TSource> query, ICollection<ParametrizedFilter> filters)
        {
            try
            {
                if (filters != null && filters.Count > 0)
                {
                    foreach (var filter in filters)
                    {
                        query = await query.WithFilterAsync(filter.Filters);
                    }
                }

                return query;

            } catch (Exception ex)
            {
                return await Task.FromException<IQueryable<TSource>>(ex);
            }
        }

        public static async Task<IQueryable<TSource>> WithFilterAsync<TSource>(this IQueryable<TSource> query, ICollection<Filter> filters)
        {
            try
            {
                if (filters != null && filters.Count() > 0)
                {
                    Expression filterExpression = null;
                    var parameter = Expression.Parameter(typeof(TSource), "x");
                    foreach (var filter in filters)
                    {
                        Expression expression = GetFilterExpression(null, parameter, null, filter.RelationalOperator, filter.Value, filter.Property.Split('.'));
                        filterExpression = GetLogicalExpression(filter.LogicalOperator, filterExpression, expression);
                    }
                    var predicate = Expression.Lambda<Func<TSource, bool>>(filterExpression, parameter);
                    return query.Where(predicate);
                }
                else
                {
                    return query;
                }
            }
            catch (Exception ex)
            {
                return await Task.FromException<IQueryable<TSource>>(new ApiException((ex)));
            }
        }

        /// <summary>
        /// Build report pager object.
        /// </summary>
        /// <typeparam name="T"> Report model.</typeparam>
        /// <param name="model">Generic model.</param>
        /// <param name="paginationRequest">Pagination request.</param>
        /// <returns> Returns ReportsPagerModel.</returns>
        public static bool HasMoreRecords<T>(this List<T> model, PaginationRequest paginationRequest)
        {
            bool retVal = false;

            if (model != null && model.Any() && paginationRequest.PageSize.GetValueOrDefault() > 0)
            {
                if (paginationRequest.PageIndex.GetValueOrDefault() == 0)
                {
                    paginationRequest.PageIndex = 1;
                }
                
                if (model.Count() > (paginationRequest.PageIndex * paginationRequest.PageSize))
                    retVal = true;
            }

            return retVal;
        }

        /// <summary>
        /// Sort and paginate report model.
        /// </summary>
        /// <typeparam name="T"> Generic model class T. </typeparam>
        /// <typeparam name="TC"> Generic model class C. </typeparam>
        /// <param name="model"> Model.</param>
        /// <param name="paginationRequest"> Pagination request.</param>
        /// <param name="func"> Function pointer.</param>
        /// <returns> Returns the sorted and page sized model.</returns>
        public static async Task<List<T>> SortAndPaginationReportAsync<T>(this List<T> model, PaginationRequest paginationRequest)
        {
            if (model != null && model.Any())
            {
                if (paginationRequest.OrderBy.ToLower() == "monthyear")
                {
                    paginationRequest.OrderBy = "MonthYearIndex";
                }

                // Check if the columns is default columns of the report.
                var checkIfColumnsExists = model.Select(x => x.GetType().GetProperty(paginationRequest.OrderBy)).FirstOrDefault();

                // Sorting Region.
                if (checkIfColumnsExists != null)
                {
                    model = model.OrderBy(a => a.GetType().GetProperty(paginationRequest.OrderBy).GetValue(a, null)).ToList();
                }
                else
                {
                    throw new ApiException(
                                      $"Invalid columns to sort.");
                }

                if (!paginationRequest.OrderByAscending)
                {
                    model.Reverse();
                }

                if (paginationRequest.PageIndex.GetValueOrDefault() == 0)
                {
                    paginationRequest.PageIndex = 1;
                }

                // Pagination Region.
                int skippedRows = (paginationRequest.PageIndex.GetValueOrDefault() - 1) * paginationRequest.PageSize.GetValueOrDefault();

                if (skippedRows > model.Count())
                {
                    throw new ApiException(
                      $"Skipped records are more then total records.");
                }

                if (paginationRequest.PageSize.GetValueOrDefault() > 0)
                {
                    model = model.Skip(skippedRows).Take(paginationRequest.PageSize.GetValueOrDefault()).ToList();
                }
                else
                {
                    model = model.Skip(skippedRows).ToList();
                } 
            }

            return await Task.FromResult(model);
        }


        #endregion

        #region FilterHelpers

        private static Expression Combine(Expression first, Expression second)
        {
            if (first == null)
            {
                return second;
            }
            else
            {
                return Expression.AndAlso(first, second);
            }
        }

        private static Expression ApplySearchExpressionToCollection(ParameterExpression parameter, Expression property, Expression searchExpression)
        {
            if (searchExpression != null)
            {
                var asQueryable = typeof(Queryable).GetMethods()
                    .Where(m => m.Name == "AsQueryable")
                    .Single(m => m.IsGenericMethod)
                    .MakeGenericMethod(property.Type.GetGenericArguments());

                var anyMethod = typeof(Queryable).GetMethods()
                    .Where(m => m.Name == "Any")
                    .Single(m => m.GetParameters().Length == 2)
                    .MakeGenericMethod(property.Type.GetGenericArguments());

                searchExpression = Expression.Call(
                    null,
                    anyMethod,
                    Expression.Call(null, asQueryable, property),
                    Expression.Lambda(searchExpression, parameter));
            }

            return searchExpression;
        }

        private static Expression BuildFilterExpression(Expression property, RelationalOperator relationalOperator, string searchTerm)
        {
            Expression searchExpression = null;
            switch (relationalOperator)
            {
                case RelationalOperator.Equal:
                case RelationalOperator.NotEqual:
                    {
                        Expression lhsExpression = property;
                        ConstantExpression constExpression = null;
                        if (DateTime.TryParse(searchTerm, out DateTime dateResult))
                        {
                            constExpression = Expression.Constant(dateResult);
                        }
                        else
                        {
                            lhsExpression = Expression.Call(
                                Expression.Call(property, "ToString", new Type[0]),
                                typeof(string).GetMethod("ToLower", new Type[0]));
                            constExpression = Expression.Constant(searchTerm.ToLower());
                        }

                        if(relationalOperator == RelationalOperator.Equal)
                        {
                            searchExpression = Expression.Equal(lhsExpression, constExpression);
                        }
                        else
                        {
                            searchExpression = Expression.NotEqual(lhsExpression, constExpression);

                        }
                        break;
                    }
                case RelationalOperator.Like:
                    {
                        MethodCallExpression lhsToStringLowerExpression = Expression.Call(
                            Expression.Call(property, "ToString", new Type[0]),
                            typeof(string).GetMethod("ToLower", new Type[0]));
                        MethodInfo containsInfo = typeof(String).GetMethod("Contains", new Type[] { typeof(String) });
                        searchExpression = Expression.Call(lhsToStringLowerExpression, containsInfo, Expression.Constant(searchTerm.ToLower()));
                        break;
                    }
                case RelationalOperator.Less:
                case RelationalOperator.LessOrEqual:
                case RelationalOperator.GreaterOrEqual:
                case RelationalOperator.Greater:
                    {
                        ConstantExpression rhsExpression = null;
                        bool retVal = Int32.TryParse(searchTerm, out int intResult);
                        if(retVal)
                        {
                            rhsExpression = Expression.Constant(intResult);
                        }
                        else
                        {
                            retVal = DateTime.TryParse(searchTerm, out DateTime dateresult);
                            if(retVal)
                            {
                                rhsExpression = Expression.Constant(dateresult);
                            }
                        }

                        if (rhsExpression != null)
                        {
                            if(relationalOperator == RelationalOperator.Less)
                                searchExpression = Expression.LessThan(property, rhsExpression);
                            else if (relationalOperator == RelationalOperator.LessOrEqual)
                                searchExpression = Expression.LessThanOrEqual(property, rhsExpression);
                            else if (relationalOperator == RelationalOperator.GreaterOrEqual)
                                searchExpression = Expression.GreaterThanOrEqual(property, rhsExpression);
                            else if (relationalOperator == RelationalOperator.Greater)
                                searchExpression = Expression.GreaterThan(property, rhsExpression);

                        }
                        else
                        {
                            throw new InvalidOperationException("Operation not supported. Check the Datatype of Property and/or Value.");
                        }
                        break;
                    }
                default:
                    throw new InvalidOperationException("Comparator not supported.");
            }

            return searchExpression;
        }

        private static Expression GetFilterExpression(
            Expression filterExpression,
            ParameterExpression parameter,
            Expression property,
            RelationalOperator relationalOperator,
            string searchTerm,
            string[] remainingPropertyParts)
        {
            // Start with the Root or with any Nested Expression.
            property = Expression.Property(property == null ? parameter : property, remainingPropertyParts[0]);
            if (remainingPropertyParts.Length == 1)
            {
                // != Null Check for the Reference/Nullable Types.
                if (!property.Type.IsValueType || property.Type.IsNullableType())
                {
                    var nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));
                    filterExpression = Combine(filterExpression, nullCheckExpression);
                }

                if (property.Type.IsNullableType())
                {
                    property = Expression.Property(property, "Value");
                }

                Expression searchExpression = null;
                if (property.Type.IsCollectionType())
                {
                    parameter = Expression.Parameter(property.Type.GetGenericArguments().First());

                    searchExpression = ApplySearchExpressionToCollection(
                        parameter,
                        property,
                        BuildFilterExpression(parameter, relationalOperator, searchTerm));
                }
                else
                {
                    searchExpression = BuildFilterExpression(property, relationalOperator, searchTerm);
                }
                if (searchExpression == null)
                {
                    return null;
                }
                else
                {
                    return Combine(filterExpression, searchExpression);
                }
            }
            else
            {
                var nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));
                filterExpression = Combine(filterExpression, nullCheckExpression);
                if (property.Type.IsCollectionType())
                {
                    parameter = Expression.Parameter(property.Type.GetGenericArguments().First());
                    Expression searchExpression = GetFilterExpression(
                        null,
                        parameter,
                        null,
                        relationalOperator,
                        searchTerm,
                        remainingPropertyParts.Skip(1).ToArray());

                    if (searchExpression == null)
                    {
                        return null;
                    }
                    else
                    {
                        searchExpression = ApplySearchExpressionToCollection(
                            parameter,
                            property,
                            searchExpression);

                        return Combine(filterExpression, searchExpression);
                    }
                }
                else
                {
                    return GetFilterExpression(filterExpression, parameter, property, relationalOperator, searchTerm, remainingPropertyParts.Skip(1).ToArray());
                }
            }
        }        

        static Expression GetLogicalExpression(LogicalOperator logicalOperator, Expression filterExpression, Expression expression)
        {
            switch (logicalOperator)
            {
                case LogicalOperator.And:
                    filterExpression = filterExpression == null ? expression : Expression.And(filterExpression, expression);
                    break;

                case LogicalOperator.Or:
                    filterExpression = filterExpression == null ? expression : Expression.Or(filterExpression, expression);
                    break;
                case LogicalOperator.Not:
                    filterExpression = filterExpression == null ? expression : Expression.NotEqual(filterExpression, expression);
                    break;
                default:
                    filterExpression = filterExpression == null ? expression : Expression.And(filterExpression, expression);
                    break;
            }
            return filterExpression;
        }
        #endregion
    }
}
