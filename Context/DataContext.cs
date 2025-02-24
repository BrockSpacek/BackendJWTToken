using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using BackendJWTToken.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendJWTToken.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options){

        }

        public DbSet<UserModel> Users {get; set;}
    }
}