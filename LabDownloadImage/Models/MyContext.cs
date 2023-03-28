using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace LabDownloadImage.Models
{
    public class MyContext:DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<Image> Images { get; set; }

        public async Task AddImage(IFormFile file)
        {
            Images.Add(new Image()
            {
                Title = file.FileName,
                Path = $"img/{file.FileName}"
            });
            await SaveChangesAsync();
        }

        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
