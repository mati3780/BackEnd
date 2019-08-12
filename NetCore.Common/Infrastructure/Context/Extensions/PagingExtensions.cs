using DelegateDecompiler.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCore.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCore.Common.Infrastructure.Context.Extensions
{
	public static class PagingExtensions
	{
		public static async Task<Page<TResult>> ToPageAsync<T, TResult>(this IQueryable<T> query, int offset, int limit, Func<T, TResult> selector)
		{
			var totalItems = await query.CountAsync();
			var lista = (await query.Skip(offset).Take(limit).ToListAsync()).Select(selector);
			return new Page<TResult>(lista, offset, limit, totalItems);
		}

		public static async Task<Page<TResult>> ToPageComputedAsync<T, TResult>(this IQueryable<T> query, int offset, int limit, Expression<Func<T, TResult>> selector)
		{
			var totalItems = await query.CountAsync();
			var lista = await query.Skip(offset).Take(limit).Select(selector).DecompileAsync().ToListAsync();
			return new Page<TResult>(lista, offset, limit, totalItems);
		}

		public static async Task<Page<TResult>> ToPageAsync<T, TKey, TResult>(this IQueryable<T> query, int offset, int limit, Func<T, TResult> selector, Expression<Func<T, TKey>> orderBy)
		{
			var count = await query.CountAsync();
			if (orderBy != null)
				query = query.OrderBy(orderBy);
			var lista = (await query.Skip(offset).Take(limit).ToListAsync()).Select(selector);
			return new Page<TResult>(lista, offset, limit, count);
		}

		public static async Task<Page<TResult>> ToPageAsync<T, TKey, TSec, TResult>(this IQueryable<T> query, int offset, int limit, Func<T, TResult> selector,
			Expression<Func<T, TKey>> orderBy, Expression<Func<T, TSec>> thenBy)
		{
			var totalItems = await query.CountAsync();
			if (orderBy != null)
				query = thenBy != null? query.OrderBy(orderBy).ThenBy(thenBy) : query.OrderBy(orderBy);
			var lista = (await query.Skip(offset).Take(limit).ToListAsync()).Select(selector);
			return new Page<TResult>(lista, offset, limit, totalItems);
		}

		public static Page<TResult> ToPage<T, TKey, TSec, TResult>(this IQueryable<T> query, int offset, int limit, Func<T, TResult> selector,
			Expression<Func<T, TKey>> orderBy, Expression<Func<T, TSec>> thenBy)
		{
			Page<TResult> page = null;
			Task.Run(async () => page = await ToPageAsync(query, offset, limit, selector, orderBy, thenBy)).Wait();
			return page;
		}

		public static Page<TResult> ToPage<T, TResult>(this IQueryable<T> query, int offset, int limit, Func<T, TResult> selector)
		{
			Page<TResult> page = null;
			Task.Run(async () => page = await ToPageAsync(query, offset, limit, selector)).Wait();
			return page;
		}

		public static Page<TResult> ToPage<T, TKey, TResult>(this IQueryable<T> query, int offset, int limit, Func<T, TResult> selector, Expression<Func<T, TKey>> orderBy)
		{
			Page<TResult> page = null;
			Task.Run(async () => page = await ToPageAsync(query, offset, limit, selector, orderBy)).Wait();
			return page;
		}

		public static Page<TResult> ToPage<T, TResult>(this IEnumerable<T> enumerable, int offset, int limit, Func<T, TResult> selector)
		{
			var list = enumerable as IList<T> ?? enumerable.ToList();
			var count = list.Count;
			var result = list.Skip(offset).Take(limit).Select(selector);
			return new Page<TResult>(result, offset, limit, count);
		}
	}
}