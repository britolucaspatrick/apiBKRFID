using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using apiBKRFID.Models;

namespace apiBKRFID.Controllers
{
    public class ProdutosController : Controller
    {
        private BKRFIDEntities db = new BKRFIDEntities();

        // GET: Produtos
        public async Task<ActionResult> Index()
        {
            return View(await db.Produto.ToListAsync());
        }

        // GET: Produtos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = await db.Produto.FindAsync(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: Produtos/Create
        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> GerarBlocoK()
        {
            List<DAO.TRegistro_SaldoEstoque> _SaldoEstoques = new List<DAO.TRegistro_SaldoEstoque>();
            db.Produto.ToList().ForEach(r => 
            {
                _SaldoEstoques.Add(new DAO.TRegistro_SaldoEstoque() 
                {
                    Cd_produto = r.CodBarras,
                    Quantidade = Convert.ToDecimal(r.Quantidade.ToString())
                });

                db.Produto.Remove(r);
            });

            if (_SaldoEstoques.Count > 0)
            {
                string arq = Business.SpedFiscal.ProcessarSpedFiscal(DateTime.Now,
                                                                 DateTime.Now,
                                                                 _SaldoEstoques);

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("c:\\blocoK.txt",
                                                                              false,
                                                                              System.Text.Encoding.Default))
                {
                    sw.Write(arq + "\r\n");
                    sw.Close();
                }
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: Produtos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ValueTag,CodBarras,Quantidade,ID_Produto")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Produto.Add(produto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(produto);
        }

        // GET: Produtos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = await db.Produto.FindAsync(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ValueTag,CodBarras,Quantidade,ID_Produto")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = await db.Produto.FindAsync(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Produto produto = await db.Produto.FindAsync(id);
            db.Produto.Remove(produto);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
