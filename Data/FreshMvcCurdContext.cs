using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FreshMvcCurd.Models;

namespace FreshMvcCurd.Data
{
    public class FreshMvcCurdContext : DbContext
    {
        public FreshMvcCurdContext (DbContextOptions<FreshMvcCurdContext> options)
            : base(options)
        {
        }

        public DbSet<FreshMvcCurd.Models.BookViewModel> BookViewModel { get; set; } = default!;
    }
}
