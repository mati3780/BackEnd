using System.Collections.Generic;

namespace NetCore.Common.Domain.Model
{
	/// <summary>
	/// Represents a page of the entity being paged/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Page<T>
	{
		/// <summary>
		/// </summary>
		/// <param name="results"></param>
		/// <param name="offset"></param>
		/// <param name="limit"></param>
		/// <param name="count"></param>
		public Page(IEnumerable<T> results, int offset, int limit, long count)
		{
			Results = results;
			Pager = new Pager
			{
				Offset = offset,
				Limit = limit,
				Count = count
			};
		}

		/// <summary>
		/// Page metadata
		/// </summary>
		public Pager Pager { get; }
		/// <summary>
		/// Paged results
		/// </summary>
		public IEnumerable<T> Results { get; }
	}
	
	/// <summary>
	/// Pagination metadata
	/// </summary>
	public class Pager
	{
		/// <summary>
		/// Record from where to start paging
		/// </summary>
		public int Offset { get; set; }

		/// <summary>
		/// Amount of items to page
		/// </summary>
		public int Limit { get; set; }

		/// <summary>
		/// Total amount of items
		/// </summary>
		public long Count { get; set; }
	}
}