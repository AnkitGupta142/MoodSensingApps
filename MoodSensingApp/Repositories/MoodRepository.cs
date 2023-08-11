using Microsoft.EntityFrameworkCore;
using MoodSensingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoodSensingApp.Repositories
{
    public class MoodRepository : GenericRepository<MoodCapture>, IMoodRepository
    {
        public MoodRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<MoodCapture>> GetAsync(Expression<Func<MoodCapture, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

    }
}
