using System.ComponentModel;

namespace UKParliament.CodeTest.Data.Models;

public enum EmployeeTypeEnum
{
    [Description("Employee")]
    Employee,

    [Description("Manager")]
    Manager,
}
