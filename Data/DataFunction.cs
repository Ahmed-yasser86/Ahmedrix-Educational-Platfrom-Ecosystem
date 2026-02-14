
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Entities;

namespace OnlineCoursesPlatform.Data 
{
    public class DataFunction : IDataFunction
    {
        private readonly ApplicationDbContext _context;
        
        public DataFunction(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task UpdateUserCategoryEntityAsyc(List<UserCategory> userCatToDelete, List<UserCategory> userCatToAdd)
        {

            using (var dbContextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    _context.RemoveRange(userCatToDelete);
                    await _context.SaveChangesAsync();

                    if (userCatToAdd != null)
                    {
                        _context.AddRange(userCatToAdd);
                        await _context.SaveChangesAsync();
                    }
                    await dbContextTransaction.CommitAsync();

                }

                catch (Exception ex)
                {
                    await dbContextTransaction.DisposeAsync();
                }
            }
        }
    }
}
