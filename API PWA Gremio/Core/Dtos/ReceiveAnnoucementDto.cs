namespace PWA_GREMIO_API.Core.Dtos
{
    public class ReceiveAnnoucementDto
    {
        public required int? Id { get; set; }

        public required string Title { get; set; }
        public required string Text { get; set; }

        public required string ImageUrl { get; set; }


    }
   
}
