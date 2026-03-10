namespace FellsideDigital.Components.Home;

// ─── Record types ─────────────────────────────────────────────────────────────

public record Project(
    string Title,
    string Description,
    string[] Tags,
    string MockupImage,
    string Link);

public record Service(
    string Title,
    string Description,
    string IconSvg);

public record Testimonial(
    string Quote,
    string Name,
    string Role,
    string AvatarUrl);

public record Faq(
    string Question,
    string Answer);

// ─── Static data ──────────────────────────────────────────────────────────────

public static class HomeData
{
    public static readonly IReadOnlyList<Project> Projects = new[]
    {
        new Project(
            "Client Portal",
            "A bespoke project-tracking portal giving clients real-time visibility into milestones, tasks, and updates.",
            new[] { "Blazor", "Real-time", "Auth" },
            "/images/mockup-portal.png",
            "/work/portal"),

        new Project(
            "E-Commerce Platform",
            "A custom storefront with integrated payments, inventory management, and an admin dashboard built from the ground up.",
            new[] { "E-Commerce", "Stripe", "Dashboard" },
            "/images/mockup-ecommerce.png",
            "/work/ecommerce"),

        new Project(
            "Booking & Scheduling System",
            "Automated booking with calendar sync, Stripe payments, and email/SMS notifications for a hospitality business.",
            new[] { "Bookings", "Payments", "Notifications" },
            "/images/mockup-booking.png",
            "/work/booking"),

        new Project(
            "Internal Operations Dashboard",
            "A data-rich internal tool connecting multiple APIs and surfacing live KPIs and alerts for a growing SME.",
            new[] { "Dashboard", "APIs", "Reporting" },
            "/images/mockup-dashboard.png",
            "/work/dashboard"),
    };

    public static readonly IReadOnlyList<Service> Services = new[]
    {
        new Service(
            "Design",
            "Interfaces that are calm, purposeful, and easy to navigate. Every pixel considered, every interaction intentional.",
            """<svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M9.53 16.122a3 3 0 00-5.78 1.128 2.25 2.25 0 01-2.4 2.245 4.5 4.5 0 008.4-2.245c0-.399-.078-.78-.22-1.128zm0 0a15.998 15.998 0 003.388-1.62m-5.043-.025a15.994 15.994 0 011.622-3.395m3.42 3.42a15.995 15.995 0 004.764-4.648l3.876-5.814a1.151 1.151 0 00-1.597-1.597L14.146 6.32a15.996 15.996 0 00-4.649 4.763m3.42 3.42a6.776 6.776 0 00-3.42-3.42" /></svg>"""),

        new Service(
            "Develop",
            "Blazor / .NET 9 applications built for performance, security, and long-term maintainability. No shortcuts.",
            """<svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M17.25 6.75L22.5 12l-5.25 5.25m-10.5 0L1.5 12l5.25-5.25m7.5-3l-4.5 16.5" /></svg>"""),

        new Service(
            "Deploy",
            "CI/CD pipelines, custom domains, and hosting — configured and managed so you never think about infrastructure.",
            """<svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M12 16.5V9.75m0 0l3 3m-3-3l-3 3M6.75 19.5a4.5 4.5 0 01-1.41-8.775 5.25 5.25 0 0110.338-2.32 5.75 5.75 0 011.344 11.095" /></svg>"""),

        new Service(
            "Maintain",
            "Ongoing support, monitoring, updates, and performance optimisation — so your software keeps working hard.",
            """<svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M11.42 15.17L17.25 21A2.652 2.652 0 0021 17.25l-5.877-5.877M11.42 15.17l2.496-3.03c.317-.384.74-.626 1.208-.766M11.42 15.17l-4.655 5.653a2.548 2.548 0 11-3.586-3.586l5.654-4.654m5.57-4.765l-5.57 4.765m0 0L6.75 9.75M3 3l3.75 3.75" /></svg>"""),
    };

    public static readonly IReadOnlyList<Testimonial> Testimonials = new[]
    {
        new Testimonial(
            "The portal Fellside Digital built has completely changed how we communicate progress. Clients love having real-time visibility into their projects.",
            "Jane Cooper", "Director, Lakeland Consulting", ""),

        new Testimonial(
            "Professional, communicative, and technically excellent. The booking system is rock-solid and has saved us hours every single week.",
            "Tom Wright", "Owner, Eden Valley Stays", ""),

        new Testimonial(
            "We came to Fellside Digital with a complex brief and they delivered something beyond what we imagined. Clear design, fast, built to last.",
            "Sarah Hodgson", "Operations Manager, Northern Roots CIC", ""),
    };

    public static readonly IReadOnlyList<Faq> Faqs = new[]
    {
        new Faq(
            "Why choose Fellside Digital?",
            "You deal directly with the developers doing the work — no account managers, no hand-offs. We keep overhead low, which means better value and a more personal service throughout."),

        new Faq(
            "What kind of projects do you take on?",
            "We specialise in custom web applications, client portals, internal dashboards, e-commerce platforms, and bespoke software systems. We work with SMEs, agencies, and non-profits across the UK."),

        new Faq(
            "What technology stack do you use?",
            "We build primarily with Blazor and .NET 9, with Tailwind CSS for UI. For databases we typically use SQL Server or PostgreSQL. We deploy to Azure, AWS, or managed VPS depending on your needs."),

        new Faq(
            "How much does a project cost?",
            "A foundational website starts from around £450. Software-driven sites and bespoke systems are scoped individually. We always provide a clear, fixed quote before any work begins."),

        new Faq(
            "How long does a project take?",
            "Simpler sites can be delivered in 2–4 weeks. Complex software systems typically take 2–4 months. We agree a timeline upfront and keep you updated via your dedicated project portal."),

        new Faq(
            "Can I track progress during the build?",
            "Yes — every client gets access to our project portal with live milestones, task updates, and feedback threads. No more chasing emails for updates."),

        new Faq(
            "Do you offer ongoing support after launch?",
            "Absolutely. We offer retainer-based maintenance packages covering hosting, updates, backups, monitoring, and priority support. We build software to last."),

        new Faq(
            "What do I need to get started?",
            "It helps to know: what is the main purpose of the software, who are the users, and do you have any brand guidelines or designs? If you're unsure about any of it — just get in touch and we'll work through it together."),
    };
}
