namespace RoleSystem.Contracts.Enums
{
    public enum ErrorCode
    {
        UNKNOWN_ERROR = 0,

        FUNCTION_NOT_FOUND = 1,
        FUNCTION_NAME_REQUIRED = 2,
        FUNCTION_PARENT_CANNOT_BE_SAME = 3,

        ROLE_NOT_FOUND = 4,
        ROLE_NAME_REQUIRED = 5,
        ROLE_ALREADY_HAS_FUNCTION = 6,
        ROLE_DO_NOT_HAVE_FUNCTION = 7,
        ROLE_ALREADY_HAS_USER = 8,
        ROLE_DO_NOT_HAVE_USER = 9,
    }
}
