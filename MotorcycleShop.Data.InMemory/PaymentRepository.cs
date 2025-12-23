using System;
using System.Collections.Generic;
using System.Linq;
using MotorcycleShop.Domain;
using MotorcycleShop.Data.Interfaces;

namespace MotorcycleShop.Data.InMemory
{
    /// <summary>
    /// In-memory реализация репозитория для работы с платежами
    /// </summary>
    public class PaymentRepository : IPaymentRepository
    {
        private readonly List<Payment> _payments = new List<Payment>();
        private int _nextId = 1;

        public int Add(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            payment.Id = _nextId++;
            _payments.Add(payment);
            return payment.Id;
        }

        public Payment GetById(int id)
        {
            return _payments.FirstOrDefault(p => p.Id == id);
        }

        public List<Payment> GetAll()
        {
            return new List<Payment>(_payments);
        }

        public bool Update(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            var existing = _payments.FirstOrDefault(p => p.Id == payment.Id);
            if (existing == null)
                return false;

            // Обновляем все свойства
            existing.OrderId = payment.OrderId;
            existing.PaymentDate = payment.PaymentDate;
            existing.Amount = payment.Amount;
            existing.PaymentMethod = payment.PaymentMethod;
            existing.CardLastFour = payment.CardLastFour;
            existing.TransactionId = payment.TransactionId;
            existing.Status = payment.Status;

            return true;
        }

        public bool Delete(int id)
        {
            var payment = _payments.FirstOrDefault(p => p.Id == id);
            if (payment == null)
                return false;

            return _payments.Remove(payment);
        }
    }
}