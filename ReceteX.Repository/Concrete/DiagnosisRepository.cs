using ReceteX.Data;
using ReceteX.Models;
using ReceteX.Repository.Abstract;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Repository.Shared.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Repository.Concrete
{
    public class DiagnosisRepository : Repository<Diagnosis>, IDiagnosisRepository
    {
        private readonly ApplicationDbContext _db;
        public DiagnosisRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void ClearRange(List<Diagnosis> diagnoses)
        {
            _db.Diagnoses.RemoveRange(diagnoses);
        }
    }
}
