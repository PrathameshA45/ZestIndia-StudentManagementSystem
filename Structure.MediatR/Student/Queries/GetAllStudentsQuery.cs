using MediatR;
using Structure.Data.Common;
using Structure.Data.DTOs;

namespace Structure.MediatR.Student.Queries;

public class GetAllStudentsQuery : IRequest<ApiResponse<List<StudentDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
