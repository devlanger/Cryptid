using System;
using System.Collections;
using System.Collections.Generic;

public class ActionsController
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }

        public static Result Success() => new Result() { IsSuccess = true };
        public static Result Failure(string error) => new Result() { Error = error };
    }

    public ActionsController.Result Execute(GameState state, GameAction action, CommandBase command)
    {
        var result = action.CanExecute(state, command);
        if (!result.IsSuccess)
        {
            return result;
        }

        action.Execute(state, command);
        return result;
    }
}
