﻿using MaratukAdmin.Entities.Global;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class SeasonRepository : MainRepository<Season>, ISeasonRepository
    {
        public SeasonRepository(MaratukDbContext context) : base(context)
        {
        }
    }
}
