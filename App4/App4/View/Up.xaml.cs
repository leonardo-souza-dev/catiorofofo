﻿using App4.Model;
using App4.Model.Resposta;
using App4.Repository;
using App4.ViewModel;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Up : ContentPage
    {
        private PostViewModel PostViewModel;
        private Stream Stream = null;
        private MediaFile File;
        private TabbedPage MainPage;

        public ObservableCollection<PostModel> posts { get; set; }

        public string Arquivo = string.Empty;

        public Up(PostViewModel postViewModel, TabbedPage mainPage)
        {
            InitializeComponent();

            this.MainPage = mainPage;
            this.PostViewModel = postViewModel;
            this.Title = "enviar catioro fofo";
            CrossMedia.Current.Initialize();

            UploadViewModel uploadViewModel = new UploadViewModel();
            uploadViewModel.UploadModel = new UploadModel(){ PostarButtonIsEnabled = false };
            this.BindingContext = uploadViewModel.UploadModel;
        }

        public async void EscolherFoto()
        {
            var cameraNaoDisponivel = !CrossMedia.Current.IsCameraAvailable;
            var escolherFotoNaoSuportado = !CrossMedia.Current.IsPickPhotoSupported;

            if (cameraNaoDisponivel || escolherFotoNaoSuportado)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                // return;
            }

            File = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
            {
                CompressionQuality = 50
            });

            if (File == null)
                return;

            PostarButton.IsEnabled = true;
            PostarButton.IsVisible = true;
            AcharButton.Text = "trocar catioro fofo";

            FotoImage.Source = ImageSource.FromStream(ObterStream);
        }

        protected async void PostarButtonClicked(object o, EventArgs args)
        {
            var resposta = await PostViewModel.Salvar(Stream,
                                                      LegendaEntry.Text,     
                                                      PostViewModel.Usuario);

            if (resposta == RespostaStatus.ErroGenerico)
            {
                await DisplayAlert("erro", "ocorreu um erro","cancelar");
                return;
            }

            var expViewCode = new ExpViewCS(PostViewModel);
            MainPage.CurrentPage = MainPage.Children[0];
        }

        private Stream ObterStream()
        {
            Stream stream = File.GetStream();
            Stream = File.GetStream();
            File.Dispose();

            return stream;
        }
    }
}
