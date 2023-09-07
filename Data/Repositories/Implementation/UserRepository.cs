
using ProjectLab.Data.Repositories.Interface;
using ProjectLab.Models.Entities;

namespace ProjectLab.Data.Repositories.Implementation
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ApplicationDBContext context)
           : base(context)
        {
        }
    }
}
