using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Keyless]
    public partial class GetTopFiveFavoriteList
    {
        public int Id { get; set; }
    }
}
