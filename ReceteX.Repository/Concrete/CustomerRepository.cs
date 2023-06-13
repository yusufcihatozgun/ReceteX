using Microsoft.EntityFrameworkCore;
using ReceteX.Data;
using ReceteX.Models;
using ReceteX.Repository.Abstract;
using ReceteX.Repository.Shared.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ReceteX.Repository.Concrete
{
    public class CustomerRepository:Repository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _db;

        public CustomerRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public IQueryable GetAllWithUserCount()
        {
			return _db.Customers.Where(c=>c.IsDeleted == false).GroupJoin(_db.Users.Where(u=>u.IsDeleted==false), c => c.Id, u => u.CustomerId, (customer, user) => new
			{
				Id = customer.Id,
				Name = customer.Name,
				TotalUsers = user.Count()
			});

		}
    }
}
