using Microsoft.EntityFrameworkCore;
using MotorcycleShop.Data.Interfaces;
using MotorcycleShop.Domain;

namespace MotorcycleShop.Data.SqlServer
{
    /// <summary>
    /// Реализация репозитория для работы с платежами в SQL Server
    /// </summary>
    public class PaymentSqlServerRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentSqlServerRepository(MotorcycleShopDbContext context) : base(context)
        {
        }

        public int Add(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            _dbSet.Add(payment);
            _context.SaveChanges();
            return payment.Id;
        }

        public Payment GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public List<Payment> GetAll()
        {
            return _dbSet.ToList();
        }

        public bool Update(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            var existing = _dbSet.Find(payment.Id);
            if (existing == null)
                return false;

            _context.Entry(existing).CurrentValues.SetValues(payment);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var payment = _dbSet.Find(id);
            if (payment == null)
                return false;

            _dbSet.Remove(payment);
            _context.SaveChanges();
            return true;
        }
    }
}