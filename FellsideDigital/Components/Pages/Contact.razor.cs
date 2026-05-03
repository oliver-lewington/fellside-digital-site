using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace FellsideDigital.Components.Pages;

public partial class Contact : ComponentBase
{
    private readonly string[] _steps = ["Your details", "About you", "Your message"];
    private int _currentStep;

    private void NextStep() { if (_currentStep < _steps.Length - 1) _currentStep++; }
    private void PreviousStep() { if (_currentStep > 0) _currentStep--; }
    private void TryGoToStep(int index) { if (index <= _currentStep) _currentStep = index; }

    private ContactFormModel _model = new();
    private bool _sending;
    private bool _submitted;

    private async Task HandleSubmit()
    {
        _sending = true;
        StateHasChanged();
        await Task.Delay(1200); // TODO: replace with real email service call
        _sending = false;
        _submitted = true;
    }

    private const string InputClass =
        "w-full rounded-xl px-4 py-2.5 text-sm " +
        "bg-slate-50 dark:bg-neutral-800 " +
        "ring-1 ring-slate-200 dark:ring-white/10 " +
        "text-slate-900 dark:text-neutral-100 " +
        "placeholder:text-slate-400 dark:placeholder:text-neutral-500 " +
        "focus:outline-none focus:ring-2 focus:ring-indigo-400/50 dark:focus:ring-orange-400/50 transition";

    private sealed class ContactFormModel
    {
        [Required(ErrorMessage = "Please enter your name.")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? Company { get; set; }
        public string? Budget { get; set; }
        public string? HowHeard { get; set; }

        [Required(ErrorMessage = "Please select a service.")]
        public string ServiceType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please tell us about your project.")]
        [MinLength(10, ErrorMessage = "Please give us a bit more detail (min 10 characters).")]
        public string Message { get; set; } = string.Empty;
    }
}
