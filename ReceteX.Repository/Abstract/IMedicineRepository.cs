using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Repository.Abstract
{        //xml için. silinebilir.

    public interface IMedicineRepository : IRepository<Medicine>
    {
        public Task ParseAndSaveMedicinesFromXml(string xmlContent);
    }
}
