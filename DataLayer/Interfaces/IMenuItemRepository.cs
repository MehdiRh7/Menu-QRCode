using Menu_QRCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IMenuItemRepository
    {
        public List<MenuItem> GetAll();
        public MenuItem GetById(Guid id);
        public MenuItem GetByName(string name);
        public bool InsertMenuItem(MenuItem menuItem);
        public bool UpdateMenuItem(MenuItem menuItem);
        public bool DeleteMenuItem(MenuItem menuItem);
        public bool DeleteMenuItem(Guid id);
        void Save();
    }
}
