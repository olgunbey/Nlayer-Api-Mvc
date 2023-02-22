using Nlayer.Core.UnitofWorks;
using Nlayer.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Repository.UnitOfWorks
{
    public class UnitOfWorks : IUnitOfWork
    {

        private readonly AppDbContext appDbContext;
        public UnitOfWorks(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void Commit()
        {
            appDbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
           await appDbContext.SaveChangesAsync();
        }
    }
}
