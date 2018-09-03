using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.Data;
using TaskPlanner.Interfaces;
namespace TaskPlanner.CSFiles
{
    public class UnitWork:IDisposable
    {
          
        private ApplicationDbContext DbContext { set; get; }
        private bool disposed = false;

        public UnitWork(ApplicationDbContext context)
        {
            this.DbContext = context;
        }

        public ApplicationDbContext GetContext
        {
            get { return this.DbContext; }
        }

       
        public virtual int Save()
        {
           var result= this.DbContext.SaveChanges();
            return result;
        }

        public virtual async Task<int> SaveAsync()
        {
            var result= await this.DbContext.SaveChangesAsync();
            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                   DbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
