using MoodSensingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoodSensingApp.Repositories
{
    public interface IMoodRepository : IGenericRepository<MoodCapture>
    {
        Task<IEnumerable<MoodCapture>> GetAsync(Expression<Func<MoodCapture, bool>> predicate);
       
    }
}