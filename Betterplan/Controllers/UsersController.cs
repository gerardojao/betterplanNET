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
            return NotFound();

        }

        [HttpGet("{id}/goal")]

        public async Task<ActionResult> GetGoalByUserId(int id)
        {
           try
            {
                var goals = await _context.Goals.Where(q => q.Id == id).FirstOrDefaultAsync();
                if (goals != null)
                {
                    var goalPortfolio = (from goal in _context.Goals
                                         join port in _context.Portfolios on goal.Portfolioid equals port.Id
                                         join ent in _context.Financialentities on port.Financialentityid equals ent.Id
                                         where goals.Id == id
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
                return NotFound();

            }
           
        }


    }
}
