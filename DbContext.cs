﻿
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using first.Models;


namespace first.Data

{
    public class ApplicationDbContext : DbContext
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }



        public DbSet<Staff> Staff { get; set; }

        public DbSet <student> Student { get; set; }
       

    }
}
