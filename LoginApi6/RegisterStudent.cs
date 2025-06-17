namespace LoginApi6
{
    public class RegisterStudent
    {
       public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int IsActive { get; set; }     
    }
}
