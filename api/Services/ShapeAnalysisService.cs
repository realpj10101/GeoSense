using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using api.Enums;
using api.Helpers;
using api.Interfaces;
using api.Models;

namespace api.Services;

public class ShapeAnalysisService : IShapeAnalysisService
{
    public OperationResult<ShapeResponse> Analyze(ShapeRequest request)
    {
        List<double> distances = [];
        IEnumerable<double> sides = [];
        IEnumerable<double> diagonals = [];

        if (request.Points.Count != 4)
        {
            return new(
                false,
                Error: new(
                    ErrorCode.IsNotFourPoints,
                    "You must enter 4 points"
                )
            );
        }

        bool arePointsUnique = CheckArePointsUnique(request.Points);

        if (!arePointsUnique)
        {
            return new(
                false,
                Error: new(
                    ErrorCode.ArePointsSame,
                    "You entered at least 1 duplicate point"
                )
            );
        }

        distances = CalcAllDistances(request.Points);

        sides = SortDistance(distances);

        diagonals = GetDiagonals(distances);

        double ratio = CheckRatioOfSides(sides);

        if (ratio < 0.8)
        {
            string quality = ratio < 0.3 ? "Poor" : "Average";

            return new(
                false,
                Error: new(
                    ErrorCode.AreSidesApprove,
                    $"Shape does not approved. Quality of side: {quality}"
                )
            );
        }

        double finalSquareScore = CheckIsSquare(sides, diagonals);

        if (finalSquareScore < 0.9 || finalSquareScore > 1.1)
        {
            return new(
                false,
                Error: new(
                    ErrorCode.IsRhombus,
                    "Shape is Rhombus"
                )
            );
        }

        return new(
            true,
            new()
            {
                ShapeType = "Square",
                Score = finalSquareScore,
                Message = "You create square successfully"
            },
            null
        );
    }

    // analyze shape do not contains same points
    private bool CheckArePointsUnique(List<PointDto> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                if (points[i].X == points[j].X && points[i].Y == points[j].Y)
                    return false;
            }
        }

        return true;
    }

    // Calc EuclideanDistance between points
    private static List<double> CalcAllDistances(List<PointDto> points)
    {
        List<double> distances = [];

        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                double distance = CalcEuclideanDistance(points[i], points[j]);

                distances.Add(distance);
            }
        }

        return distances;
    }

    private static double CalcEuclideanDistance(PointDto point1, PointDto point2)
    {
        return Math.Sqrt(
            Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2)
        );
    }

    // sort and take the first four from distances for sides
    private static IEnumerable<double> SortDistance(List<double> distances)
    {
        distances.Sort();

        return distances.Take(4);
    }

    private static IEnumerable<double> GetDiagonals(List<double> distances)
    {
        return distances.TakeLast(2);
    }

    // First rule check ratio of sides 
    private static double CheckRatioOfSides(IEnumerable<double> sides)
    {
        return sides.Min() / sides.Max();
    }

    private static double CheckIsSquare(IEnumerable<double> sides, IEnumerable<double> diagonals)
    {
        double avgSides = sides.Average();

        double avgDiagonals = diagonals.Average();

        double expectedDiagonal = avgSides * Math.Sqrt(2);

        return avgDiagonals / expectedDiagonal;
    }
}