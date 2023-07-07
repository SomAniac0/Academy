using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class GenerateData
    {
        public static List<Employee> GetEmployees()
        {
            List<Employee> Employees = new List<Employee>()
            {
                new Employee()
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Clark",
                    AnnualSalary = 10000,
                    IsManager = true,
                    DepartmentId = 1,
                },
                 new Employee()
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Wick",
                    AnnualSalary = 20000,
                    IsManager = false,
                    DepartmentId = 1,
                },
                 new Employee()
                {
                    Id = 3,
                    FirstName = "Scarlett",
                    LastName = "Johanson",
                    AnnualSalary = 30000,
                    IsManager = true,
                    DepartmentId = 1,
                },
                new Employee()
                {
                    Id = 4,
                    FirstName = "Emilia",
                    LastName = "Clarke",
                    AnnualSalary = 40000,
                    IsManager = false,
                    DepartmentId = 2,
                },
                new Employee()
                {
                    Id = 5,
                    FirstName = "Bober",
                    LastName = "Smith",
                    AnnualSalary = 50000,
                    IsManager = true,
                    DepartmentId = 2,
                },
                new Employee()
                {
                    Id = 6,
                    FirstName = "Viktor",
                    LastName = "Orban",
                    AnnualSalary = 10000,
                    IsManager = false,
                    DepartmentId = 3,
                },
                new Employee()
                {
                    Id = 7,
                    FirstName = "Soma",
                    LastName = "Mayer",
                    AnnualSalary = 2000000,
                    IsManager = true,
                    DepartmentId = 3,
                },
                new Employee()
                {
                    Id = 8,
                    FirstName = "Yi",
                    LastName = "Chu",
                    AnnualSalary = 30000,
                    IsManager = false,
                    DepartmentId = 3,
                },
                new Employee()
                {
                    Id = 9,
                    FirstName = "Jyrki",
                    LastName = "Sota",
                    AnnualSalary = 40000,
                    IsManager = true,
                    DepartmentId = 4,
                },
                new Employee()
                {
                    Id = 10,
                    FirstName = "Sarika",
                    LastName = "Gesine",
                    AnnualSalary = 50000,
                    IsManager = false,
                    DepartmentId = 4,
                },
                new Employee()
                {
                    Id = 11,
                    FirstName = "John",
                    LastName = "Doesmond",
                    AnnualSalary = 100000,
                    IsManager = true,
                    DepartmentId = 4,
                },
                new Employee()
                {
                    Id = 12,
                    FirstName = "Ludvik",
                    LastName = "Pòl",
                    AnnualSalary = 100000,
                    IsManager = false,
                    DepartmentId = 4,
                },
                new Employee()
                {
                    Id = 13,
                    FirstName = "Colobert",
                    LastName = "Theodorus",
                    AnnualSalary = 100000,
                    IsManager = true,
                    DepartmentId = 5,
                }
            };
            return Employees;
        }

        public static List<Department> GetDepartments()
        {
            List<Department> Departments = new List<Department>()
            {
                new Department()
                {
                    Id = 1,
                    Name = "Koka kola",
                    ShortName = "kola",
                    Description = "Drink"
                },
                 new Department()
                {
                    Id = 2,
                    Name = "Tesla",
                    ShortName = "T",
                    Description = "Car&Space"
                },
                 new Department()
                {
                    Id = 3,
                    Name = "Microsoft Corporation",
                    ShortName = "Microsoft",
                    Description = "Technology corporation"
                },
                new Department()
                {
                    Id = 4,
                    Name = "RandomCompany",
                    ShortName = "Random",
                    Description = "Food&Drink"
                }
            };
            return Departments;
        }
    }
}
