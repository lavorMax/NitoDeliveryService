﻿using System.Collections.Generic;

namespace NitoDeliveryService.ManagementPortal.Models.DTOs
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public IEnumerable<ClientResponsibleDto> Responsibles { get; set; }
        public IEnumerable<SlotDto> Slots { get; set; }
    }
}
