namespace SchoolApp.Repositories
{
    public interface IUnitOfWork
    {
        //UserRepository
        TeacherRepository TeacherRepository { get; }
        StudentRepository StudentRepository { get; }
        CourseRepository CourseRepository { get; }

        Task<bool> SaveAsync();
    }
}
