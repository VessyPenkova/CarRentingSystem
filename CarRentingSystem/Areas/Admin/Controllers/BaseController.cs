using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CarRentingSystem.Areas.Admin.Constants.AdminConstants;

namespace CarRentingSystem.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Route("/Admin/[controller]/[Action]/{id?}")]
    [Authorize(Roles = AdminRolleName)]
    public class BaseController : Controller
    {
       
    }
}
