using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain.Models;
using MediatR;
using Persistence.Data;

namespace Application.Games
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public GameType Type { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext context;

            public Handler(DataContext context)
            {
                this.context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // var game = new GameFactory().CreateGame(request.Type) as Game;
                // await context.Games.AddAsync(game);
                // var result = await context.SaveChangesAsync() > 0;

                // if(result)
                // {
                    return Result<Unit>.Success(Unit.Value);
                // }

                return Result<Unit>.Failure("Couldnt create game");
            }
        }
    }
}