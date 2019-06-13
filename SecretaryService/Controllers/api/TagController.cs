using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SecretaryService.Models;

namespace SecretaryService.Controllers.api
{
    public class TagController : ApiController
    {
        TagRepository tagRepository = new TagRepository();
        // GET: api/Tag
        public HttpResponseMessage Get()
        {
            List<Tag> tags = tagRepository.GetAllTags();
            return Request.CreateResponse(HttpStatusCode.OK, tags);
        }

        // GET: api/Tag/5
        public HttpResponseMessage Get(int id)
        {
            Tag tag = tagRepository.GetTag(id);
            return Request.CreateResponse(HttpStatusCode.OK, tag);
        }

        // POST: api/Tag
        public HttpResponseMessage Post([FromBody]Tag value)
        {
            if (String.IsNullOrWhiteSpace(value.Value))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Данные о тэге не введены!");
            if (tagRepository.InsertTag(value) != -1)
                return Request.CreateResponse(HttpStatusCode.OK);
            else
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Пользователь не добавлено!");
        }

        // PUT: api/Tag/5
        public HttpResponseMessage Put(int id, [FromBody]Tag value)
        {
            if (String.IsNullOrWhiteSpace(value.Value))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Данные о тэге не введены!");
            if (tagRepository.UpdateTag(value) == 1)
                return Request.CreateResponse(HttpStatusCode.OK);
            else
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Тэг не изменён!");
        }

        // DELETE: api/Tag/5
        public HttpResponseMessage Delete(int id)
        {
            tagRepository.DeleteTag(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
