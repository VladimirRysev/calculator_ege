using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator_ege_bu.Models.Accaunt
{
    public class UsersListViewModel
    {
        public ICollection<UserDto> Users { get; set; }

        internal async Task Init(ApplicationDbContext dbContext, ApplicationUser current)
        {
            if (current == null)
            {
                current = new ApplicationUser();
            }
            Users = await dbContext.Users
                .Select(u=> new UserDto
                {
                    Id = u.Id,
                    Name = u.UserName,
                    CanBeDeleted = current.Id != u.Id && current.Id != 0 && u.Id != 1
                })
                .ToListAsync();
        }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanBeDeleted { get; set; }
    }
}
