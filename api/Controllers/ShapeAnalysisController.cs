using api.Enums;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShapeAnalysisController(IShapeAnalysisService _shapeAnalysisService) : ControllerBase
{
    [HttpPost]
    public ActionResult<ShapeResponse> Analyze(ShapeRequest req)
    {
        OperationResult<ShapeResponse> opResult = _shapeAnalysisService.Analyze(req);

        return opResult.IsSuccess
        ? opResult.Result
        : opResult.Error?.Code switch
        {
            ErrorCode.IsNotFourPoints => BadRequest(opResult.Error.Message),
            ErrorCode.ArePointsSame => BadRequest(opResult.Error.Message),
            ErrorCode.AreSidesApprove => BadRequest(opResult.Error.Message),
            ErrorCode.IsRhombus => BadRequest(opResult.Error.Message),
            _ => BadRequest("Something went wrong")
        };
    }
}