namespace EHR_Application
{
    internal class User
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDoctor { get; set; }
    }
}