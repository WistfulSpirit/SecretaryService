using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SecretaryService.Models;

namespace SecretaryService.Controllers.api
{
    public class PersonController : ApiController
    {
        private PersonRepository personRepository = new PersonRepository();
        // GET: api/Person
        public HttpResponseMessage Get()
        {
            List<Person> persons = personRepository.GetAllPersons();
            return Request.CreateResponse(HttpStatusCode.OK, persons);
        }
        // GET: api/Person/5
        public HttpResponseMessage Get(int id)
        {
            Person person = personRepository.GetPerson(id);
            return Request.CreateResponse(HttpStatusCode.OK, person);
        }

        // POST: api/Person
        public HttpResponseMessage Post([FromBody]Person value)
        {
            if(String.IsNullOrWhiteSpace(value.Email) || String.IsNullOrWhiteSpace(value.Name))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Данные о пользователе не введены!");
            if (personRepository.InsertPerson(value) != -1)
                return Request.CreateResponse(HttpStatusCode.OK);
            else
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Пользователь не добавлено!");
        }

        // PUT: api/Person/5
        public HttpResponseMessage Put(int id, [FromBody]Person value)
        {
            if (String.IsNullOrWhiteSpace(value.Email) || String.IsNullOrWhiteSpace(value.Name))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Данные о пользователе не введены!");
            if (personRepository.UpdatePerson(value) == 1)
                return Request.CreateResponse(HttpStatusCode.OK);
            else
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Пользователь не изменён!");
        }

        // DELETE: api/Person/5
        public HttpResponseMessage Delete(int id)
        {
            if (personRepository.IsSafeToDelete(id))
            {
                personRepository.DeletePerson(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            return Request.CreateResponse(HttpStatusCode.Conflict, "Невозможно удалить!");
        }
    }
}
