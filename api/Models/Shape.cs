namespace api.Models;

public record PointDto(
    double X,
    double Y
);

public class ShapeRequest
{
    public List<PointDto> Points { get; set; } = [];
}

public class ShapeResponse
{
    public string ShapeType { get; set; } = string.Empty;
    public double Score { get; set; }
    public string Message { get; set; } = string.Empty;
}