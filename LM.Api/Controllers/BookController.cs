using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using LM.Api.Models;
using LM.Api.Models.DB;
using LM.Api.Models.VM;
using Microsoft.Owin;
using PagedList;

namespace LM.Api.Controllers
{
    [Authorize]
    public class BookController : ApiController
    {
        private const int PAGE_SIZE = 20;
        private readonly LibraryDbContext _db = new LibraryDbContext();

        [HttpGet]
        [ActionName("list")]
        [AllowAnonymous]
        public IHttpActionResult GetList(int page = 1, string search = "", string column = BookPropertyKeys.ID, string sort = SortTypes.ASC)
        {
            var query = _db.Books
                .Include(i => i.Authors);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(w => w.Title.Contains(search) ||
                                         w.Publisher.Contains(search) ||
                                         w.ISBN.Contains(search) ||
                                         w.Authors.Any(a => a.FirstName.Contains(search) || a.LastName.Contains(search)));

            IPagedList<Book> pager;

            switch (column.ToLowerInvariant())
            {
                default:
                    pager = (sort == SortTypes.DESC) ? query.OrderByDescending(o => o.Id).ToPagedList(page, PAGE_SIZE) : query.OrderBy(o => o.Id).ToPagedList(page, PAGE_SIZE);
                    break;
                case BookPropertyKeys.TITLE:
                    pager = (sort == SortTypes.DESC) ? query.OrderByDescending(o => o.Title).ToPagedList(page, PAGE_SIZE) : query.OrderBy(o => o.Title).ToPagedList(page, PAGE_SIZE);
                    break;
                case BookPropertyKeys.YEAR:
                    pager = (sort == SortTypes.DESC) ? query.OrderByDescending(o => o.Year).ToPagedList(page, PAGE_SIZE) : query.OrderBy(o => o.Year).ToPagedList(page, PAGE_SIZE);
                    break;
            }

            var model = new BookListViewModel
            {
                Items = pager,
                Pager = pager.GetMetaData(),
                Column = column,
                Sort = sort,
                Search = search
            };

            return Ok(model);
        }

        [HttpPost]
        [ActionName("save")]
        public async Task<IHttpActionResult> Save(BookInlineEditModel model)
        {
            var book = await _db.Books.Include(i => i.Authors).FirstOrDefaultAsync(f => f.Id == model.Id);

            if (book == null)
                return NotFound();

            if (model.AuthorId == null)
            {
                switch (model.Column.ToLowerInvariant())
                {
                    case BookPropertyKeys.TITLE:
                        book.Title = model.Data;
                        break;

                    case BookPropertyKeys.YEAR:
                        book.Year = Convert.ToInt32(model.Data);
                        break;

                    case BookPropertyKeys.PUBLISHER:
                        book.Publisher = model.Data;
                        break;

                    case BookPropertyKeys.ISBN:
                        book.ISBN = model.Data;
                        break;

                    case BookPropertyKeys.COUNT_OF_PAGE:
                        book.CountOfPage = Convert.ToInt32(model.Data);
                        break;
                }
            }
            else
            {
                var author = book.Authors.FirstOrDefault(f => f.Id == model.AuthorId);

                if (author == null)
                    return NotFound();

                switch (model.Column.ToLowerInvariant())
                {
                    case BookPropertyKeys.FIRST_NAME:
                        author.FirstName = model.Data;
                        break;

                    case BookPropertyKeys.LAST_NAME:
                        author.LastName = model.Data;
                        break;
                }
                 
            }

            _db.Entry(book).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [ActionName("details")]
        public async Task<IHttpActionResult> GetDetails(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = await _db.Books.Include(i => i.Authors).FirstOrDefaultAsync(f => f.Id == id);

            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        [ActionName("add")]
        public async Task<IHttpActionResult> Post(Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Books.Add(book);
            _db.Entry(book).State = EntityState.Added;

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [ActionName("edit")]
        public async Task<IHttpActionResult> Put(Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ent = await _db.Books.Include(i => i.Authors).FirstOrDefaultAsync(f => f.Id == book.Id);

            if (ent == null)
                return NotFound();

            ent.CountOfPage = book.CountOfPage;
            ent.ISBN = book.ISBN;
            ent.PicturePath = book.PicturePath;
            ent.Publisher = book.Publisher;
            ent.Year = book.Year;
            ent.Title = ent.Title;

            _db.Books.Attach(ent);
            _db.Entry(ent).State = EntityState.Modified;

            for (var i = 0; i < book.Authors.Count; i++)
            {
                var item = book.Authors.ElementAt(i);

                if (item.Id == 0)
                {
                    var author = new Author
                    {
                        BookId = book.Id,
                        LastName = item.LastName,
                        FirstName = item.LastName
                    };
                    _db.Authors.Add(author);
                    _db.Entry(author).State = EntityState.Added;
                }
                else
                {
                    var author = ent.Authors.FirstOrDefault(f => f.Id == item.Id);

                    author.FirstName = book.Authors.ElementAt(i).FirstName;
                    author.LastName = book.Authors.ElementAt(i).LastName;

                    _db.Authors.Attach(author);
                    _db.Entry(author).State = EntityState.Modified;
                }
            }

            var authors = ent.Authors.Where(w => book.Authors.Any(a => a.Id != w.Id)).ToArray();
            foreach (var item in authors)
            {
                var author = ent.Authors.FirstOrDefault(f => f.Id == item.Id);

                _db.Authors.Remove(author);
                _db.Entry(author).State = EntityState.Deleted;
            }

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [ActionName("remove")]
        public async Task<IHttpActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = await _db.Books.FindAsync(id);

            if (book == null)
                return NotFound();

            book.IsDeleted = true;
            _db.Entry(book).State = EntityState.Modified;

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [ActionName("remove-picture")]
        public async Task<IHttpActionResult> DeletePicture(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = await _db.Books.FindAsync(id);

            if (book == null)
                return NotFound();

            try
            {
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/"), book.PicturePath);

                if (File.Exists(path))
                    File.Delete(path);

                book.PicturePath = string.Empty;
                _db.Entry(book).State = EntityState.Modified;

                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok();
        }

        [HttpDelete]
        [ActionName("remove-author")]
        public async Task<IHttpActionResult> DeleteAuthor(int? id)
        {
            if (id == null)
                return BadRequest();

            var author = await _db.Authors.FindAsync(id);

            if (author == null)
                return NotFound();

            author.IsDeleted = true;
            _db.Entry(author).State = EntityState.Modified;

            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}