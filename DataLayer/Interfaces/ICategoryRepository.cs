using Menu_QRCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface ICategoryRepository
    {
        public List<Category> GetAll();
        public Category GetById(Guid id);
        public Category GetByName(string name);
        public bool InsertCategory(Category category);
        public bool UpdateCategory(Category category);
        public bool DeleteCategory(Category category);
        public bool DeleteCategory(Guid id);
        void Save();
    }
}
