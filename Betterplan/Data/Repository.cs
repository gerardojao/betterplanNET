using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiBase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace ApiBase.Data
{
    public class Repository<TDbContext> : IRepository where TDbContext : DbContext
    {
        private readonly IConfiguration _appsettings;
        private readonly AppDbContext _context;
        private TDbContext _dbContext;

        public Repository(TDbContext context, IConfiguration appsettings, AppDbContext appcontext)
        {
            this._dbContext = context;
            this._appsettings = appsettings;
            this._context = appcontext;
        }
  


        public async Task<List<T>> SelectAll<T>() where T : class
        {
            return await this._dbContext
                    .Set<T>()
                    .ToListAsync();
        }

        public async Task<T> SelectById<T>(int Id) where T : class
        {
            return await this._dbContext
                    .Set<T>()
                    .FindAsync(Id);
        }   

    }
}