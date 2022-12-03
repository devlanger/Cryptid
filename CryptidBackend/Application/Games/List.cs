using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Games
{
    public class List 
    {
        public class Query : IRequest<Result<List<GameDto>>> {}

        public class Handler : IRequestHandler<Query, Result<List<GameDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<GameDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var games = await _context.Games
                    .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<GameDto>>.Success(games);
            }
        }
    }
}