using Microsoft.AspNetCore.Authorization;

namespace WebApp_UnderTheHood.Authorization
{
    public class HRManagerProbationRequirmentHandler : AuthorizationHandler<HRManagerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
        {
            if(!context.User.HasClaim(x => x.Type == "EmploymentData"))
            {
                return Task.CompletedTask;
            }

            var employmentDate = context.User.FindFirst(x => x.Type == "EmploymentData")?.Value;

            if (DateTime.TryParse(employmentDate, out DateTime employmentData))
            {
                var period = DateTime.Now - employmentData;
                if(period.Days > 30 * requirement.ProbationMonths)
                {
                    context.Succeed(requirement);
                }
            }

           return Task.CompletedTask;
        }
    }
}
