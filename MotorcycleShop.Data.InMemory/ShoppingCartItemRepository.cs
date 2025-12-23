using System;
using System.Collections.Generic;
using System.Linq;
using MotorcycleShop.Domain;
using MotorcycleShop.Data.Interfaces;

namespace MotorcycleShop.Data.InMemory
{
    /// <summary>
    /// In-memory реализация репозитория для работы с позициями корзины
    /// </summary>
    public class ShoppingCartItemRepository : IShoppingCartItemRepository
    {
        private readonly List<ShoppingCartItem> _shoppingCartItems = new List<ShoppingCartItem>();
        private int _nextId = 1;

        public int Add(ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem == null)
                throw new ArgumentNullException(nameof(shoppingCartItem));

            shoppingCartItem.Id = _nextId++;
            _shoppingCartItems.Add(shoppingCartItem);
            return shoppingCartItem.Id;
        }

        public ShoppingCartItem GetById(int id)
        {
            return _shoppingCartItems.FirstOrDefault(sci => sci.Id == id);
        }

        public List<ShoppingCartItem> GetAll()
        {
            return new List<ShoppingCartItem>(_shoppingCartItems);
        }

        public bool Update(ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem == null)
                throw new ArgumentNullException(nameof(shoppingCartItem));

            var existing = _shoppingCartItems.FirstOrDefault(sci => sci.Id == shoppingCartItem.Id);
            if (existing == null)
                return false;

            // Обновляем все свойства
            existing.ShoppingCartId = shoppingCartItem.ShoppingCartId;
            existing.MotorcycleId = shoppingCartItem.MotorcycleId;
            existing.Quantity = shoppingCartItem.Quantity;
            existing.AddedDate = shoppingCartItem.AddedDate;

            return true;
        }

        public bool Delete(int id)
        {
            var shoppingCartItem = _shoppingCartItems.FirstOrDefault(sci => sci.Id == id);
            if (shoppingCartItem == null)
                return false;

            return _shoppingCartItems.Remove(shoppingCartItem);
        }
    }
}