using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NetCore.Common.Infrastructure.Context.Extensions
{
	public static class SortingExtensions
	{
		public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string sortParam, string defaultSortParam, Dictionary<string, Expression<Func<T, object>>> sortMap)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source), "Data source is empty");

			if (string.IsNullOrWhiteSpace(defaultSortParam))
				throw new ArgumentNullException(nameof(defaultSortParam), "Default sort parameters are missing");

			//convierto a Dictionary con Keys en Lowercase para facilidad de busqueda
			var sortMapLower = sortMap.ToDictionary(x => x.Key.ToLower(), x => x.Value);
			//convierto la lista de campos para ordenar en un diccionario campo/sentido y filtro los que no existen
			var lstSort = ProcessSortParam(sortParam, sortMapLower);
			if (!lstSort.Any()) //Si no quedó ninguno, cargo los default
				lstSort = ProcessSortParam(defaultSortParam, sortMapLower);

			IQueryable<T> query = null;
			foreach(var sort in lstSort) //Recorro los campos y aplico su ordenamiento
				query = query == null
					? source.GetOrderedQuery(sortMapLower[sort.Key], sort.Value, true)
					: query.GetOrderedQuery(sortMapLower[sort.Key], sort.Value, false);
			return query;
		}

		private static IOrderedQueryable<T> GetOrderedQuery<T>(this IQueryable<T> source, Expression<Func<T, object>> expression, bool ascending, bool firstOrder)
		{
			var bodyExpression = (MemberExpression)(expression.Body.NodeType == ExpressionType.Convert? ((UnaryExpression)expression.Body).Operand : expression.Body);
			var sortLambda = Expression.Lambda(bodyExpression, expression.Parameters);
			Expression<Func<IOrderedQueryable<T>>> sortMethod;
			if (firstOrder)
			{
				if (ascending) sortMethod = () => source.OrderBy<T, object>(k => null);
				else sortMethod = () => source.OrderByDescending<T, object>(k => null);
			}
			else
			{
				if (ascending) sortMethod = () => ((IOrderedQueryable<T>)source).ThenBy<T, object>(k => null);
				else sortMethod = () => ((IOrderedQueryable<T>)source).ThenByDescending<T, object>(k => null);
			}

			var methodCallExpression = (MethodCallExpression)sortMethod.Body;
			var method = methodCallExpression.Method.GetGenericMethodDefinition();
			var genericSortMethod = method.MakeGenericMethod(typeof(T), bodyExpression.Type);
			var orderedQuery = (IOrderedQueryable<T>)genericSortMethod.Invoke(source, new object[] {source, sortLambda});
			return orderedQuery;
		}

		public static IEnumerable<T> ApplySort<T>(this IEnumerable<T> source, string sortParam, string defaultSortParam, Dictionary<string, Expression<Func<T, object>>> sortMap)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source), "Data source is empty");

			if (string.IsNullOrWhiteSpace(defaultSortParam))
				throw new ArgumentNullException(nameof(defaultSortParam), "Default sort parameters are missing");

			//convierto a Dictionary con Keys en Lowercase para facilidad de busqueda
			var sortMapLower = sortMap.ToDictionary(x => x.Key.ToLower(), x => x.Value);
			//convierto la lista de campos para ordenar en un diccionario campo/sentido y filtro los que no existen
			var lstSort = ProcessSortParam(sortParam, sortMapLower);
			if (!lstSort.Any()) //Si no quedó ninguno, cargo los default
				lstSort = ProcessSortParam(defaultSortParam, sortMapLower);

			IEnumerable<T> query = null;
			foreach(var sort in lstSort) //Recorro los campos y aplico su ordenamiento
				query = query == null
					? source.GetOrderedQuery(sortMapLower[sort.Key], sort.Value, true)
					: query.GetOrderedQuery(sortMapLower[sort.Key], sort.Value, false);
			return query;
		}

		private static IOrderedEnumerable<T> GetOrderedQuery<T>(this IEnumerable<T> source, Expression<Func<T, object>> expression, bool ascending, bool firstOrder)
		{
			var bodyExpression = (MemberExpression)(expression.Body.NodeType == ExpressionType.Convert? ((UnaryExpression)expression.Body).Operand : expression.Body);
			var sortLambda = Expression.Lambda(bodyExpression, expression.Parameters);
			Expression<Func<IOrderedEnumerable<T>>> sortMethod;
			if (firstOrder)
			{
				if (ascending) sortMethod = () => source.OrderBy<T, object>(k => null);
				else sortMethod = () => source.OrderByDescending<T, object>(k => null);
			}
			else
			{
				if (ascending) sortMethod = () => ((IOrderedEnumerable<T>)source).ThenBy<T, object>(k => null);
				else sortMethod = () => ((IOrderedEnumerable<T>)source).ThenByDescending<T, object>(k => null);
			}

			var methodCallExpression = sortMethod.Body as MethodCallExpression;
			if (methodCallExpression == null)
				throw new Exception("oops");

			var meth = methodCallExpression.Method.GetGenericMethodDefinition();
			var genericSortMethod = meth.MakeGenericMethod(typeof(T), bodyExpression.Type);
			var orderedQuery = (IOrderedEnumerable<T>)genericSortMethod.Invoke(source, new object[] {source, sortLambda.Compile()});
			return orderedQuery;
		}

		private static Dictionary<string, bool> ProcessSortParam<T>(string sortParam, Dictionary<string, Expression<Func<T, object>>> sortMap)
		{
			return sortParam.Split(',').ToDictionary(x => (x.StartsWith("-")? x.Remove(0, 1) : x).ToLower(),
					x => !x.StartsWith("-"))
				.Where(x => sortMap.ContainsKey(x.Key))
				.ToDictionary(x => x.Key, x => x.Value);
		}
	}
}