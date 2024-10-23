using MosEisleyCantina.MosEisleyCantina.Common.Models;

namespace MosEisleyCantina.MosEisleyCantina.Domain
{
    public interface ICantinaService
    {
        Task<Dish> CreateDish(Dish dish);
        Task<Drink> CreateDrink(Drink drink);
        Task DeleteDish(int id);
        Task DeleteDrink(int id);
        Task<Dish> GetDish(int id);
        Task<IEnumerable<Dish>> GetDishes();
        Task<Drink> GetDrink(int id);
        Task<IEnumerable<Drink>> GetDrinks();
        Task UpdateDish(int id, Dish dish);
        Task UpdateDrink(int id, Drink drink);
    }
}