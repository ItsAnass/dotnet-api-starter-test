using System.Collections.Generic;

namespace dotnet_api_test.Persistence.Repositories.Interfaces
{
    public interface IDishRepository
    {
        void SaveChanges();
        IEnumerable<Dish> GetAllDishes();
        dynamic? GetAverageDishPrice();
        Dish GetDishById(int Id);
        bool DeleteDishById(int Id);
        Dish CreateDish(Dish dish);
        Dish UpdateDish(int id, Dish dish);
    }
}