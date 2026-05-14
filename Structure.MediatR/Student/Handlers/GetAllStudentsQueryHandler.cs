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

public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, ApiResponse<List<StudentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllStudentsQueryHandler> _logger;

    public GetAllStudentsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetAllStudentsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<List<StudentDto>>> Handle(
        GetAllStudentsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var students = await _unitOfWork.Repository<StudentEntity>().GetAllAsync();
            var studentDtos = _mapper.Map<List<StudentDto>>(students);

            _logger.LogInformation($"Retrieved {studentDtos.Count} students");

            return ApiResponse<List<StudentDto>>.Success(studentDtos, "Students retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving students");
            return ApiResponse<List<StudentDto>>.Failure("Failed to retrieve students", new List<string> { ex.Message });
        }
    }
}
