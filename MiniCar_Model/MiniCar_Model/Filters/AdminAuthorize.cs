using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MiniCar_Model.Filters
{
  public class AdminAuthorize : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      // Phải là "RoleId", KHÔNG ĐƯỢC viết sai thành "UserRole" hay "roleId"
      var roleId = context.HttpContext.Session.GetInt32("RoleId");

      if (roleId == null || roleId != 1)
      {
        // Kiểm tra xem có đang ở trang Login không để tránh vòng lặp
        var controller = context.RouteData.Values["controller"]?.ToString();
        var action = context.RouteData.Values["action"]?.ToString();

        if (!(controller == "Account" && action == "Login"))
        {
          context.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
        }
      }
      base.OnActionExecuting(context);
    }
  }
}
