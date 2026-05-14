using System;
using System.Collections.Generic;
using System.Text;
using Structure.Repository.Interfaces;
using Structure.Data.Entities;


namespace Structure.Repository;

public interface IStudentRepository
    : IRepository<Data.Entities.Student>
{
    Task<Data.Entities.Student?> GetByEmailAsync(
        string email);
    Task<bool> EmailExistsAsync(string email);
}
