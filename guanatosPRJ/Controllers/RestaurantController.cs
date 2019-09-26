using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using guanatosSvc.Models;
using guanatosSvc.Data;

namespace guanatosSvc.Controllers
{
    [Route("api/Restaurant")]
    public class RestaurantController : Controller
    {
        ConnectionsObj con = ConnectionsObj.GetInstance;

        // GET: api/Restaurant
        [HttpGet]
        public IEnumerable<String> Get()
        {
            con.CreateConnection();
            List<String> list = new List<String>();
            list = con.Search();

            return list;
        }

            // GET: api/Restaurant/5
        [HttpGet("{val}", Name = "Get")]
        public IEnumerable<Restaurant> Get(string val)
        {
            con.CreateConnection();
            List<Restaurant> list = new List<Restaurant>();

            return con.Search(val);
        }


        // POST: api/Restaurant
        [HttpPost]
        public void Post([FromBody] Restaurant restaurant)
        {
            con.CreateConnection();
            con.Save(restaurant);
        }

        // POST: api/Restaurant/Correo
        [Route("Correo")]
        [HttpPost]
        public void Post([FromBody] String correo)
        {
            con.CreateConnection();
            con.Save(correo);
        }
        // PUT: api/Restaurant/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]int value)
        {

        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
