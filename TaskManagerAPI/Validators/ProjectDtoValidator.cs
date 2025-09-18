using FluentValidation;
using TaskManagerAPI.Dtos;

namespace TaskManagerAPI.Validators
{
    public class ProjectDtoValidator : AbstractValidator<ProjectDto>
    {
        public ProjectDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }
}


