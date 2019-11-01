using chat.backend.Models;
using chat.backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace chat.backend.ContextExtensions
{
    public static class ContextExtensions
    {
        public static void AddOrUpdateToken(this ApplicationDbContex context, RefToken entity, RefToken token)
        {
            if (token == null)
            {
                context.RefreshTokens
                    .Add(entity);
            }
            else
            {

                context.RefreshTokens
                    .Update(entity);
            }
        }
    }
}
