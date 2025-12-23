using System;
using System.Collections.Generic;
using System.Linq;
using MotorcycleShop.Domain;
using MotorcycleShop.Data.Interfaces;

namespace MotorcycleShop.Data.InMemory
{
    /// <summary>
    /// In-memory реализация репозитория для работы с корзиной
    /// </summary>
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly List<ShoppingCart> _shoppingCarts = new List<ShoppingCart>();
        private int _nextId = 1;

        public int Add(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
                throw new ArgumentNullException(nameof(shoppingCart));

            shoppingCart.Id = _nextId++;
            _shoppingCarts.Add(shoppingCart);
            return shoppingCart.Id;
        }

        public ShoppingCart GetById(int id)
        {
            return _shoppingCarts.FirstOrDefault(sc => sc.Id == id);
        }

        public List<ShoppingCart> GetAll()
        {
            return new List<ShoppingCart>(_shoppingCarts);
        }

        public bool Update(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
                throw new ArgumentNullException(nameof(shoppingCart));

            var existing = _shoppingCarts.FirstOrDefault(sc => sc.Id == shoppingCart.Id);
            if (existing == null)
                return false;

            // Обновляем все свойства
            existing.SessionId = shoppingCart.SessionId;
            existing.CreatedDate = shoppingCart.CreatedDate;
            existing.LastModified = shoppingCart.LastModified;
            existing.TotalAmount = shoppingCart.TotalAmount;
            // CartItems не обновляем здесь, для этого есть отдельный репозиторий

            return true;
        }

        public bool Delete(int id)
        {
            var shoppingCart = _shoppingCarts.FirstOrDefault(sc => sc.Id == id);
            if (shoppingCart == null)
                return false;

            return _shoppingCarts.Remove(shoppingCart);
        }
    }
}