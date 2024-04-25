
using Microsoft.AspNetCore.Mvc;

namespace Pharma.Components{

    public class FooterComponent : ViewComponent{

        public async Task<IViewComponentResult> InvokeAsync(){


            return View("~/Views/Shared/_Footer.cshtml");

        }

    }

}