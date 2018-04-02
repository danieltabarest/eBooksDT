using eBooksDT.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eBooksDT.Helpers
{
    public class MovieTemplateSelector : Xamarin.Forms.DataTemplateSelector
    {
        readonly DataTemplate MovieCard;

        public MovieTemplateSelector()
        {
            this.MovieCard= new DataTemplate(typeof(MovieItem));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var movieData = item as eBooksDT.Core.Models.DetailedMovie;
            
            if (movieData== null)
                return null;
            DataTemplate selectedTemplate = MovieCard;
          
            return selectedTemplate;
        }
    }
}
