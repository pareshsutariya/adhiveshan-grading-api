using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using AdhiveshanGrading.Entities;
using AdhiveshanGrading.Services;

namespace AdhiveshanGradingAPI.WebAPI.Controllers;

[Route("/")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    public string Index() => "Welcome";
}
