using eBooksDT.Core;
using eBooksDT.Core.Interfaces;
using eBooksDT.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBooksDT.ViewModels
{
    public class BookListDetailPageViewModel : BaseViewModel
    {
        private readonly IBooksService _BookService;
        private Book _Book { get; set; }

        public BookListDetailPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService, IBooksService BookService)
            : base(pageDialogService, navigationService)
        {
            _BookService = BookService;
            Task.Run(GetBookInfo).ConfigureAwait(true);
        }

        private async Task GetBookInfo()
        {
            var Book = await _BookService.DetailedBooksFromId(_Book.BookId);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            _Book = (Book)parameters["Book"];
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            _Book = (Book)parameters["Book"];
        }
    }
}