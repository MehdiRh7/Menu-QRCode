using DataLayer.Interfaces;
using Menu_QRCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        Context _context;
        public CategoryRepository(Context context)
        {
            this._context = context;
        }
        public bool DeleteCategory(Category category)
        {
            try
            {
                _context.Entry(category).State = EntityState.Deleted;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCategory(Guid id)
        {
            try
            {
                DeleteCategory(_context.Categories.Find(id));
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public Category GetById(Guid id)
        {
            return _context.Categories.Find(id);
        }

        public Category GetByName(string name)
        {
            return _context.Categories.Find(name);
        }

        public bool InsertCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool UpdateCategory(Category category)
        {
            try
            {
                _context.Entry(category).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
