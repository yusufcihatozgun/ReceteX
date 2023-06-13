﻿using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Repository.Abstract
{
    public interface ICustomerRepository : IRepository<Customer>
	{
		public IQueryable GetAllWithUserCount();
	}
}
