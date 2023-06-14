using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Repository.Abstract
{
    public interface IDiagnosisRepository : IRepository<Diagnosis>
    {
        public void ClearRange(List<Diagnosis> diagnoses);

    }
}
