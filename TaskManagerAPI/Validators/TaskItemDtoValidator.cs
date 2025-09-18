using FluentValidation;
using TaskManagerAPI.Dtos;

namespace TaskManagerAPI.Validators
{
    public class TaskItemDtoValidator : AbstractValidator<TaskItemDto>
    {
        public TaskItemDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MinimumLength(2).MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(2000);
        }
    }
}


