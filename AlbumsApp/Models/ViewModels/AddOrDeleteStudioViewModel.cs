using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsApp.Models.ViewModels
{
    public class AddOrDeleteStudioViewModel : Studio
    {
        public AddOrDeleteStudioViewModel()
        {
        }

        public AddOrDeleteStudioViewModel(Studio studio)
        {
            this.StudioId = studio.StudioId;
            this.Name = studio.Name;
            this.Url = studio.Url;
            this.Address = studio.Address;
            this.City = studio.City;
            this.ZipCode = studio.ZipCode;
        }

    }
}
