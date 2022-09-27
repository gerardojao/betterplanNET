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
           
           var users =  await _repository.SelectAll<User>();
            return Ok(users);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {     
            var user = await _context.Users.Where(q => q.Id == id).FirstOrDefaultAsync();
    
            if (user != null)
            {
                return Ok(new
                {
                    user.Id,
                    Name = user.Firstname +" "+ user.Surname,   
                    user.Advisorid,
                    user.Created                   
                });
            }
            return NotFound("Este ID no se encuentra en la Base de Datos");
        }

        [HttpGet("{userId}/goal")]
        public async Task<ActionResult> GetGoalByUserId(int userId)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var goals = await (from goal in _context.Goals
                                   join u in _context.Users on goal.Userid equals u.Id
                                   join port in _context.Portfolios on goal.Portfolioid equals port.Id
                                   join ent in _context.Financialentities on port.Financialentityid equals ent.Id
                                   where goal.Userid == userId
                                   select new
                                   {
                                       goal.Title,
                                       goal.Years,
                                       goal.Initialinvestment,
                                       goal.Monthlycontribution,
                                       goal.Targetamount,
                                       Portafolio = port.Title,
                                       Entity = ent.Title,
                                       goal.Created
                                   }).ToListAsync();

                return Ok(goals);
               
            }

            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }


        [HttpGet("{id}/goal/{goalId}")]

        public async Task<ActionResult> GetGoalIdByUserId(int id, int goalId)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var goals = await _context.Goals.Where(q => q.Id == goalId).FirstOrDefaultAsync();
                if (goals != null)
                {
                    var goalPortfolio = (from goal in _context.Goals
                                         join port in _context.Portfolios on goal.Portfolioid equals port.Id
                                         join ent in _context.Financialentities on port.Financialentityid equals ent.Id
                                         join gtf in _context.Goaltransactionfundings on goal.Id equals gtf.Goalid
                                         where goals.Userid == id
                                         select new
                                         {
                                             goal.Title,
                                             goal.Years,
                                             goal.Initialinvestment,
                                             goal.Monthlycontribution,
                                             goal.Targetamount,
                                             Portfolio = port.Title,
                                             Entity = ent.Title,
                                             goal.Created,
                                             Percentage = gtf.Percentage
                                         }).ToList();

                    foreach (var gp in goalPortfolio)
                    {
                        return Ok(gp);
                    }

                }

                return NotFound();
            }

            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }



    }
}
