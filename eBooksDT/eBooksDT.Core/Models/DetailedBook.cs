﻿using System;
using System.Collections.Generic;

namespace eBooksDT.Core.Models
{
	public class DetailedBook
    {
		public int Id { get; set; }
		public string OriginalTitle { get; set; }
		public string ComposedTitle { get; set; }
		public string ReleaseDate { get; set; }
		public double Score { get; set; }
		public int VoteCount { get; set; }
		public string ImdbId { get; set; }
		public List<Genre> Genres { get; set; }
		public string GenresCommaSeparated { get; set; }
		public string Overview { get; set; }
		public int Runtime { get; set; }
		public string Tagline { get; set; }
		public string PosterUrl { get; set; }
	}
}
