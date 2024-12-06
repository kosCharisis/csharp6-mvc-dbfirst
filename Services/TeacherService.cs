using AutoMapper;
using SchoolApp.Data;
using SchoolApp.DTO;
using SchoolApp.Exceptions;
using SchoolApp.Repositories;
using SchoolApp.Security;
using Serilog;

namespace SchoolApp.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TeacherService> _logger;

        public TeacherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = new LoggerFactory().AddSerilog().CreateLogger<TeacherService>();
        }

        public async Task<List<User>> GetAllUsersTeachersAsync()
        {
            List<User> usersTeachers = new();
            try 
            {
                usersTeachers = await _unitOfWork.TeacherRepository.GetAllUsersTeachersAsync();
                _logger.LogInformation("{Message}", "All teachers returned");
            }
            catch(Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
            return usersTeachers;
        }


        public async Task<List<User>> GetAllUsersTeachersAsync(int pageNumber, int pageSize)
        {
            List<User> usersTeachers = new();
            try
            {
                usersTeachers = await _unitOfWork.TeacherRepository.GetAllUsersTeachersPaginatedAsync(pageNumber, pageSize);
                _logger.LogInformation("{Message}", "All teachers paginated returned");
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);

            }
            return usersTeachers;
        }

        public async Task<User?> GetTeacherByUsernameAsync(string username)
        {
            return await _unitOfWork.TeacherRepository.GetUserTeacherByUsernameAsync(username);
        }

        public async Task<int> GetTeacherCountAsync()
        {
            return await _unitOfWork.TeacherRepository.GetCountAsync();
        }

        public async Task SignUpUserAsync(TeacherSignUpDTO request)
        {
            Teacher teacher;
            User user;

            try
            {
                user = ExtractUser(request);
                User? existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(user.Username);

                if (existingUser != null)
                {
                    throw new EntityAlreadyExistsException("User", "User with username " +
                        existingUser.Username + " already exists");
                }

                user.Password = EncryptionUtil.Encrypt(user.Password);
                await _unitOfWork.UserRepository.AddAsync(user);

                teacher = ExtractTeacher(request);
                if (await _unitOfWork.TeacherRepository.GetByPhoneNumberAsync(teacher.PhoneNumber) is not null)
                {
                    throw new EntityAlreadyExistsException("Teacher", "Teacher with phone number " +
                        teacher.PhoneNumber + " already exists");
                }

                await _unitOfWork.TeacherRepository.AddAsync(teacher);
                user.Teacher = teacher;
                //teacher.User = user; EF manages the other-end of the realationship since both entities are attached

                await _unitOfWork.SaveAsync();
                _logger.LogInformation("{Message}", "Teacher: " + teacher + " signed up successfully.");  // ToDo toString in Teacher
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        private User ExtractUser(TeacherSignUpDTO signUpDTO)
        {
            return new User()
            {
                Username = signUpDTO.Username!,
                Password = signUpDTO.Password!,
                Firstname = signUpDTO.Firstname!,
                Lastname = signUpDTO.Lastname!,
                UserRole = signUpDTO.UserRole
            };
        }

        private Teacher ExtractTeacher(TeacherSignUpDTO signUpDTO)
        {
            return new Teacher()
            {
                PhoneNumber = signUpDTO.PhoneNumber,
                Institution = signUpDTO.Institution!
            };
        }
    }
}
