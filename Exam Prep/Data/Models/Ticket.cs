﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Range(1.00, 100.00)]
        public decimal Price { get; set; }

        [Range(1, 10)]
        public sbyte RowNumber { get; set; }

        public int PlayId { get; set; }
        public Play Play { get; set; }
        
        public int TheatreId { get; set; }
        public Theatre Theatre { get; set; }
    }
}
