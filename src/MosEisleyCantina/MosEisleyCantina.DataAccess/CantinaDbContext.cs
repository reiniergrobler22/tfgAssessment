using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MosEisleyCantina.MosEisleyCantina.Common.Models;

namespace MosEisleyCantina.MosEisleyCantina.DataAccess
{
    public class CantinaDbContext : IdentityDbContext<User>, ICantinaDbContext
    {
        // Public DbSet properties for dishes and drinks
        private DbSet<Dish> Dishes { get; set; }
        public DbSet<Drink> Drinks { get; set; }

        public CantinaDbContext(DbContextOptions<CantinaDbContext> options) : base(options)
        {
        }

        // Configure additional model constraints if needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dish>()
                .HasKey(d => d.Id); // Ensure Id is the primary key

            modelBuilder.Entity<Dish>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Drink>()
                .HasKey(d => d.Id); // Ensure Id is the primary key

            modelBuilder.Entity<Drink>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd();
        }

        // Retrieve all dishes
        public async Task<IEnumerable<Dish>> GetDishes()
        {
            return await Dishes.ToListAsync();  
        }

        // Retrieve a single dish by ID
        public async Task<Dish> GetDish(int id)
        {
            var dish = await Dishes.FindAsync(id);  
            if (dish == null)
                throw new NullReferenceException("Dish not found");
            return dish;
        }

        // Create a new dish
        public async Task<Dish> CreateDish(Dish dish)
        {
            await Dishes.AddAsync(dish);  
            await SaveChangesAsync();
            return dish;
        }

        // Update an existing dish
        public async Task UpdateDish(int id, Dish dish)
        {
            Entry(dish).State = EntityState.Modified;  
            await SaveChangesAsync();
        }

        // Delete a dish
        public async Task DeleteDish(int id)
        {
            var dish = await Dishes.FindAsync(id);  
            if (dish == null)
                throw new NullReferenceException("Dish not found");
            Dishes.Remove(dish);
            await SaveChangesAsync();
        }

        // Retrieve all drinks
        public async Task<IEnumerable<Drink>> GetDrinks()
        {
            return await Drinks.ToListAsync(); 
        }

        // Retrieve a single drink by ID
        public async Task<Drink> GetDrink(int id)
        {
            var drink = await Drinks.FindAsync(id);  
            if (drink == null)
                throw new NullReferenceException("Drink not found");
            return drink;
        }

        // Create a new drink
        public async Task<Drink> CreateDrink(Drink drink)
        {
            await Drinks.AddAsync(drink);  
            await SaveChangesAsync();
            return drink;
        }

        // Update an existing drink
        public async Task UpdateDrink(int id, Drink drink)
        {
            Entry(drink).State = EntityState.Modified; 
            await SaveChangesAsync();
        }

        // Delete a drink
        public async Task DeleteDrink(int id)
        {
            var drink = await Drinks.FindAsync(id); 
            if (drink == null)
                throw new NullReferenceException("Drink not found");
            Drinks.Remove(drink);
            await SaveChangesAsync();
        }
    }
}
