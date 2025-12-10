using api.Models;

namespace api.Interfaces;

public interface IShapeAnalysisService
{
    public ShapeResponse Analyze(ShapeRequest request);
}