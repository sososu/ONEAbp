namespace MyCompanyName.MyProjectName.Settings
{
    public class IdentitySettingDto
    {
        public Password Password { get; set; }
    }

    public class Password
    {
        public int RequiredLength { get; set; }
        public int RequiredUniqueChars { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireDigit { get; set; }
        public string DefaultPassword { get; set; } 
    }

    public class Lockout
    {
        public bool AllowedForNewUsers { get; set; }
        public int LockoutDuration { get; set; }
        public int MaxFailedAccessAttempts { get; set; }
    }

    public class SignIn
    {
        public bool RequireConfirmedEmail { get; set; }
        public bool EnablePhoneNumberConfirmation { get; set; }
        public bool RequireConfirmedPhoneNumber { get; set; }
    }

    public class User
    {
        public bool IsUserNameUpdateEnabled { get; set; }
        public bool IsEmailUpdateEnabled { get; set; }
    }
}
