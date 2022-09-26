using ApiBase.Data;
using ApiBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace ApiBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IRepository _repository;
        private readonly IConfiguration _appsettings;

        public UsersController(IConfiguration appsettings,
                                AppDbContext context,
                                IRepository repository
                            )
        {

            _context = context;
            _repository = repository;
            _appsettings = appsettings;
        }

        //GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            //Respuesta<object> respuesta = new();
            //try
            //{
            //    var users = await _repository.SelectAll<User>();
            //    foreach (var item in users)
            //    {
            //        respuesta.Data.Add(item);
            //    }
            //    respuesta.Ok = 1;
            //    respuesta.Message = "Success";
            //}
            //catch (Exception e)
            //{
            //    respuesta.Ok = 0;
            //    respuesta.Message = e.Message + " " + e.InnerException;
            //}
            //return Ok(respuesta);
           var users =  await _repository.SelectAll<User>();
            return Ok(users);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            //Respuesta<object> respuesta = new();
            //try
            //{
            //    var user = await _context.user.Where(q => q.id == id).FirstOrDefaultAsync();
            //    if (user == null)
            //    {
            //        respuesta.Ok = 0;
            //        respuesta.Message = "User not found";

            //    }
            //    else
            //    {
            //        respuesta.Data.Add(new
            //        {
            //            user.firstname,
            //            user.surname
            //        });
            //        respuesta.Ok = 1;
            //        respuesta.Message = "Success";
            //    }
            //}
            //catch (Exception e)
            //{
            //    respuesta.Ok = 0;
            //    respuesta.Message = e.Message + " " + e.InnerException;
            //}
            //return Ok(respuesta);

            var user = await _context.Users.Where(q => q.Id == id).FirstOrDefaultAsync();
            //var advisor = await (from advisor in _context.Answers
            //                     where advisor.QuestionId == question.Id
            //                     select new
            //                     {
            //                         Id = answer.Id,
            //                         Name = answer.Name,
            //                         Image = answer.Image
            //                     }).ToListAsync();
            if (user != null)
            {
                return Ok(new
                {
                    user.Id,
                    Name = user.Firstname +" "+ user.Surname,                    
                    user.Created                   
                });
            }
            return NotFound();

        }
    }
}
