using System;
using System.Collections.Generic;

namespace eBooksDT.Core.Constants
{
	internal class AppConstants
	{
		public const string TmdbApiKey = "c0d0458190688fc0796d41acf1cef8ec";
		public const string TmdbBaseUrl = "https://api.theBookdb.org/3/";
		public const string TmdbBookSearchUrl = "search/multi?api_key={0}&query={1}";
		public const string TmdbBookInTheaters = "discover/Book?api_key={0}&primary_release_date.gte={1}&primary_release_date.lte={2}";
		public const string TmdbBookPopular = "discover/Book?api_key={0}&sort_by=popularity.desc";
        public const string BooksPopular = "http://it-ebooks-api.info/v1/search/php mysql";
        public const string TmdbBookHighestRated = "discover/Book?api_key={0}&certification_country=US&certification=R&sort_by=vote_average.desc";
		public const string TmdbConfigurationUrl = "configuration?api_key={0}";
		public const string TmdbBookUrl = "Book/{1}?api_key={0}";
		public const string TmdbSimilarBooks = "Book/{0}/similar?api_key={1}&language=en-US&page=1";
		public const string TmdbBookCredits = "Book/{0}/credits?api_key={1}";
		public const string TmdbUpcomingBooks = "Book/upcoming?api_key={0}&language=en-US&page=1";
		public const int BookTitleMaxLength = 20;
 	}

	public enum DiscoverOption
	{
		InTheaters,
		Popular,
		HighestRated,
		Upcoming
	}
}
