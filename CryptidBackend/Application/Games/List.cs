using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Games
{
    public class List 
    {
        public class Query : IRequest<Result<List<Game>>> {}

        public class Handler : IRequestHandler<Query, Result<List<Game>>>
        {
            private readonly DataContext _context;
            
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<Game>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var games = await _context.Games.ToListAsync();
                return Result<List<Game>>.Success(games);
            }
        }
    }
}