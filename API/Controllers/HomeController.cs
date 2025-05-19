namespace AdhiveshanGrading.Controllers;

[Route("/")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    public string Index() => "Welcome";
}
