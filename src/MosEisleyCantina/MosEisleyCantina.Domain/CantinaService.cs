using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using MosEisleyCantina.MosEisleyCantina.Common.Models;
using MosEisleyCantina.MosEisleyCantina.DataAccess;

namespace MosEisleyCantina.MosEisleyCantina.Domain
{
    public class CantinaService : ICantinaService
    {
        private readonly ICantinaDbContext _cantinaDbContext;
        private readonly IMemoryCache _cache;

        public CantinaService(IServiceProvider services)
        {
            _cantinaDbContext = (ICantinaDbContext)services.GetService(typeof(ICantinaDbContext));
            _cache = (IMemoryCache)services.GetService(typeof(IMemoryCache));
        }

        public async Task<Dish> CreateDish(Dish dish)
        {
            // Validate the dish
            if (dish == null)
            {
                throw new ArgumentNullException(nameof(dish), "Dish cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(dish.Name))
            {
                throw new ArgumentException("Dish name cannot be empty.", nameof(dish.Name));
            }

            // Check for unique name
            var existingDish = await _cantinaDbContext.GetDishes();
            if (existingDish.Any(d => d.Name.Equals(dish.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A dish with the name '{dish.Name}' already exists.");
            }

            var createdDish = await _cantinaDbContext.CreateDish(dish);

            // Invalidate cache for dishes
            _cache.Remove("Dishes");

            return createdDish;
        }

        public async Task<Drink> CreateDrink(Drink drink)
        {
            // Validate the drink
            if (drink == null)
            {
                throw new ArgumentNullException(nameof(drink), "Drink cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(drink.Name))
            {
                throw new ArgumentException("Drink name cannot be empty.", nameof(drink.Name));
            }

            // Check for unique name
            var existingDrinks = await _cantinaDbContext.GetDrinks();
            if (existingDrinks.Any(d => d.Name.Equals(drink.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A drink with the name '{drink.Name}' already exists.");
            }

            var createdDrink = await _cantinaDbContext.CreateDrink(drink);

            // Invalidate cache for drinks
            _cache.Remove("Drinks");

            return createdDrink;
        }

        public async Task DeleteDish(int id)
        {
            var dish = await _cantinaDbContext.GetDish(id);
            if (dish == null)
            {
                throw new KeyNotFoundException($"Dish with ID '{id}' not found.");
            }

            await _cantinaDbContext.DeleteDish(id);

            // Invalidate cache for dishes
            _cache.Remove("Dishes");
        }

        public async Task DeleteDrink(int id)
        {
            var drink = await _cantinaDbContext.GetDrink(id);
            if (drink == null)
            {
                throw new KeyNotFoundException($"Drink with ID '{id}' not found.");
            }

            await _cantinaDbContext.DeleteDrink(id);

            // Invalidate cache for drinks
            _cache.Remove("Drinks");
        }

        public async Task<Dish> GetDish(int id)
        {
            var cacheKey = $"Dish_{id}";

            // Try to get the dish from the cache
            if (!_cache.TryGetValue(cacheKey, out Dish dish))
            {
                dish = await _cantinaDbContext.GetDish(id);
                if (dish == null)
                {
                    throw new KeyNotFoundException($"Dish with ID '{id}' not found.");
                }

                // Store in cache
                _cache.Set(cacheKey, dish);
            }

            return dish;
        }

        public async Task<IEnumerable<Dish>> GetDishes()
        {
            const string cacheKey = "Dishes";

            // Try to get the list of dishes from the cache
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Dish> dishes))
            {
                dishes = await _cantinaDbContext.GetDishes();

                // Store in cache
                _cache.Set(cacheKey, dishes);
            }

            return dishes;
        }

        public async Task<Drink> GetDrink(int id)
        {
            var cacheKey = $"Drink_{id}";

            // Try to get the drink from the cache
            if (!_cache.TryGetValue(cacheKey, out Drink drink))
            {
                drink = await _cantinaDbContext.GetDrink(id);
                if (drink == null)
                {
                    throw new KeyNotFoundException($"Drink with ID '{id}' not found.");
                }

                // Store in cache
                _cache.Set(cacheKey, drink);
            }

            return drink;
        }

        public async Task<IEnumerable<Drink>> GetDrinks()
        {
            const string cacheKey = "Drinks";

            // Try to get the list of drinks from the cache
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Drink> drinks))
            {
                drinks = await _cantinaDbContext.GetDrinks();

                // Store in cache
                _cache.Set(cacheKey, drinks);
            }

            return drinks;
        }

        public async Task UpdateDish(int id, Dish dish)
        {
            // Validate the dish
            if (dish == null)
            {
                throw new ArgumentNullException(nameof(dish), "Dish cannot be null.");
            }

            var existingDish = await _cantinaDbContext.GetDish(id);
            if (existingDish == null)
            {
                throw new KeyNotFoundException($"Dish with ID '{id}' not found.");
            }

            // Additional validation can be done here, e.g., checking for unique name
            await _cantinaDbContext.UpdateDish(id, dish);

            // Invalidate cache for the updated dish
            _cache.Remove($"Dish_{id}");
        }

        public async Task UpdateDrink(int id, Drink drink)
        {
            // Validate the drink
            if (drink == null)
            {
                throw new ArgumentNullException(nameof(drink), "Drink cannot be null.");
            }

            var existingDrink = await _cantinaDbContext.GetDrink(id);
            if (existingDrink == null)
            {
                throw new KeyNotFoundException($"Drink with ID '{id}' not found.");
            }

            // Additional validation can be done here, e.g., checking for unique name
            await _cantinaDbContext.UpdateDrink(id, drink);

            // Invalidate cache for the updated drink
            _cache.Remove($"Drink_{id}");
        }
    }
}
