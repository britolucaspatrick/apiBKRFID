using apiBKRFID.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace apiBKRFID.Controllers
{
    public class ValuesController : ApiController
    {
        private BKRFIDEntities db = new BKRFIDEntities();

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public IEnumerable<Produto> Get(int id)
        {
            IEnumerable <Produto> asd = db.Produto.SqlQuery("select * from produto where valuetag = @id", new SqlParameter("@id", id));
            return asd;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
