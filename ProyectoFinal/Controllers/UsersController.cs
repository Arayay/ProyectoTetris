using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using ProyectoFinal.Data;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoFinal.Controllers
{
    public class UsersController : Controller
    {
        private ProyectoFinalDbEntities db = new ProyectoFinalDbEntities();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Users users = await db.Users.FindAsync(id);
            if (users == null)
            {
                return HttpNotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,Username,PasswordHash,Email,IsActive,Role")] Users users)
        {
            if (ModelState.IsValid)
            {
                users.PasswordHash = GetSha256(users.PasswordHash);
                users.MaxScore = 0;
                users.TotalGames = 0;
                users.CreatedAt = DateTime.Now;

                db.Users.Add(users);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Users users = await db.Users.FindAsync(id);
            if (users == null)
            {
                return HttpNotFound();
            }

            return View(users);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,Username,PasswordHash,Email,IsActive,Role")] Users users)
        {
            if (ModelState.IsValid)
            {
                Users userDb = await db.Users.FindAsync(users.UserId);

                if (userDb == null)
                {
                    return HttpNotFound();
                }

                userDb.Username = users.Username;
                userDb.Email = users.Email;
                userDb.IsActive = users.IsActive;
                userDb.Role = users.Role;

                if (!string.IsNullOrWhiteSpace(users.PasswordHash))
                {
                    userDb.PasswordHash = GetSha256(users.PasswordHash);
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Users users = await db.Users.FindAsync(id);
            if (users == null)
            {
                return HttpNotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Users users = await db.Users.FindAsync(id);

            if (users == null)
            {
                return HttpNotFound();
            }

            db.Users.Remove(users);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private string GetSha256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }

                return builder.ToString();
            }
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
