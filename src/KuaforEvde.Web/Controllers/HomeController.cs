using System.Threading.Tasks;
using KuaforEvde.Service.Query.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KuaforEvde.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index()
        {
            await _mediator.Send(new ServiceSaveQuery
            {

            });
            return View();
        }
    }
}
