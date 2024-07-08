using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TokenAuth.API.Data;

namespace TokenAuth.API.Controllers
{
    public class EmployeeController : ApiController
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        [Authorize(Roles = ("User"))]
        public HttpResponseMessage GetEmployeeById(int id)
        {
            var user = dbContext.Employees.FirstOrDefault(e => e.Id == id);
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [Authorize(Roles = ("Admin, SupperAdmin"))]
        [Route("api/Employee/GetSomeEmployees")]
        public HttpResponseMessage GetSomeEmployees()
        {
            var user = dbContext.Employees.Where(e => e.Id <= 10);
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [Authorize(Roles = ("SupperAdmin"))]
        [Route("api/Employee/GetEmployees")]
        public HttpResponseMessage GetEmployees()
        {
            var user = dbContext.Employees.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

    }
}
