using MediatR;
using AutoMapper;
using Structure.Repository.UnitOfWork;
using Structure.Data.Common;
using Structure.Data.DTOs;
using Structure.Data.Entities;
using Structure.MediatR.Student.Queries;
using Microsoft.Extensions.Logging;
using StudentEntity = Structure.Data.Entities.Student;

namespace Structure.MediatR.Student.Handlers;

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, ApiResponse<StudentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetStudentByIdQueryHandler> _logger;

    public GetStudentByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetStudentByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<StudentDto>> Handle(
        GetStudentByIdQuery request,
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

            var studentDto = _mapper.Map<StudentDto>(student);
            _logger.LogInformation($"Retrieved student with ID: {request.Id}");

            return ApiResponse<StudentDto>.Success(studentDto, "Student retrieved successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ApiResponse<StudentDto>.Failure(ex.Message, new List<string> { ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student");
            return ApiResponse<StudentDto>.Failure("Failed to retrieve student", new List<string> { ex.Message });
        }
    }
}
