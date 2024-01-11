using Microsoft.AspNetCore.Mvc;
using Redis.Cache.Abstract;
using Redis.Cache.Entities;

namespace Redis.WebProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ICacheService<User> _cacheService;

        public UserController(ICacheService<User> cacheService)
        {
            _cacheService = cacheService;
        }


        public IActionResult AddUser() { return View(new User()); }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            Guid guid=Guid.NewGuid();
            user.Id=guid.ToString();
            _cacheService.SetValue(user);
            return Ok($"Kullanıcı eklendi. {user.FirstName} - {user.LastName}");
        }

        public IActionResult GetAllUser()
        {
            var list=_cacheService.GetAll();
            return Ok(list);
        }



        public IActionResult GetUser(string key)
        {
            var user=_cacheService.GetValue(key);
            return Ok(user);    

        }


        public IActionResult DeleteUser(string id)
        {
            var result = _cacheService.DeleteValue(id);
            if (result) return Ok("Silindi");
            return BadRequest("Silinmedi");
        }




    }
}
