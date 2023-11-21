namespace EmsApi.Dtos
{
    public class LoginResponseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string Token { get; set; }
    }
}
