using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using apiBKRFID.Models;

namespace apiBKRFID.ControllersApi
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProdutoController : ApiController
    {
        private BKRFIDEntities db = new BKRFIDEntities();

        // GET: api/Produto
        public IQueryable<Produto> GetProduto()
        {
            return db.Produto;
        }

        // GET: api/Produto/5
        [ResponseType(typeof(Produto))]
        public async Task<IHttpActionResult> GetProduto(int id)
        {
            Produto produto = await db.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }
        
        // PUT: api/Produto/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduto(int id, Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != produto.ID_Produto)
            {
                return BadRequest();
            }

            db.Entry(produto).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Produto
        [ResponseType(typeof(Produto))]
        public async Task<IHttpActionResult> PostProduto(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Produto.Add(produto);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = produto.ID_Produto }, produto);
        }

        // DELETE: api/Produto/5
        [ResponseType(typeof(Produto))]
        public async Task<IHttpActionResult> DeleteProduto(int id)
        {
            Produto produto = await db.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            db.Produto.Remove(produto);
            await db.SaveChangesAsync();

            return Ok(produto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProdutoExists(int id)
        {
            return db.Produto.Count(e => e.ID_Produto == id) > 0;
        }
    }
}