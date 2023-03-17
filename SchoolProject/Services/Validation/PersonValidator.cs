using FluentValidation;
using SchoolProject.BL.Models;
using SchoolProject.BL.Models.Enums;
using System.Globalization;

namespace SchoolProject.API.Services.Validation
{
    public class PersonValidator : AbstractValidator<Person>
    {
        private readonly DataContext _dataContext;

        public PersonValidator(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(person => person.First_name)
                .NotNull().WithMessage("Please enter a first name...")
                .NotEmpty().WithMessage("Please enter a first name...")
                .Length(3, 20).WithMessage("First name must be between 3 and 20 characters.");

            RuleFor(person => person.Last_name)
                .NotNull().WithMessage("Please enter a last name...")
                .NotEmpty().WithMessage("Please enter a last name...")
                .Length(3, 30).WithMessage("Last name must be between 3 and 30 characters.");

            RuleFor(person => person.User_type)
                .NotNull().WithMessage("Please select a user type...")
                .NotEmpty().WithMessage("Please select a user type...")
                .Must(IsValidUserType).WithMessage("Please select a valid user type...")
                .IsInEnum();

            RuleFor(person => person.School_ID)
                .NotNull().WithMessage("Please enter a valid ID")
                .NotEmpty().WithMessage("Please enter a valid ID")
                .Must(IsValidSchoolID).WithMessage("Please enter a valid School ID");

            RuleFor(person => person.Date_of_birth)
                .Null()
                .When(person => person.User_type == UserType.Teacher)
                .NotNull()
                .When(person => person.User_type == UserType.Pupil)
                .NotEmpty()
                .When(person => person.User_type == UserType.Pupil)
                .Must(date => date >= new DateTime(2005, 1, 1) && date <= new DateTime(2018, 12, 31))
                .WithMessage("Age of pupil must be between 5 and 18 years old. Please enter a valid date of birth.");

            RuleFor(person => person.Year_group)
                .Null()
                .When(person => person.User_type == UserType.Teacher)
                .NotNull()
                .When(person => person.User_type == UserType.Pupil)
                .NotEmpty()
                .When(person => person.User_type == UserType.Pupil)
                .InclusiveBetween(1, 13)
                .WithMessage("Please select a year group between 1 and 13.");
        }

        private bool IsValidUserType(UserType userType)
        {
            return _dataContext.Set<Person>()
                               .Any(p => p.User_type == userType);
        }

        private bool IsValidSchoolID(Guid schoolID)
        {
            return _dataContext.Set<School>()
                               .Any(s => s.School_ID == schoolID);
        }

    }
}
