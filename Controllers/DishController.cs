using AutoMapper;
using dotnet_api_test.Exceptions.ExceptionResponses;
using dotnet_api_test.Models.Dtos;
using dotnet_api_test.Persistence.Repositories.Interfaces;
using dotnet_api_test.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnet_api_test.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly ILogger<DishController> _logger;
        private readonly IMapper _mapper;
        private readonly IDishRepository _dishRepository;

        public DishController(ILogger<DishController> logger, IMapper mapper, IDishRepository dishRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _dishRepository = dishRepository;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<DishesAndAveragePriceDto> GetDishesAndAverageDishPrice()
        {

            var getDishes = _dishRepository.GetAllDishes().ToList();
            var avaragePrice = _dishRepository.GetAverageDishPrice();
            
            if (getDishes == null) return NotFound(new NotFoundRequestExceptionResponse("There are no dishes found", 404));

            DishesAndAveragePriceDto dishes = new DishesAndAveragePriceDto()
            {
                Dishes = _mapper.Map<List<Dish>, List<ReadDishDto>>(getDishes),
                AveragePrice = avaragePrice
            };

            return Ok(dishes);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<ReadDishDto> GetDishById(int id)
        {
            var getDishById = _dishRepository.GetDishById(id);

            if (getDishById == null) return NotFound(new NotFoundRequestExceptionResponse("Dish not found", 404));

            var dishDto = _mapper.Map<Dish, ReadDishDto>(getDishById);
            
            return Ok(dishDto);
            
        }

        [HttpPost]
        [Route("")]
        public ActionResult<ReadDishDto> CreateDish([FromBody] CreateDishDto createDishDto)
        {
            ModelValidation.ValidateCreateDishDto(createDishDto);

            var createObj = _dishRepository.CreateDish(_mapper.Map<CreateDishDto, Dish>(createDishDto));
            var dishDto = _mapper.Map<Dish, ReadDishDto>(createObj);
            
            return Ok(dishDto);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<ReadDishDto> UpdateDishById(int id, UpdateDishDto updateDishDto)
        {
            ModelValidation.ValidateUpdateDishDto(updateDishDto);

            var updateObj = _dishRepository.UpdateDish(id, _mapper.Map<UpdateDishDto, Dish>(updateDishDto));
            var dishDto = _mapper.Map<Dish, ReadDishDto>(updateObj);
            
            return Ok(dishDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteDishById(int id)
        {
           var response = _dishRepository.DeleteDishById(id);

            if (response == false) return BadRequest(new NotFoundRequestExceptionResponse("Dish not found", 404));
            return Ok("Deleted");
        }
    }
}