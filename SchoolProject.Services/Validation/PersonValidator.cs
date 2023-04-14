using FluentValidation;
using SchoolProject.Data;
using SchoolProject.Models.Entities;
using SchoolProject.Models.Entities.Enums;

namespace SchoolProject.Services.Validation
{
    public class PersonValidator : AbstractValidator<Person>
    {
        private readonly DataContext _dataContext;

        public PersonValidator(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(person => person.FirstName)
                .NotNull().WithMessage("Please enter a first name...")
                .NotEmpty().WithMessage("Please enter a first name...")
                .Length(3, 20).WithMessage("First name must be between 3 and 20 characters.");

            RuleFor(person => person.LastName)
                .NotNull().WithMessage("Please enter a last name...")
                .NotEmpty().WithMessage("Please enter a last name...")
                .Length(3, 30).WithMessage("Last name must be between 3 and 30 characters.");

            RuleFor(person => person.UserType)
                .Must(IsValidUserType).WithMessage("Please select a valid user type...")
                .IsInEnum();

            RuleFor(person => person.SchoolID)
                .NotNull().WithMessage("Please enter an ID")
                .NotEmpty().WithMessage("Please enter an ID");             

            RuleFor(person => person.DateOfBirth)
                .Null()
                .When(person => person.UserType == UserType.Teacher)
                .NotNull()
                .When(person => person.UserType == UserType.Pupil)
                .NotEmpty()
                .When(person => person.UserType == UserType.Pupil)
                .Must(date => date >= new DateTime(2005, 1, 1) && date <= new DateTime(2018, 12, 31))
                .When(person => person.UserType == UserType.Pupil)
                .WithMessage("Age of pupil must be between 5 and 18 years old. Please enter a valid date of birth.");

            RuleFor(person => person.YearGroup)
                .Null()
                .When(person => person.UserType == UserType.Teacher)
                .NotNull()
                .When(person => person.UserType == UserType.Pupil)
                .NotEmpty()
                .When(person => person.UserType == UserType.Pupil)
                .InclusiveBetween(1, 13)
                .When(person => person.UserType == UserType.Pupil)
                .WithMessage("Please select a year group between 1 and 13.");
        }

        private bool IsValidUserType(UserType userType)
        {
            foreach (UserType type in Enum.GetValues(typeof(UserType)))
            {
                if (userType == type) return true;
            }
            return false;
        }
    }
}
