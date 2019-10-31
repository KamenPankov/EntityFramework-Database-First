using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            SoftUniContext softUniContext = new SoftUniContext();

            Console.WriteLine(RemoveTown(softUniContext));
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            //IQueryable<Employee> employees = from employee in context.Employees
            //                                 orderby employee.EmployeeId
            //                                 select employee;

            //foreach (Employee item in employees)
            //{
            //    stringBuilder.AppendLine(string.Format($"{item.FirstName} {item.LastName} {item.MiddleName} {item.JobTitle} {item.Salary:f2}"));
            //}
            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    Name = string.Join(" ", e.FirstName, e.LastName, e.MiddleName),
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                });

            foreach (var item in employees)
            {
                stringBuilder.AppendLine(string.Format($"{item.Name} {item.JobTitle} {item.Salary:f2}"));
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            //IQueryable<Employee> employees = from employee in context.Employees
            //                                 where employee.Salary > 50000
            //                                 orderby employee.FirstName
            //                                 select employee;

            //foreach (Employee item in employees)
            //{
            //    stringBuilder.AppendLine($"{item.FirstName} - {item.Salary:f2}");
            //}

            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FirstName);

            foreach (var item in employees)
            {
                stringBuilder.AppendLine($"{item.FirstName} - {item.Salary:f2}");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            //IQueryable<Employee> employees = from employee in context.Employees
            //                                 where employee.Department.Name == "Research and Development"
            //                                 orderby employee.Salary, employee.FirstName descending
            //                                 select employee;

            //foreach (Employee item in employees)
            //{
            //    stringBuilder.AppendLine($"{item.FirstName} {item.LastName} from Research and Development - ${item.Salary:f2}");
            //}

            var employees = context.Employees
                .Where(d => d.Department.Name == "Research and Development")
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    DepartmentName = e.Department.Name,
                    Salary = e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName);

            foreach (var item in employees)
            {
                stringBuilder.AppendLine($"{item.FirstName} {item.LastName} from Research and Development - ${item.Salary:f2}");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string employeeLastName = "Nakov";

            Address newAddress = new Address { AddressText = "Vitoshka 15", TownId = 4 };
            context.Addresses.Add(newAddress);


            Employee employee = context.Employees.FirstOrDefault(e => e.LastName == employeeLastName);
            if (employee != null)
            {
                employee.Address = newAddress;
            }

            context.SaveChanges();


            //IQueryable<Address> employeesAddress = from empl in context.Employees
            //                                 orderby empl.AddressId descending
            //                                 select empl.Address;
            //int countLines = 1;

            //foreach (Address item in employeesAddress)
            //{                
            //    stringBuilder.AppendLine(item.AddressText);
            //    if (countLines == 10)
            //    {
            //        break;
            //    }

            //    countLines++;
            //}

            var employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => e.Address.AddressText)
                .Take(10);

            foreach (var item in employees)
            {
                stringBuilder.AppendLine(item);
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            //var employees = from ep in context.EmployeesProjects
            //                join e in context.Employees
            //                on ep.EmployeeId equals e.EmployeeId
            //                join em in context.Employees
            //                on e.ManagerId equals em.EmployeeId
            //                join p in context.Projects
            //                on ep.ProjectId equals p.ProjectId
            //                where p.StartDate.Year >= 2001 && p.StartDate.Year <= 2003
            //                select new
            //                {
            //                    ep,
            //                    e,
            //                    em,
            //                    p
            //                } into elements
            //                group elements by elements.ep.Employee into grouped
            //                select grouped;

            //int countEmployees = 1;

            //foreach (var item in employees)
            //{
            //    stringBuilder.AppendLine($"{item.Key.FirstName} {item.Key.LastName} - Manager: {item.Key.Manager.FirstName} {item.Key.Manager.LastName}");

            //    foreach (var project in item.Key.EmployeesProjects)
            //    {
            //        string endDate;
            //        if (project.Project.EndDate == null)
            //        {
            //            endDate = "not finished";
            //        }
            //        else
            //        {
            //            endDate = ((DateTime)project.Project.EndDate).ToString("M/d/yyyy h:mm:ss tt");
            //        }
            //        stringBuilder.AppendLine($"--{project.Project.Name} - {project.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - {endDate}");
            //    }

            //    if (countEmployees == 10)
            //    {
            //        break;
            //    }

            //    countEmployees++;
            //}

            //var employees = context.Employees
            //    .Select(e => new
            //    {
            //        EmployeeName = string.Join(" ", e.FirstName, e.LastName),
            //        ManagerName = string.Join(" ", e.Manager.FirstName, e.Manager.LastName),
            //        EmployeesProjects = e.EmployeesProjects
            //                                    .Where(ep => ep.Project.StartDate.Year >= 2001 &&
            //                                                 ep.Project.StartDate.Year <= 2003)
            //                                    .Select(p => new
            //                                    {
            //                                        ProjectName = p.Project.Name,
            //                                        StartDate = p.Project.StartDate,
            //                                        EndDate = p.Project.EndDate
            //                                    })
            //    })
            //    .Take(10);

            var employees = context.Employees
                .Where(e => e.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 &&
                                                         p.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    EmployeeName = string.Join(" ", e.FirstName, e.LastName),
                    ManagerName = string.Join(" ", e.Manager.FirstName, e.Manager.LastName),
                    Projects = e.EmployeesProjects.Select(p => new
                    {
                        p.Project.Name,
                        p.Project.StartDate,
                        p.Project.EndDate
                    })
                })
                .Take(10);

            foreach (var item in employees)
            {
                stringBuilder.AppendLine($"{item.EmployeeName} - Manager: {item.ManagerName}");

                foreach (var project in item.Projects)
                {
                    string startDate = project.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                    string endDate = project.EndDate == null ? "not finished" : ((DateTime)project.EndDate).ToString("M/d/yyyy h:mm:ss tt");

                    stringBuilder.AppendLine($"--{project.Name} - {startDate} - {endDate}");
                }
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();


            //var query = (from a in context.Addresses
            //            join t in context.Towns
            //            on a.TownId equals t.TownId
            //            orderby a.Employees.Count descending, t.Name, a.AddressText
            //            select new { a, t, emplCount = a.Employees.Count() }).Take(10);

            //foreach (var item in query)
            //{
            //    stringBuilder.AppendLine($"{item.a.AddressText}, {item.t.Name} - {item.emplCount} emloyees");
            //}

            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count
                })
                .Take(10);

            foreach (var item in addresses)
            {
                stringBuilder.AppendLine($"{item.AddressText}, {item.TownName} - {item.EmployeeCount} employees");
            }
            
            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            //var employee147 = from ep in context.EmployeesProjects
            //                  join e in context.Employees on ep.EmployeeId equals e.EmployeeId
            //                  join p in context.Projects on ep.ProjectId equals p.ProjectId
            //                  where ep.EmployeeId == 147
            //                  select new
            //                  {
            //                      ep,
            //                      e,
            //                      p
            //                  } into elements group elements by elements.ep.Employee into grouped select grouped;

            //foreach (var item in employee147)
            //{
            //    stringBuilder.AppendLine($"{item.Key.FirstName} {item.Key.LastName} - {item.Key.JobTitle}");

            //    foreach (var project in item.Key.EmployeesProjects.OrderBy(p => p.Project.Name))
            //    {
            //        stringBuilder.AppendLine($"{project.Project.Name}");
            //    }
            //}

            var employee147 = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    EmployeeName = string.Join(" ", e.FirstName, e.LastName),
                    JobTitle = e.JobTitle,
                    ProjectNames = e.EmployeesProjects.OrderBy(p => p.Project.Name)
                                       .Select(p => p.Project.Name)
                });

            foreach (var item in employee147)
            {
                stringBuilder.AppendLine($"{item.EmployeeName} - {item.JobTitle}");

                foreach (var project in item.ProjectNames)
                {
                    stringBuilder.AppendLine(project);
                }
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerName = string.Join(" ", d.Manager.FirstName, d.Manager.LastName),
                    EmloyeesInfo = d.Employees
                                        .OrderBy(e => e.FirstName)
                                        .ThenBy(e => e.LastName)
                                        .Select(e => new
                                        {
                                            EmployeeName = string.Join(" ", e.FirstName, e.LastName),
                                            JobTitle = e.JobTitle
                                        })
                });

            foreach (var item in departments)
            {
                stringBuilder.AppendLine($"{item.DepartmentName} - {item.ManagerName}");

                foreach (var employee in item.EmloyeesInfo)
                {
                    stringBuilder.AppendLine($"{employee.EmployeeName} - {employee.JobTitle}");
                }
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate
                })
                .OrderBy(p => p.Name);

            foreach (var item in projects)
            {
                stringBuilder.AppendLine(item.Name)
                    .AppendLine(item.Description)
                    .AppendLine(item.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)

        {
            StringBuilder stringBuilder = new StringBuilder();

            var employeesToChangeSalary = context.Employees
                .Where(e => e.Department.Name == "Engineering" |
                            e.Department.Name == "Tool Design" |
                            e.Department.Name == "Marketing" |
                            e.Department.Name == "Information Services")
                 .Select(e => e);

            foreach (var item in employeesToChangeSalary)
            {
                item.Salary *= 1.12m;
            }


            context.SaveChanges();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering" |
                            e.Department.Name == "Tool Design" |
                            e.Department.Name == "Marketing" |
                            e.Department.Name == "Information Services")
                 .OrderBy(e => e.FirstName)
                 .ThenBy(e => e.LastName)
                 .Select(e => new
                 {
                     FullName = string.Join(" ", e.FirstName, e.LastName),
                     Salary = e.Salary
                 });

            foreach (var item in employees)
            {
                stringBuilder.AppendLine($"{item.FullName} (${item.Salary:f2})");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.FirstName.Substring(0, 2) == "Sa")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    FullName = string.Join(" ", e.FirstName, e.LastName),
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                });

            foreach (var item in employees)
            {
                stringBuilder.AppendLine($"{item.FullName} - {item.JobTitle} - (${item.Salary:f2})");
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var employeesReferenceProject = context.EmployeesProjects
                 .Where(ep => ep.ProjectId == 2)
                 .ToArray();

            context.EmployeesProjects.RemoveRange(employeesReferenceProject);

            var getProjectId2 = context.Projects.First(p => p.ProjectId == 2);
            if (getProjectId2 != null)
            {
                context.Projects.Remove(getProjectId2);
            }

            context.SaveChanges();

            var projects = context.Projects
                .Select(p => p.Name)
                .Take(10)
                .ToArray();

            foreach (var item in projects)
            {
                stringBuilder.AppendLine(item);
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            // get address ids in Seatle
            var addressesSeattle = context.Addresses
                .Where(a => a.Town.Name == "Seattle")
                .Select(a => a)
                .ToArray();

            int removedAddresses = addressesSeattle.Length;

            // set address id to null of employees
            var employeesInSeattle = context.Employees
                .Where(e => e.Address.Town.Name == "Seattle")
                .Select(e => e);

            foreach (var item in employeesInSeattle)
            {
                item.AddressId = null;
            }

            // delete addresses in Seattle
            context.Addresses.RemoveRange(addressesSeattle);

            // delete Seattle from towns
            var townSeattle = context.Towns.First(t => t.Name == "Seattle");
            if (townSeattle != null)
            {
                context.Towns.Remove(townSeattle);
            }

            context.SaveChanges();

            return $"{removedAddresses} addresses in Seattle were deleted";
        }
    }
}
