using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Persistence.Data;

namespace Persistence.Repositories.Games
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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
                var game = await context.Games.FindAsync(request.Id);
                if(game == null)
                {
                    return null;
                }

                context.Games.Remove(game);
                var result = await context.SaveChangesAsync() > 0;

                if(!result)
                {
                    return Result<Unit>.Failure("Couldnt delete game");
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}