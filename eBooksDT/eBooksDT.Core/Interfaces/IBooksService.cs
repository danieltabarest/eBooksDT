using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eBooksDT.Core.Constants;
using eBooksDT.Core.Models;

namespace eBooksDT.Core.Interfaces
{
	public interface IBooksService
    {
		Task<List<Books>> SearchBooks(string BooksTitle);
		Task<DetailedBook> DetailedBooksFromId(int id);
		Task<List<Books>> DiscoverBooks(DiscoverOption option);
	}
}
