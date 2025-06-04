use StoredProcDb;
go

create procedure spSearchEmployees
@FirstName NVARCHAR(100),
@LastName NVARCHAR(100),
@Gender NVARCHAR(50),
@Salary int
as
begin
    select * from Employees WHERE
    (FirstName = @FirstName or @FirstName is null) and
    (LastName = @LastName or @LastName is null) and
    (Gender = @Gender or @Gender is null) and
    (@Salary = @Salary or @Salary is null)
end;
go