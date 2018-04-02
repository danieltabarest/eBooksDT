using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace eBooksDT.Core.Models
{
   /* [DataContract]
    public class Book
    {
        [DataMember]
        public string ID;
        [DataMember]
        public string URL;
        [DataMember]
        public string Title;
        [DataMember]
        public string Description;
        [DataMember]
        public string Publisher;
        [DataMember]
        public string Author;
        [DataMember]
        public string ISBN;
        [DataMember]
        public string DatePublished;
        [DataMember]
        public string NumberOfPages;
        [DataMember]
        public string Language;
        [DataMember]
        public string Format;
        [DataMember]
        public string DownloadLink;
        [DataMember]
        public string FileName;
        [DataMember]
        public string SavePath;
        [DataMember]
        public bool Downloaded;
    }*/


    public class Book
    {
        public string Error { get; set; }
        public float Time { get; set; }
        public string Total { get; set; }
        public int Page { get; set; }
        public List<Books> Books { get; set; }
    }

    public class Books
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string isbn { get; set; }
        public string SubTitle { get; set; }
    }

}
