using Microsoft.EntityFrameworkCore;
using MosEisleyCantina.MosEisleyCantina.Common.Models;

namespace MosEisleyCantina.MosEisleyCantina.DataAccess
{
    public interface ICantinaDbContext
    {
         Task<IEnumerable<Dish>> GetDishes();


         Task<Dish> GetDish(int id);


         Task<Dish> CreateDish(Dish dish);


         Task UpdateDish(int id, Dish dish);


         Task DeleteDish(int id);


         Task<IEnumerable<Drink>> GetDrinks();


         Task<Drink> GetDrink(int id);


         Task<Drink> CreateDrink(Drink drink);


         Task UpdateDrink(int id, Drink drink);


         Task DeleteDrink(int id);

    }
}