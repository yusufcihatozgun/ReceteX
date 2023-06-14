using ReceteX.Data;
using ReceteX.Models;
using ReceteX.Repository.Abstract;
using ReceteX.Repository.Shared.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Repository.Concrete
{
    public class MedicineRepository : Repository<Medicine>, IMedicineRepository
    {
        private readonly ApplicationDbContext _db;
        public MedicineRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void ClearRange(List<Medicine> medicines)
        {
            _db.Medicines.RemoveRange(medicines);
        }
    }
}
