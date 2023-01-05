using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AlbumsApp.Models;

namespace AlbumsApp.Components
{
    public class TopRatedAlbums : ViewComponent
    {
        public TopRatedAlbums(AlbumsDbContext albumDbContext)
        {
            _albumDbContext = albumDbContext;
        }

        public IViewComponentResult Invoke()
        {
            var albums = _albumDbContext.Albums.Where(a => a.Rating >= 8.0).OrderByDescending(a => a.Rating).ToList();

            var viewModel = new TopRatedAlbumsViewModel() { 
                Albums = albums, 
                NumberOfAlbumsToDisplay = Math.Min(albums.Count, 3)
            };

            return View(viewModel);
        }

        private AlbumsDbContext _albumDbContext;
    }
}
