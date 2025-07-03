# text-forum-dos
An updated approach towards how I would make a neat text forum app


# Planning

## Rough Notes

We're making a simple chat / forum app here. 
I would love to go big picture with it but lets get the basics working. 

- [X] Use DDD?
  - [ ] Use CQRS?
  - [ ] Use MediatR?
  - [ ] Use FluentValidation?
  - [ ] Use AutoMapper? ( Not sure about this one )
- [ ] Use Serilog?
- [X] Use TDD?
  - [ ] Use FluentAssertions?
  - [X] Use xUnit
- [ ] Think About Frontend ( Depends on how far I get )
  - [ ] Otherwise postman / swagger
- [ ] Neaten checklist
- [X] Use Aspire
- [X] Template out the DDD base with clean architecture ( https://github.com/ardalis/CleanArchitecture )

## Requirement Checklists

- [X] Backend
  - [X] Dotnet project + C#
  - [ ] Datastore
    - [ ] Out of the box integration with dotnet
  - [ ] Authentication must be baked into the backend
    - [ ] MFA can be added
    - [ ] SSO can be added
  - [ ] This needs to service < 100 concurrent users
  - [ ] An exposable API for automated integrations should be available
    - [ ] This should at minimum have a postman collection to test with

- [ ] System Key Features
  - [ ] User Authentication
  - [ ] User Management
  - [ ] Creating posts
  - [ ] Liking posts
    - [ ] A user can like a post once
    - [ ] A user cant like their own post
  - [ ] Users can view posts anonymously
  - [ ] Commenting on posts
  - [ ] Retrieve posts
  - [ ] Add moderators
    - [ ] Moderators can tag posts as misleading / false information
  - [ ] Retrieve and page posts 
    - [ ] For posts comments as well
  - [ ] Filtering with a date range / author / tags 
  - [ ] Sorting by date / like count
- [ ] Restrictions
  - [ ] Users can't edit or delete their posts for"ethics", but i believe we're selling data :^)


- [ ] Instructions TODO
- [ ] Need to design a neat message servicing feature for the users to be notified