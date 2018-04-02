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
        private Genres _genres = null;
        private Configuration _tmdbConfiguration;
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

        private async Task GetConfigurationIfNeeded()
        {
            if (_tmdbConfiguration != null) return;

            try
            {
                var res = await BaseClient.GetAsync(string.Format(AppConstants.TmdbConfigurationUrl, AppConstants.TmdbApiKey));
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json)) throw new Exception("Return content was empty :(");

                _tmdbConfiguration = JsonConvert.DeserializeObject<Configuration>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(typeof(TMDBMovieService).Name,
                    "Ooops! Something went wrong fetching the configuration. Exception: {1}", ex);
            }
        }

        public async Task<List<DetailedBook>> SearchBooks(string BooksTitle)
        {
            try
            {
                /*var res = await BaseClient.GetAsync(string.Format(AppConstants.TmdbMovieSearchUrl, AppConstants.TmdbApiKey,
                    movieTitle))
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json)) return null;

                var movies = JsonConvert.DeserializeObject<Movies>(json);
                await GetConfigurationIfNeeded();

                var movieList = movies.results.Where(x => x.original_title != null).Select(movie => GetDetailMovie(movie)).ToList();

                return movieList;*/
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(typeof(TMDBMovieService).Name,
                    "Ooops! Something went wrong fetching information for: {0}. Exception: {1}", "", ex);
                return null;
            }
        }

        private DetailedMovie GetDetailMovie(TmdbMovie movie)
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

        public async Task<List<DetailedBook>> DiscoverBooks(DiscoverOption option)
        {
            try
            {
                var dscUrl = string.Empty;

                if (_genres == null) GetGenres();

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
                //var movies = JsonConvert.DeserializeObject<Movies>(json);
                await GetConfigurationIfNeeded();

                var bookList = books;

                return null;
                //return bookList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(typeof(TMDBMovieService).Name,
                                "Ooops! Something went wrong fetching information for: {0}. Exception: {1}", option.ToString(), ex);
                return null;
            }
        }


        public async Task<DetailedBook> DetailedBooksFromId(int id)
        {
            try
            {
                return null;
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
                Debug.WriteLine(typeof(TMDBMovieService).Name,
                    "Ooops! Something went wrong fetching information id: {0}. Exception: {1}", id, ex);
                return null;
            }
        }

        private string GetGenresString(string genresIDs)
        {
            var _gen = string.Empty;

            try
            {
                var ids = genresIDs?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (ids.Count() > 0)
                {
                    int i = 0;
                    foreach (var genreId in ids)
                    {
                        i++;

                        var g = _genres.genres.FirstOrDefault(x => x.id == Int32.Parse(genreId));

                        var genreName = (g != null) ? g.name : string.Empty;

                        if (!string.IsNullOrEmpty(genreName))
                        {
                            if (g != null)
                            {
                                _gen = _gen + genreName;
                            }

                            if (i != ids.Count())
                            {
                                _gen = _gen + ", ";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("ERROR: Getting genres by ids", ex);
                _gen = string.Empty;
            }

            return _gen;
        }

        private Genres GetGenres()
        {
            _genres = new Genres();
            var result = string.Empty;
            try
            {
                var s = Assembly.Load(new AssemblyName("eBooksDT.Core")).GetManifestResourceStream(@"eBooksDT.Core.Extras.MovieGenres.json");
                var sr = new StreamReader(s);
                result = sr.ReadToEnd();

                _genres = Newtonsoft.Json.JsonConvert.DeserializeObject<Genres>(result);
                return _genres;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("ERROR: Getting Genres", ex);
                return _genres;
            }
        }

      
    }

}
