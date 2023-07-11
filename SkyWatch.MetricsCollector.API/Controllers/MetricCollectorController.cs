using System.Net;
using Hangfire;
using MetricsShared.MetricsCollectorDTO;
using Microsoft.AspNetCore.Mvc;
using SkyWatch.MetricsCollector.Core;

namespace SkyWatch.MetricsCollector.API.Controllers;

[Controller]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class MetricCollectorController : Controller
{
    private readonly ILogger<MetricCollectorController> _logger;
    private readonly MetricCollectorService _metricCollectorService;
    private readonly IBackgroundJobClient _backgroundJob;
    
    public MetricCollectorController(ILogger<MetricCollectorController> logger, MetricCollectorService metricCollectorService, IBackgroundJobClient backgroundJob)
    {
        _logger = logger;
        _metricCollectorService = metricCollectorService;
        _backgroundJob = backgroundJob;
    }


    [HttpPost("createMetric")]
    [ProducesResponseType(typeof(MCResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MCResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateMetric([FromBody] MCRequest request)
    {
        try
        {
            _logger.LogInformation("Create Metric Record. Start. Request {@request}", request);
            
            _backgroundJob.Enqueue(() => _metricCollectorService.CreateMetric(request));
            
            var response = GenerateSuccessfulResponse();
            
            _logger.LogInformation("Create Metric Record. End. Response {@response}", response);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create Metric Record. Error.");
            
            var response = GenerateFailResponse(ex);
            _logger.LogInformation("Create Metric Record. End. Response {@response}", response);
            return BadRequest(response);
        }
    }

    private static MCResponse GenerateFailResponse(Exception ex)
    {
        var response = new MCResponse
        {
            Success = false,
            Message = "Metric record creation failed.",
            ErrorMessage = ex.Message
        };
        return response;
    }

    private static MCResponse GenerateSuccessfulResponse()
    {
        var response = new MCResponse
        {
            Success = true,
            Message = "Metric record created successfully."
        };
        return response;
    }
}