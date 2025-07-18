namespace PWA_GREMIO_API.Core.Entities.Users
{
    public class UserData : Auditable<int?>
    {
        public int? UserAuthId { get; set; }
        public  int DNI { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        
        public string? Category { get; set; }

        public  DateOnly BirthDate { get; set; }

        public List<PersonaGrupoFamiliar>? FamilyGroupPersons { get; set; }
        

    }
}
