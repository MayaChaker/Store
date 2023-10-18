using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Store.Controllers
{
    public class StoreController : ApiController
    {
        static List<Item> items = new List<Item>
        {
            new Item{ Id = 1, Name = "Pant" , price = 20 , quantity= 5},
            new Item{ Id = 2, Name = "T-shirt" , price = 10 , quantity= 10},
            new Item{ Id = 3, Name = "Hoodies" , price = 35 , quantity= 15},
        };


        // GET: Store
        public IHttpActionResult Get([FromUri] string Name = "")
        {
            Item item = items.Find(s => s.Name.Contains(Name));
            if (item == null)
            {
                return Content(HttpStatusCode.NotFound, " no items wuth tihs name was found!");
            }
            return Content(HttpStatusCode.OK, items);
        }


        public IHttpActionResult Get(int id)
        {
            Item item = items.Find(s => s.Id == id);
            if (item == null)
            {
                return Content(HttpStatusCode.NotFound, "item was not found");
            }
            return Content(HttpStatusCode.OK, item);
        }

        public IHttpActionResult Post(Item item)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "all fieds are required!");
            }
            int id = 0;
            if (items.Count > 0)
            {
                items.ForEach(s =>
                {
                    if (s.Id >= id)
                    {
                        id = s.Id + 1;
                    }
                });

            }
            item.Id = id;
            items.Add(item);
            return Content(HttpStatusCode.Created, "item was created succesfully!");
        }

        public IHttpActionResult Put(int id, [FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "All fields are required!");
            }
            bool found = false;
            Item updatedItem = new Item();
            items.ForEach(s =>
            {
                if (s.Id == id)
                {
                    s.Id = item.Id;
                    s.Name = item.Name;
                    s.price = item.price;
                    s.quantity = item.quantity;

                    found = true;
                }
            });
            if (!found)
            {
                return Content(HttpStatusCode.NotFound, "item does not exist");
            }
            return Content(HttpStatusCode.OK, "item was updated succesfullt!");
        }

        public IHttpActionResult Delete(int id)
        {
            Item item = items.Find(s => s.Id == id);
            if (item == null)
            {
                return Content(HttpStatusCode.NotFound, "item does not found");
            }
            items.Remove(item);
            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}

