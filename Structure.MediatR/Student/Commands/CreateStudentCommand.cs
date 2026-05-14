using Structure.MediatR;
using Structure.Data.Common;
using Structure.Data.DTOs;
using MediatR;

namespace Structure.MediatR.Student.Commands;

public class CreateStudentCommand
    : IRequest<ApiResponse<StudentDto>>
{
    public string Name { get; set; } = string.Empty;
        
    public string Email { get; set; } = string.Empty;

    public int Age { get; set; }

    public string Course { get; set; } = string.Empty;
}