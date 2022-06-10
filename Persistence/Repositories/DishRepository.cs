using System.Collections.Generic;
using dotnet_api_test.Exceptions.ExceptionResponses;
using dotnet_api_test.Persistence.Repositories.Interfaces;
using dotnet_api_test.Validation;
using Microsoft.EntityFrameworkCore;

namespace dotnet_api_test.Persistence.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly AppDbContext _context;

        public DishRepository(AppDbContext context)
        {
            _context = context;
        }

        void IDishRepository.SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Dish> GetAllDishes()
        {
            return _context.Dishes.ToList();
        }

        public dynamic? GetAverageDishPrice()
        {
            return GetAllDishes().Select(x => x.Cost).Average();
        }

        public Dish GetDishById(int Id)
        {
            
            var dishObj = _context.Dishes.FirstOrDefault(x => x.Id == Id);
            if (dishObj == null) return null;
            
            return dishObj;

        }

        public bool DeleteDishById(int Id)
        {

            var dish = _context.Dishes.FirstOrDefault(x => x.Id == Id);
            if (dish == null) return false;
            _context.Dishes.Remove(dish);

            return _context.SaveChanges() > 0;
        }

        public Dish CreateDish(Dish dish)
        {

            var addDish = _context.Dishes.Add(dish);

            _context.SaveChanges();

            return _context.Dishes.FirstOrDefault(x => x.Id == dish.Id);
            
        }

        public Dish UpdateDish(int id , Dish dish)
        {
            var dishInDb = _context.Dishes.FirstOrDefault(x => x.Id == id);

            if (dishInDb == null) return null;
            
            dish.Id = id;
            _context.Entry(dishInDb).CurrentValues.SetValues(dish);
            
            _context.SaveChanges();

            return _context.Dishes.FirstOrDefault(x => x.Id == id);

        }
    }
}