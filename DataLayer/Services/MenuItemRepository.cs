using DataLayer.Interfaces;
using Menu_QRCode;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Services
{
    public class MenuItemRepository : IMenuItemRepository
    {
        Context _context;
        public MenuItemRepository(Context context)
        {
            this._context = context;
        }
        public bool DeleteMenuItem(MenuItem menuItem)
        {
            try
            {
                _context.Entry(menuItem).State = EntityState.Deleted;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteMenuItem(Guid id)
        {
            try
            {
                DeleteMenuItem(_context.MenuItems.Find(id));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<MenuItem> GetAll()
        {
            return _context.MenuItems.Include(o=>o.Category).ToList();
        }

        public MenuItem GetById(Guid id)
        {
            return _context.MenuItems.Find(id);
        }

        public MenuItem GetByName(string name)
        {
            return _context.MenuItems.Find(name);
        }

        public bool InsertMenuItem(MenuItem menuItem)
        {
            try
            {
                _context.MenuItems.Add(menuItem);
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

        public bool UpdateMenuItem(MenuItem menuItem)
        {
            try
            {
                var existingMenuItem = _context.MenuItems.FirstOrDefault(x => x.Id == menuItem.Id);
                if (existingMenuItem != null)
                {
                    _context.Entry(existingMenuItem).State = EntityState.Detached;
                }
                _context.Entry(menuItem).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
