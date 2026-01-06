using FluentValidation;

using simple_crud.Library.Models.DTOs;

namespace simple_crud.Api.Validators;

public class PostValidator : AbstractValidator<PostDto>
{
    public PostValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("Es requerido el titulo del post.");

        RuleFor(x => x.Titulo)
            .Must(titulo => !titulo.Contains('-'))
            .WithMessage("El título no debe contener el carácter '-'");
    }
}
