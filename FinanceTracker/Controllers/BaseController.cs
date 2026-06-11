using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers;

public class BaseController : ControllerBase
{
    protected int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}