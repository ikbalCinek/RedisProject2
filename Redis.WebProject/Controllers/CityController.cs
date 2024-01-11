using Microsoft.AspNetCore.Mvc;
using Redis.Cache.Abstract;
using Redis.Cache.Entities;

namespace Redis.WebProject.Controllers
{
    public class CityController : Controller
    {
        private readonly ICacheService<City> _cacheService;

        public CityController(ICacheService<City> cacheService )
        {
            _cacheService = cacheService;
        }

        public IActionResult AddCity(string id, string name)
        {
            City city = new City() { Id = id, Name = name };
            _cacheService.SetValue( city );
            return Ok($"Şehir bilgisi eklendi : {name}");
        }


        public IActionResult GetCity(string key)
        {
            var city = _cacheService.GetValue(key);
            return Ok(city);
        }


        public IActionResult GetAllCity()
        {
            var list=_cacheService.GetAll();
            return Ok(list);
        }

        public IActionResult DeleteCity(string id)
        {
            var result=_cacheService.DeleteValue(id);
            if (result) return Ok("Silindi");
            return BadRequest("Silinmedi.");
        }


    }
}
