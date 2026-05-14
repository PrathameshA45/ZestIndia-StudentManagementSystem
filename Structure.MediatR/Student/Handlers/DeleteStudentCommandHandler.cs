using MediatR;
using Structure.Repository.UnitOfWork;
using Structure.Data.Common;
using Structure.Data.Entities;
using Structure.MediatR.Student.Commands;
using Microsoft.Extensions.Logging;
using StudentEntity = Structure.Data.Entities.Student;

namespace Structure.MediatR.Student.Handlers;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, ApiResponse<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteStudentCommandHandler> _logger;

    public DeleteStudentCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteStudentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiResponse<string>> Handle(
        DeleteStudentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var student = await _unitOfWork.Repository<StudentEntity>().GetByIdAsync(request.Id);
            
            if (student == null || student.IsDeleted)
            {
                _logger.LogWarning($"Student not found with ID: {request.Id}");
                throw new KeyNotFoundException($"Student with ID {request.Id} not found");
            }

            student.IsDeleted = true;
            student.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<StudentEntity>().Update(student);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Student deleted successfully with ID: {student.Id}");

            return ApiResponse<string>.Success(request.Id.ToString(), "Student deleted successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ApiResponse<string>.Failure(ex.Message, new List<string> { ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student");
            return ApiResponse<string>.Failure("Failed to delete student", new List<string> { ex.Message });
        }
    }
}
