namespace PWA_GREMIO_API.Core.Entities.Users
{
    public class PersonaGrupoFamiliar : Auditable<int?>
    {
        public required int GrupoFamiliarId { get; set; }
        public required int DNI { get; set; }

        public required string Name { get; set; }

        public required string LastName { get; set; }

        public required int Age { get; set; }
        public required bool IsAffiliate { get; set; }
        public required bool IsBeneficiaryOfMedical { get; set; }


    }
}