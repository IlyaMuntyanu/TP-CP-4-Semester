using Microsoft.AspNetCore.Identity;

namespace TP_CP_5_Semester.Models;

public class User: IdentityUser
{
    public int Balance { get; set; }
}