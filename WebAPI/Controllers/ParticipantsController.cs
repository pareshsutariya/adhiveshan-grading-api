using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AdhiveshanGrading.Models;
using AdhiveshanGrading.Services;

namespace AdhiveshanGradingAPI.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParticipantsController : ControllerBase
{
    private readonly IParticipantsService _service;

    public ParticipantsController(IParticipantsService service)
    {
        _service = service;
    }

    [HttpGet("{center}/{mandal}")]
    public async Task<List<ParticipantModel>> Get(string center = "", string mandal = "") => await _service.Get(center, mandal);

    [HttpGet("getById/{id}")]
    public async Task<ParticipantModel> GetById(int id) => await _service.GetById(id);

    [HttpGet("getByMISId/{misId}")]
    public async Task<ParticipantModel> GetByMISId(int misId) => await _service.GetByMISId(misId);

    [HttpPost("import")]
    public async Task<ActionResult<List<ParticipantModel>>> Import(List<ParticipantModel> models)
    {
        var result = await _service.Import(models);

        return result;
    }
}

