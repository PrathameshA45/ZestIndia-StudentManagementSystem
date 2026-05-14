using MediatR;
using Structure.Data.Common;
using Structure.Data.DTOs;

namespace Structure.MediatR.Student.Queries;

public class GetStudentByIdQuery : IRequest<ApiResponse<StudentDto>>
{
    public Guid Id { get; set; }

    public GetStudentByIdQuery(Guid id)
    {
        Id = id;
    }
}
