using API.Dto;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Games
{
    public class ListForUser
    {
        public class Query : IRequest<Result<List<GameDto>>>
        {
            public string UserId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<GameDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<List<GameDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<GameDto> list = await context.Games
                    .Where(g => g.Participants
                    .FirstOrDefault(p => p.AppUserId == request.UserId) != null)
                    .ProjectTo<GameDto>(mapper.ConfigurationProvider)
                    .ToListAsync();
                    

                return Result<List<GameDto>>.Success(list);
            }
        }
    }
}
