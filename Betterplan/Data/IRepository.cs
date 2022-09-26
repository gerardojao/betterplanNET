using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApiBase.Data
{
    public interface IRepository 
    {
        Task<List<T>> SelectAll<T>() where T : class; 
        Task<T> SelectById<T>(int Id) where T : class;
      
      

    }
}