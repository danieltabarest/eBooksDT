using eBooksDT.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eBooksDT.Helpers
{
    public class BookTemplateSelector : Xamarin.Forms.DataTemplateSelector
    {
        readonly DataTemplate BookCard;

        public BookTemplateSelector()
        {
            this.BookCard= new DataTemplate(typeof(BookItem));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var BookData = item as eBooksDT.Core.Models.DetailedBook;
            
            if (BookData== null)
                return null;
            DataTemplate selectedTemplate = BookCard;
          
            return selectedTemplate;
        }
    }
}
