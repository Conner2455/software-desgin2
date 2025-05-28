using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Porter_ChinookMusicApp.Data;

namespace Porter_ChinookMusicApp.Services;

public class SeedingService
{
    private readonly ApplicationDbContext _context;
    public SeedingService(ApplicationDbContext context)
    {
        _context = context;
    }
    public  SeedDatabase(){}
}