using MediatR;
using Structure.Data.Common;
using Structure.Data.DTOs;

namespace Structure.MediatR.Student.Commands;

public class UpdateStudentCommand
    : IRequest<ApiResponse<StudentDto>>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int Age { get; set; }

    public string Course { get; set; } = string.Empty;
}