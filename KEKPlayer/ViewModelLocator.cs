using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using KEKPlayer.ViewModels;
using KEKPlayer.Services;
using KEKPlayer.Models;

namespace KEKPlayer
{
    class ViewModelLocator
    {
        private static ServiceProvider _provaider;


        public static void Init()
        {
            var services = new ServiceCollection();

            //My ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<FirstOpenPageViewModel>();
            services.AddTransient<FunktionMenuKekPLayerPageViewModel>();
            services.AddTransient<TrackOnAirPageViewModel>();


            // My service
            services.AddSingleton<NavigationService>();
            services.AddSingleton<EventBus>();
            services.AddSingleton<MessageBus>();

            _provaider = services.BuildServiceProvider();

            foreach (var item in services)
            {
                _provaider.GetRequiredService(item.ServiceType);
            }
        }

        public MainViewModel MainViewModel => _provaider.GetRequiredService<MainViewModel>();

        public FirstOpenPageViewModel FirstOpenPageViewModel => _provaider.GetRequiredService<FirstOpenPageViewModel>();

        public FunktionMenuKekPLayerPageViewModel FunktionMenuKekPLayerPageViewModel => _provaider.GetRequiredService<FunktionMenuKekPLayerPageViewModel>();

        public TrackOnAirPageViewModel TrackOnAirPageViewModel => _provaider.GetRequiredService<TrackOnAirPageViewModel>();

    }
}
