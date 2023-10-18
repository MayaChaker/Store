using Store.Functions;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Store.Controllers
{
    public class UserController : ApiController
    {
        static List<User> users = new List<User>
        {

        };
        // GET: api/User
        public IHttpActionResult Get( )
        {
            var x = users.Select(y => new
            {
                y.Id,
                y.FirstName,
                y.LastName,
                y.Email,
                y.PhoneNumber,
            }).ToList();
            return Content(HttpStatusCode.OK , x);
        }

        // GET: api/User/5
        public IHttpActionResult Get(int id)
        {
            User user = users.Find(s => s.Id == id);
            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, "user  was not found");
            }
            return Content(HttpStatusCode.OK, user);
        }

        // POST: api/User
        public IHttpActionResult Post(User user)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest , "some fields are missing");
            }
            int id = 0;
            if (users.Count > 0)
            {
                users.ForEach(s =>
                {
                    if (s.Id >= id)
                    {
                        id = s.Id + 1;
                    }
                });
               

            }
            CryptoGraphy cryto = new CryptoGraphy();
            string token = cryto.generateJwtToken(user.Id.ToString(), user.Email);
            user.Token = token;
            users.Add(user);
            return Content(HttpStatusCode.Created, new
            {
                Token = token,
                Message = "created"
            });
        }

        // PUT: api/User/5
        public IHttpActionResult Put(int id, [FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "some fields are missing");
            }
            bool found = false;
            User updatedUser = new User();
            users.ForEach(s =>
            {
                if (s.Id == id)
                {
                    s.Id = user.Id;
                    s.FirstName = user.FirstName;
                    s.LastName = user.LastName;
                    s.Token = user.Token;
                    s.Email = user.Email;
                    s.PhoneNumber = user.PhoneNumber;

                    found = true;
                }
            });
            if (!found)
            {
                return Content(HttpStatusCode.NotFound, "user does not exist");
            }
            return Content(HttpStatusCode.OK, "user was updated succesfullt!");

        }

        // DELETE: api/User/5
        public IHttpActionResult Delete(int id)
        {

            User user = users.Find(s => s.Id == id);
            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, "user does not found");
            }
            users.Remove(user);
            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}
