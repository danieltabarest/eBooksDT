using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eBooksDT.Core.Constants;
using eBooksDT.Core.Models;

namespace eBooksDT.Core.Interfaces
{
	public interface IMovieService
	{
		Task<List<DetailedMovie>> SearchMovie(string movieTitle);
		Task<DetailedMovie> DetailedMovieFromId(int id);
		Task<List<DetailedMovie>> DiscoverMovie(DiscoverOption option);
	}
}
