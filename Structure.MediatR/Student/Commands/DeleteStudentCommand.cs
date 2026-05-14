using MediatR;
using Structure.Data.Common;

namespace Structure.MediatR.Student.Commands;

public class DeleteStudentCommand
    : IRequest<ApiResponse<string>>
{
    public Guid Id { get; set; }
}