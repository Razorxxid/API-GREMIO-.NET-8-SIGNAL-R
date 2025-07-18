﻿namespace PWA_GREMIO_API.Core.Dtos
{
    public class SendAnnoucementDto
    {

        public required string Title { get; set; }
        public required string Text { get; set; }

        public required string ImageUrl { get; set; }

        public required List<int?> DestinationGroupsIds { get; set; }

    }
   
}
