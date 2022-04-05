// EmployeeUpsertFilterAttribute.cs

var user = context.HttpContext.User;

employee.Name =
  user.FindFirstValue(_auth0Options.NameClaim);
employee.Email =
  user.FindFirstValue(_auth0Options.EmailClaim);