using LinqKit;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NetCore.Common.Infrastructure.Context.Extensions
{
	public static class FilterExtensions
	{
		public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> source, IEnumerable<KeyValuePair<string, StringValues>> queryString, Dictionary<string, Expression<Func<T, object>>> filterMap,
			bool defaultCondition = true, bool allConditions = true)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source), "Data source is empty");

			//convierto a Dictionary con Keys en Lowercase para facilidad de busqueda
			var filterMapLower = filterMap.ToDictionary(x => x.Key.ToLower(), x => x.Value);
			//convierto la lista de campos para filtrar en una lista en Lowercase y filtro los que no existen y los que tienen valores nulos
			var filterList = queryString.ToDictionary(q => q.Key.ToLower(), q => q.Value.FirstOrDefault()?.ToLower()).Where(x => filterMapLower.ContainsKey(x.Key.ToLower()) && !string.IsNullOrWhiteSpace(x.Value));

			var predicate = PredicateBuilder.New<T>(defaultCondition); //Genero un PredicateBuilder (True para que devuelva todos por defecto)
			foreach(var filter in filterList) //y mientras recorro la lista de filtros válidos para usar
			{
				var expr = GetOperationExpression(filterMapLower[filter.Key], filter.Value); //genero la expresión equivalente a ese querystring con el mapeo correspondiente a la DB
				predicate = allConditions ? predicate.And(expr) : predicate.Or(expr); //y agrego al predicado ya generado
			}

			return source.Where(predicate).AsExpandable();
		}

		public static IEnumerable<T> ApplyFilter<T>(this IEnumerable<T> source, IEnumerable<KeyValuePair<string, string>> queryString, Dictionary<string, Expression<Func<T, object>>> filterMap,
			bool defaultCondition = true, bool allConditions = true)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source), "Data source is empty");

			//convierto a Dictionary con Keys en Lowercase para facilidad de busqueda
			var filterMapLower = filterMap.ToDictionary(x => x.Key.ToLower(), x => x.Value);
			//convierto la lista de campos para filtrar en una lista en Lowercase y filtro los que no existen y los que tienen valores nulos
			var filterList = queryString.Where(x => filterMapLower.ContainsKey(x.Key.ToLower()) && !string.IsNullOrWhiteSpace(x.Value))
				.ToDictionary(x => x.Key.ToLower(), x => x.Value.ToLower());
			var predicate = PredicateBuilder.New<T>(defaultCondition); //Genero un PredicateBuilder (True para que devuelva todos por defecto)
			foreach(var filter in filterList) //y mientras recorro la lista de filtros válidos para usar
			{
				var expr = GetOperationExpression(filterMapLower[filter.Key], filter.Value, true); //genero la expresión equivalente a ese querystring con el mapeo correspondiente a la DB
				predicate = allConditions? predicate.And(expr) : predicate.Or(expr); //y agrego al predicado ya generado
			}

			return source.Where(predicate);
		}

		private static Expression<Func<T, bool>> GetOperationExpression<T>(Expression<Func<T, object>> fieldMap, object value, bool iEnumerable = false)
		{
			//Obtengo el Type de la expresión y basado en el mismo elijo el método para la operación en la DB dependiendo de si es un String o cualquier otro Type
			var bodyExpression = fieldMap.Body.NodeType == ExpressionType.Convert? ((UnaryExpression)fieldMap.Body).Operand : fieldMap.Body;
			var type = bodyExpression.Type;

			//Armo la llamada al método usando la expresión seleccionada, el método y el valor a buscar. Si es un string y está trabajando con un IEnumerable, primero le aplico ToLower
			MethodCallExpression methodCall;
			if (type == typeof(string))
			{
				if (iEnumerable)
				{
					var toLowerCall = Expression.Call(bodyExpression, type.GetMethod("ToLower", new Type[0]));
					methodCall = Expression.Call(toLowerCall, type.GetMethod("Contains", new[] {type}), Expression.Constant(Convert.ChangeType(value, type)));
				}
				else
				{
					methodCall = Expression.Call(bodyExpression, type.GetMethod("Contains", new[] {type}), Expression.Constant(Convert.ChangeType(value, type)));
				}
			}
			else
			{
				methodCall = Expression.Call(bodyExpression, type.GetMethod("Equals", new[] {type}), Expression.Constant(Convert.ChangeType(value, type)));
			}

			//Armo y devuelvo la Expresión Lambda que representa la operación a realizar
			return Expression.Lambda<Func<T, bool>>(methodCall, fieldMap.Parameters);
		}
	}
}