namespace ApiDeEscola.Models.TempModels
{
    public class UserAndEmployModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Emaíl { get; set; }
        public string password { get; set; }
        public EmploymentModel Employment { get; set; }

        public UserAndEmployModel()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
