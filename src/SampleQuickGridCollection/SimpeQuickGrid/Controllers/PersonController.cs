using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using SimpeQuickGrid.Data;

namespace SimpeQuickGrid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ODataController
    {
        private readonly PeopleDbContext dBContext;

        public PersonController(PeopleDbContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Person>> Get()
        {
            return Ok(dBContext.People);
        }
    }
}
