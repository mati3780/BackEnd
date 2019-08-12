using DelegateDecompiler.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCore.Common.Infrastructure.Context.Extensions
{
	public static class SelectMapExtensions
	{
		public static async Task<TDto> SingleOrDefaultMapAsync<T, TDto>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector)
		{
			return await source.Where(predicate).Select(selector).DecompileAsync().SingleOrDefaultAsync();
		}

		public static async Task<TDto> SingleOrDefaultMapAsync<T, TDto>(this IQueryable<T> source, Expression<Func<T, TDto>> selector)
		{
			return await source.Select(selector).DecompileAsync().SingleOrDefaultAsync();
		}

		public static async Task<TDto> FirstOrDefaultMapAsync<T, TDto>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector)
		{
			return await source.Where(predicate).Select(selector).DecompileAsync().FirstOrDefaultAsync();
		}

		public static async Task<TDto> FirstOrDefaultMapAsync<T, TDto>(this IQueryable<T> source, Expression<Func<T, TDto>> selector)
		{
			return await source.Select(selector).DecompileAsync().FirstOrDefaultAsync();
		}

		public static async Task<List<TDto>> ToListMapAsync<T, TDto>(this IQueryable<T> source, Expression<Func<T, TDto>> selector)
		{
			return await source.Select(selector).DecompileAsync().ToListAsync();
		}
	}
}