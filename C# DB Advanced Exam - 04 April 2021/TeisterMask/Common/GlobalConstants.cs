namespace TeisterMask.Shared
{
    public static class GlobalConstants
    {
        //Employee
        public const int EmployeeUsernameMinLength = 3;
        public const int EmployeeUsernameMaxLength = 40;
        public const int EmployeePhoneMaxLength = 12;
        public const string EmployeeUsernameReggex = @"^[A-Za-z0-9]+$";
        public const string EmployeePhoneReggex = @"^\d{3}-\d{3}-\d{4}$";


        //Project
        public const int ProjectNameMinLength = 2;
        public const int ProjectNameMaxLength = 40;

        //Task
        public const int TaskNameMinLength = 2;
        public const int TaskNameMaxLength = 40;

        public const int TaskExecutionTypeMaxLenght = 3;
        public const int TaskExecutionTypeMinLenght = 0;
        public const int TaskLabelTypeMaxLenght = 4;
        public const int TaskLabelTypeMinLenght = 0; 
 



    }
}
