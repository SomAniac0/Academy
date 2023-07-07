using Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#region Introduction
//  How basic Linq function works

// LINQ simplifies this situation by offering a consistent model for working with data across
// various kinds of data sources and formats. In a LINQ query, you are always working with objects.
// You use the same basic coding patterns to query and transform data in XML documents,
// SQL databases, ADO.NET Datasets, .NET collections,
// and any other format for which a LINQ provider is available.

List<Employee> employeeList = GenerateData.GetEmployees();
List<Department> departmentList = GenerateData.GetDepartments();

// Based on Filter extension fuction and lambda expression every generic type data is filterable
var filteredEmployees = employeeList.Filter(e => e.IsManager == true);

// Basicly your linq expression is beeing compiled into generic classes, and Expression trees.
// Always be mindfull which linq function you use, because there are big performance differences if
// you choose a wrong one or you implement it in an unoptimized way.
// Some example will be at performance region.

#endregion Introduction

#region Syntax difference
// Choose Query Syntax IF possible for easy readability otherwise stick to function syntax
// Reset Data
employeeList = GenerateData.GetEmployees();
departmentList = GenerateData.GetDepartments();

// LINQ Query Syntax lazy loading
var queryLazyResult = from emp in employeeList
             where emp.IsManager == true
             select emp.FirstName;

// LINQ Query Syntax Eager loading
var queryEagerResult = (from emp in employeeList
                  where emp.IsManager == true
                  select emp.FirstName).ToList();

// LINQ Method Syntax lazy loading
var functionLazyResult = employeeList.Where(e => e.IsManager == true);

// LINQ Method Syntax Eager loading
var functionEagerResult = employeeList.Where(e => e.IsManager == true).ToList();

// Test Employee added for lazy-eager loading
employeeList.Add(new Employee() { IsManager = true});

var querylazyCount = queryLazyResult.Count();               //8
var functionLazyCount = functionLazyResult.Count();         //8  
var queryEagerCount = queryEagerResult.Count();             //7
var functionEagerCount = functionEagerResult.Count();       //7

#endregion Syntax difference

#region Linq examples

// Reset Data
employeeList = GenerateData.GetEmployees();
departmentList = GenerateData.GetDepartments();

// Most important queries

// Return an Employee or throw an exception
Employee? firstResult1 = employeeList.First();
Employee? firstResult2 = employeeList.First(e => e.Id == 3);

// Return an Employee or default value
Employee? firstOrDefaultResult1 = employeeList.FirstOrDefault();
Employee? firstOrDefaultResult2 = employeeList.FirstOrDefault(e => e.Id == 3);

// Return an Employee type if there is only one in the collection otherwise throws an exception
Employee? singleResult = employeeList.Single(e => e.Id == 3);

// Return an Employee type if there is only one in the collection otherwise return default value
Employee? singleOrDefaultResult = employeeList.SingleOrDefault(e => e.Id == 3);

// Return TRUE if it has atleast one entry and it contains the specified element
bool anyResult = employeeList.Any();
bool anyResult2 = employeeList.Any(e => e.Id == 3);

// The Select() method helps in projecting or mapping each element of the collection into a new sequence.
// If the elements of the collection are complex types (such as classes) consisting of properties,
// the method can be used to project one or more specific properties of
// each element in the collection into a new collection of elements.

// Function syntax || anonymus return || simple reduction
var reducedEmployees1 = employeeList.Select(x => new
{
    x.Id,
    x.FirstName,
    x.LastName
});

// Function syntax || declared Type return || simple reduction
var reducedEmployees2 = employeeList.Select(x =>
    new ReducedEmploeyee () 
    {
        Id = x.Id,
        FirstName = x.FirstName,
        LastName = x.LastName
    });


// Query syntax || Anonymus return || simple reduction
var reducedEmployees = 
    from e in employeeList 
    select new 
    { 
        e.Id,
        e.FirstName,
        e.LastName 
    };

// Query syntax || Anonymus return || Inner join
var query =
   from emp in employeeList
   join dep in departmentList 
   on emp.DepartmentId equals dep.Id
   select new 
   {
       EmployeeName = emp.FirstName + " " + emp.LastName,
       DepartmentName = dep.Name
   };

// Query syntax || Anonymus return || Left Outer join
// There is an extra 13 th object with null properties
var result = from emp in employeeList
             join dep in departmentList
             on emp.DepartmentId equals dep.Id into tempstorage
             from dx in tempstorage.DefaultIfEmpty()
             select new
             {
                 EmployeeId = emp.Id,
                 EmployeeName = emp.FirstName + " " + emp.LastName,
                 Department = (dx != null) ? dx.Name : "NULL",
                 Description = (dx != null) ? dx.Description : "NULL"
             };

// WHERE function syntax
IEnumerable<Employee>? whereResult1 = employeeList.Where(e => e.Id % 2 == 0);

// WHERE query syntax 
IEnumerable<Employee>? whereResult2 = from emp in employeeList
                     where emp.Id % 2 == 0
                     select emp;

// Chaining example
var result11 = employeeList.Where(e => e.Id % 2 == 0)
  .Select(e => new { e.Id, e.FirstName, e.LastName });


// OrderBy function syntax || ascending order
IOrderedEnumerable<Employee> orderByResult1 = employeeList.OrderBy(e => e.Id)
  .ThenBy(e => e.FirstName).ThenBy(e => e.LastName);

// OrderBy query syntax || ascending order
IOrderedEnumerable<Employee> orderByResult2 = from emp in employeeList
                                     orderby emp.Id, emp.FirstName, emp.LastName
                                     select emp;

// Converting to abstract types
var convertedToAbstractType = orderByResult2.ToList();
var convertedToAbstractType2 = orderByResult2.ToArray();

Dictionary<int, string> emploeyeeDictionary = employeeList
      .ToDictionary(e => e.Id, value => value.FirstName);


#endregion Linq examples

#region IQueryable<T> vs IEnumerable<T>

// IQueryable<T> extends IEnumerable<T>
// IEnumerable<T> interface is useful when your collection is loaded in memory using LINQ
// or Entity framework and you want to apply filter on the collection.

// IQueryable<T>: best suits for remote data source,
// like a database or web service (or remote queries).
// IQueryable is a very powerful feature that enables a
// variety of interesting deferred execution scenarios
// (like paging and composition based queries).

// With simple words IEnumerable executes the query then filters it on client side,
// IQueryable does the filtering on server side.

IQueryable<Employee> MethodSyntax = employeeList.AsQueryable().Where(emp => emp.IsManager == true); 
IEnumerable<Employee> iEnumerable = employeeList.Where(emp => emp.IsManager == true);



#endregion IQueryable<T> vs IEnumerable<T>

#region Performance


MeasureThisFunction(Where, employeeList);
MeasureThisFunction(Any, employeeList);
MeasureThisFunction(AnyEmpty, employeeList);
MeasureThisFunction(Count, employeeList);
MeasureThisFunction(Let, employeeList);
MeasureThisFunction(Let2, employeeList);
MeasureThisFunction(GroupBy, employeeList);
MeasureThisFunction(LastLinq, employeeList);
MeasureThisFunction(Last, employeeList);


//          ANY VS WHERE
static void Where(List<Employee> employeeList)
{
    var x = employeeList.Where(e => e.IsManager == true);
}
static void Any(List<Employee> employeeList)
{
   
    var x = employeeList.Any(e => e.IsManager == true);
}

//         Any VS Count
static void AnyEmpty(List<Employee> employeeList)
{
    var x = employeeList.Any();
}
static void Count(List<Employee> employeeList)
{
    var x = employeeList.Count() > 0;
}

//  LET || Using let in quarry  might be trivial to remove code
//  dupplication but in this case it perform poorly
//  (for more information compile the code int 3.0).
static void Let(List<Employee> employeeList)
{
    string[] domestic = new string[] { "Bober" };
    string[] domestic2 = new string[] { "Viktor" };
    var x = (
        from emp in employeeList
        where domestic.Contains(emp.FirstName) ||
        domestic2.Contains(emp.FirstName) select emp).ToList(); 
}
static void Let2(List<Employee> employeeList)
{
    string[] domestic = new string[] { "Bober" };
    string[] domestic2 = new string[] { "Viktor" };
    var x = (
        from emp in employeeList
        let n = emp.LastName
        where domestic.Contains(n) ||
        domestic2.Contains(n)
        select emp).ToList();
}

// GroupBy performs really quick thanks to hasmapping
static void GroupBy(List<Employee> employeeList)
{
    var results = employeeList.GroupBy(
    emp => emp.Id,
    emp => emp.FirstName,
    (key, g) => new { PersonId = key, FirstName = g.ToList() });
}


// Last || Indexing approach is faster then LINQ Last()
// Interface casting check in Last() is expensive. (VSD Virtual Stub Dispatch)
static void LastLinq(List<Employee> employeeList)
{
    var results = employeeList.Last();
}
static void Last(List<Employee> employeeList)
{
    var results = employeeList[employeeList.Count - 1];
}

// First or Default improvable with custom classes which reduce
// heap memory allocations and operate on stack (TSOURCE)
// only usefull if mainly 1 or 2 type is prefered. 
/// SUPER LINQ OR HYPER LINQ !!!!

#endregion

#region BasicMeasure
static void MeasureThisFunction(Action<List<Employee>> action, List<Employee> employeeList)
{

    for (int i = 0; i < 100_000; i++)
    {
        action(employeeList);
    }

    var timer = new Stopwatch();
    timer.Start();

    for (int j = 0; j < 10; j++)
    {
        for (int i = 0; i < 100_000; i++)
        {
            action(employeeList);
        }
    }
    
    var name = action.Method.Name;
    timer.Stop();

    //Console.WriteLine("Time elapsed (s): {0}", timer.Elapsed.TotalSeconds);
    Console.WriteLine("Function: {0} Time elapsed (ms): {1}", name, timer.Elapsed.TotalMilliseconds / 10);
    //Console.WriteLine("Time elapsed (ns): {0}", timer.Elapsed.TotalMilliseconds * 1000000);
}
#endregion BasicMeasure
Console.ReadKey();
