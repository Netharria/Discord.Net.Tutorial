﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Rank
    {
        public int Id { get; set; }
        public ulong RoleId { get; set; }
        public ulong ServerId { get; set; }
    }
}
