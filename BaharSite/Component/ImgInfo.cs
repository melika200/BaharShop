using Microsoft.AspNetCore.Mvc;

namespace BaharSite.Component
{
    public class ImgInfo:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("/Views/Shared/Component/ImgInfo.cshtml");
        }
    }
}
