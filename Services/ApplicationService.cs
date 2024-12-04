using AutoMapper;
using SchoolApp.Repositories;

namespace SchoolApp.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _ulogger;
        private readonly ILogger<TeacherService> _tlogger;
        private readonly ILogger<StudentService> _slogger;

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _ulogger = logger;
        }

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TeacherService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tlogger = logger;
        }

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StudentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _slogger = logger;
        }

        public UserService UserService => new(_unitOfWork, _mapper, _ulogger);

        public TeacherService TeacherService => new(_unitOfWork, _mapper, _tlogger);

        public StudentService StudentService => new(_unitOfWork, _mapper, _slogger);
    }
}
