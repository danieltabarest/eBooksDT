using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using ModernHttpClient;
using eBooksDT.Core.Constants;
using eBooksDT.Core.Helpers;
using eBooksDT.Core.Interfaces;
using eBooksDT.Core.Models;
using Newtonsoft.Json;

namespace eBooksDT.Core
{
    public class BooksService : IBooksService
    {
        Books bs = new Books();
        private HttpClient _baseClient;

        private HttpClient BaseClient
        {
            get
            {
                return _baseClient ?? (_baseClient = new HttpClient(new NativeMessageHandler())
                {
                    BaseAddress = new Uri(AppConstants.TmdbBaseUrl)
                });
            }
        }

      
        public async Task<List<Books>> SearchBooks(string BooksTitle)
        {
            try
            {
                var res = await BaseClient.GetAsync(string.Format(AppConstants.TmdbBookSearchUrl, BooksTitle));
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json)) return null;

                var books = JsonConvert.DeserializeObject<Book>(json);

                return books.Books;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(typeof(BooksService).Name,
                    "Ooops! Something went wrong fetching information for: {0}. Exception: {1}", "", ex);
                return null;
            }
        }

       /* private DetailedMovie GetDetailMovie(TmdbMovie movie)
        {
            
            return new DetailedMovie
            {
                Id = movie.id,
                OriginalTitle = movie.original_title,
                ComposedTitle = GetTitle(movie),
                Overview = movie.overview,
                Score = movie.vote_average,
                VoteCount = movie.vote_count,
                ImdbId = movie.imdb_id,
                PosterUrl = GetUrl(movie),
                GenresCommaSeparated = GetGenresString(String.Join(", ", movie.genre_ids)),
                ReleaseDate = movie.release_date,
                Runtime = movie.runtime,
                Tagline = movie.tagline
            };
        }

        private string GetUrl(TmdbMovie movie)
        {
            return _tmdbConfiguration.images.base_url +_tmdbConfiguration.images.poster_sizes[3] + movie.poster_path;
        }

        private string GetTitle(TmdbMovie movie)
        {
            return string.Format("{0}{1}({2})", movie.title.Substring(0, Math.Min(movie.title.Length, AppConstants.MovieTitleMaxLength)),movie.title.Length > AppConstants.MovieTitleMaxLength ? "..." : " ", string.IsNullOrEmpty(movie.release_date) ? "N/A" : movie.release_date.Substring(0, 4));
        }
        */
        public async Task<List<Books>> DiscoverBooks(DiscoverOption option)
        {
            try
            {
                var dscUrl = string.Empty;

                switch (option)
                {
                    case DiscoverOption.Popular:
                        dscUrl = string.Format(AppConstants.BooksPopular);
                        break;
                   /* case DiscoverOption.HighestRated:
                        dscUrl = string.Format(AppConstants.TmdbMovieHighestRated, AppConstants.TmdbApiKey);
                        break;
                    case DiscoverOption.InTheaters:
                        dscUrl = string.Format(AppConstants.TmdbMovieInTheaters, AppConstants.TmdbApiKey,
                                               DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd"),
                                               DateTime.Today.ToString("yyyy-MM-dd"));
                        break;
                    case DiscoverOption.Upcoming:
                        dscUrl = string.Format(AppConstants.TmdbUpcomingMovies, AppConstants.TmdbApiKey);
                        break;
                    default:
                        dscUrl = string.Format(AppConstants.TmdbMoviePopular, AppConstants.TmdbApiKey);
                        break;*/
                }

                var res = await BaseClient.GetAsync(dscUrl);
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json)) return null;

                var books= JsonConvert.DeserializeObject<Book>(json);

                return books.Books;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(typeof(BooksService).Name,
                                "Ooops! Something went wrong fetching information for: {0}. Exception: {1}", option.ToString(), ex);
                return null;
            }
        }


        public async Task<DetailedBook> DetailedBooksFromId(int id)
        {
            try
            {
                return null;

                var res = await BaseClient.GetAsync(string.Format(AppConstants.TmdbBookSearchUrl, id));
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json)) return null;

                var detailedbook = JsonConvert.DeserializeObject<DetailedBook>(json);

                //var movieList = detailedbook.results.Where(x => x.original_title != null).Select(movie => GetDetailMovie(movie)).ToList();

                //return movieList;
                /* var res = await BaseClient.GetAsync(string.Format(AppConstants.TmdbMovieUrl, AppConstants.TmdbApiKey,
                     id));
                 res.EnsureSuccessStatusCode();

                 var json = await res.Content.ReadAsStringAsync();

                 if (string.IsNullOrEmpty(json)) return null;

                 var movie = JsonConvert.DeserializeObject<TmdbMovie>(json);
                 await GetConfigurationIfNeeded();

                 var detailed = GetDetailMovie(movie);

                 return detailed;*/
            }
            catch (Exception ex)
            {
                Debug.WriteLine(typeof(BooksService).Name,
                    "Ooops! Something went wrong fetching information id: {0}. Exception: {1}", id, ex);
                return null;
            }
        }

       

       
      
    }

}
