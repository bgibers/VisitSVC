namespace Visit.Service.Models.Requests
{
    public class UpdateUserInfoRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Title { get; set; }
        public string Education { get; set; }
        public string BirthLocation { get; set; }
        public string ResidenceLocation { get; set; }
    }
}