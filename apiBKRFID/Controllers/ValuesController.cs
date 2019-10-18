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
        private BKRFIDEntities1 db = new BKRFIDEntities1();
        
        // GET api/values/5
        public IEnumerable<Produto> Get(int id)
        {
            IEnumerable<Produto> asd = db.Produto.SqlQuery("select * from produto where valuetag = @id", new SqlParameter("@id", id));
            asd.ToList().ForEach(r =>
            {
                r.Quantidade = true;
                db.Produto.Add(r);
                db.SaveChangesAsync();
            });
            return asd;
        }

    }
}
