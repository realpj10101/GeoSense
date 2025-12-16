using api.Helpers;
using api.Models;

namespace api.Interfaces;

public interface IShapeAnalysisService
{
    public OperationResult<ShapeResponse> Analyze(ShapeRequest request);
}   