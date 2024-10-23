using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MosEisleyCantina.MosEisleyCantina.Common.Models;
using MosEisleyCantina.MosEisleyCantina.Domain;

namespace MosEisleyCantina.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CantinaController : ControllerBase
{
    private readonly ICantinaService _cantinaService;

    public CantinaController(IServiceProvider services)
    {
        _cantinaService = (ICantinaService)services.GetService(typeof(ICantinaService));
    }

    [HttpGet("dishes/")]
    public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
    {
        var dishes = await _cantinaService.GetDishes();
        return dishes == null ? NotFound() : Ok(dishes);
    }

    [HttpGet("dishes/{id}")]
    public async Task<ActionResult<Dish>> GetDish(int id)
    {
        var dish = await _cantinaService.GetDish(id);
        return dish == null ? NotFound() : Ok(dish);
    }

    [HttpPost("dishes/")]
    public async Task<ActionResult<Dish>> CreateDish(Dish dish)
    {
        var newDish = await _cantinaService.CreateDish(dish);
        return CreatedAtAction(nameof(GetDish), new { id = newDish.Id }, newDish);
    }

    [HttpPut("dishes/{id}")]
    public async Task<IActionResult> UpdateDish(int id, Dish dish)
    {
        await _cantinaService.UpdateDish( id, dish);
        return NoContent();
    }

    [HttpDelete("dishes/{id}")]
    public async Task<IActionResult> DeleteDish(int id)
    {
        await _cantinaService.DeleteDish(id);
        return NoContent();
    }

    [HttpGet("drinks/")]
    public async Task<ActionResult<IEnumerable<Drink>>> GetDrinks()
    {
        var drinks = await _cantinaService.GetDrinks();
        return drinks == null ? NotFound() : Ok(drinks);

    }

    [HttpGet("drinks/{id}")]
    public async Task<ActionResult<Drink>> GetDrink(int id)
    {
        var drink = await _cantinaService.GetDrink(id);
        return drink == null ? NotFound() : Ok(drink);
    }

    [HttpPost("drinks/")]
    public async Task<ActionResult<Drink>> CreateDrink(Drink drink)
    {
        var newDrink = await _cantinaService.CreateDrink(drink);
        return CreatedAtAction(nameof(GetDish), new { id = newDrink.Id }, newDrink);
    }

    [HttpPut("drinks/{id}")]
    public async Task<IActionResult> UpdateDrink(int id, Drink drink)
    {
        await _cantinaService.UpdateDrink( id,  drink);
        return NoContent();
    }

    [HttpDelete("drinks/{id}")]
    public async Task<IActionResult> DeleteDrink(int id)
    {
        await _cantinaService.DeleteDrink( id);
        return NoContent();
    }
}