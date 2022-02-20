using GeekShopping.Email.Messages;
using GeekShopping.Email.Model;
using GeekShopping.Email.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<PostgressContext> _context;

        public EmailRepository(DbContextOptions<PostgressContext> context)
        {
            _context = context;
        }

        public async Task LogEmail(UpdatePaymentResultMessage message)
        {
            var emailLog = new EmailLog()
            {
                Email = message.Email,
                Log = $"Order - {message.OrderId} has been created successfully",
                SentDate = DateTime.Now,
            };

            await using var _db = new PostgressContext(_context);

            _db.Emails.Add(emailLog);

            await _db.SaveChangesAsync();
        }
    }
}
