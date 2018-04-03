using System;
using SQLite.Net.Attributes;

namespace eBooksDT.Models
{
	public class Book
	{
		[PrimaryKey, AutoIncrement]
		public int id { get; set; }
		public int BookId { get; set; }
        public int ListId { get; set; }
		public string BookName { get; set; }
		public string PosterURL { get; set; }
		public string BookDescription { get; set; }
		public string BookRate { get; set; }
		public string BookActors { get; set; }
        public string Genres { get; set; }
        public bool ToWatch { get; set; } = false;
		public bool AlreadySeen { get; set; } = false;
		public DateTime DateAdded { get; set; }
	}
}