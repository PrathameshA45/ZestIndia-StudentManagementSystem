using MediatR;
using AutoMapper;
using Structure.Repository.UnitOfWork;
using Structure.Data.Common;
using Structure.Data.DTOs;
using Structure.Data.Entities;
using Structure.MediatR.Student.Commands;
using Microsoft.Extensions.Logging;
using StudentEntity = Structure.Data.Entities.Student;

namespace Structure.MediatR.Student.Handlers;

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, ApiResponse<StudentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateStudentCommandHandler> _logger;

    public UpdateStudentCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateStudentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<StudentDto>> Handle(
        UpdateStudentCommand request,
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

            student.Name = request.Name;
            student.Email = request.Email;
            student.Age = request.Age;
            student.Course = request.Course;
            student.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<StudentEntity>().Update(student);
            await _unitOfWork.SaveChangesAsync();

            var studentDto = _mapper.Map<StudentDto>(student);
            _logger.LogInformation($"Student updated successfully with ID: {student.Id}");

            return ApiResponse<StudentDto>.Success(studentDto, "Student updated successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ApiResponse<StudentDto>.Failure(ex.Message, new List<string> { ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student");
            return ApiResponse<StudentDto>.Failure("Failed to update student", new List<string> { ex.Message });
        }
    }
}
