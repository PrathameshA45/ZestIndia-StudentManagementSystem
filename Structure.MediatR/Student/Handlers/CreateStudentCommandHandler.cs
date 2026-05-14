using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Structure.Data.Common;
using Structure.Data.DTOs;
using Structure.Data.Entities;
using Structure.MediatR.Student.Commands;
using Structure.Repository;
using Structure.Repository;
using Structure.Repository.UnitOfWork;
using StudentEntity = Structure.Data.Entities.Student;

namespace Structure.MediatR.Student.Handlers;

/// <summary>
/// Handler for CreateStudentCommand
/// IMPROVEMENTS:
/// ✅ Checks for duplicate emails before creating
/// ✅ Better error messages and validation
/// ✅ Proper transaction handling
/// ✅ Better logging with context information
/// ✅ Returns proper error responses
/// ✅ Validates age constraints
/// ✅ Sanitizes input data
/// </summary>
public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, ApiResponse<StudentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateStudentCommandHandler> _logger;
    private readonly IStudentRepository _studentRepository;

    public CreateStudentCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateStudentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        // Get student repository from unit of work
        _studentRepository = unitOfWork.Repository<StudentEntity>() as IStudentRepository
            ?? throw new InvalidOperationException("StudentRepository not registered");
    }

    public async Task<ApiResponse<StudentDto>> Handle(
        CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate request
            var validationErrors = ValidateRequest(request);
            if (validationErrors.Count > 0)
            {
                _logger.LogWarning($"Validation failed for create student request: {string.Join(", ", validationErrors)}");
                return ApiResponse<StudentDto>.Failure(
                    "Student creation failed",
                    validationErrors);
            }

            // Check for duplicate email
            var emailExists = await _studentRepository.EmailExistsAsync(request.Email);
            if (emailExists)
            {
                _logger.LogWarning($"Attempt to create student with duplicate email: {request.Email}");
                return ApiResponse<StudentDto>.Failure(
                    "Student creation failed",
                    new List<string>
                    {
                        $"Email '{request.Email}' is already registered"
                    });
            }

            // Create student entity
            var student = new StudentEntity
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLower(),
                Age = request.Age,
                Course = request.Course.Trim(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _logger.LogInformation($"Creating new student: Name={student.Name}, Email={student.Email}, Course={student.Course}");

            // Add to repository
            await _unitOfWork.Repository<StudentEntity>().AddAsync(student);

            // Save changes

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result <= 0)
            {
                _logger.LogError("Failed to save student to database");

                return ApiResponse<StudentDto>.Failure(
                    "Student creation failed",
                    new List<string>
                    {
            "Failed to save student to database"
                    });
            }

            var studentDto = _mapper.Map<StudentDto>(student);
            _logger.LogInformation($"Student created successfully. ID: {student.Id}, Email: {student.Email}");

            return ApiResponse<StudentDto>.Success(
                studentDto,
                "Student created successfully");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Create student operation was cancelled");
            return ApiResponse<StudentDto>.Failure(
                "Operation cancelled",
                new List<string> { "The operation was cancelled. Please try again." });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation while creating student");
            return ApiResponse<StudentDto>.Failure(
                "Invalid operation",
                new List<string> { ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating student");
            return ApiResponse<StudentDto>.Failure(
                "Failed to create student",
                new List<string>
                {
                    "An unexpected error occurred while creating the student. Please contact support."
                });
        }
    }

    /// <summary>
    /// Validates the create student request
    /// </summary>
    private List<string> ValidateRequest(CreateStudentCommand request)
    {
        var errors = new List<string>();

        // Validate name
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors.Add("Name is required");
        }
        else if (request.Name.Length > 100)
        {
            errors.Add("Name cannot exceed 100 characters");
        }
        else if (request.Name.Length < 2)
        {
            errors.Add("Name must be at least 2 characters");
        }

        // Validate email
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            errors.Add("Email is required");
        }
        else if (!IsValidEmail(request.Email))
        {
            errors.Add("Email format is invalid");
        }
        else if (request.Email.Length > 100)
        {
            errors.Add("Email cannot exceed 100 characters");
        }

        // Validate age
        if (request.Age < 15)
        {
            errors.Add("Student age must be at least 15 years old");
        }
        else if (request.Age > 65)
        {
            errors.Add("Student age cannot exceed 65 years");
        }

        // Validate course
        if (string.IsNullOrWhiteSpace(request.Course))
        {
            errors.Add("Course is required");
        }
        else if (request.Course.Length > 100)
        {
            errors.Add("Course name cannot exceed 100 characters");
        }
        else if (request.Course.Length < 2)
        {
            errors.Add("Course name must be at least 2 characters");
        }

        return errors;
    }

    /// <summary>
    /// Basic email validation
    /// </summary>
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}