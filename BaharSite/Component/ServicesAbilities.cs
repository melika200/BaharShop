using Microsoft.AspNetCore.Mvc;

namespace BaharSite.Component
{
    public class ServicesAbilities:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("/Views/Shared/Component/ServicesAbilities.cshtml");
        }
    }
}
