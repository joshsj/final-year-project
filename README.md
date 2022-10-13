# RendezVous

My Bachelors dissertation: a proof-of-concept for businesses to decentralise
their clocking system by verifying employees at a place/time without
dedicated infrastructure or management. 

Based on the CQRS pattern, the API is built using .NET 6 alongside EFCore, FluentValidation, AutoMapper, and NSwag. Vite serves the Vue client, build with Element Plus and authenticated/authorized with Auth0.

## Resources

- [Integrating Vite with ASP.NET Core](https://blogs.taiga.nl/martijn/2021/02/24/integrating-vite-with-asp-net-core-a-winning-combination/)
  (i.e., how the Vue server is hosted by the API)
