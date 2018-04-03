using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eBooksDT.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MostPopularPage : ContentPage
	{
		public MostPopularPage()
		{
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }
		}
	}
}
