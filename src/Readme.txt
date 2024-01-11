===============================================================================
Clean Architecture = Onion Architecture or Hexagonal Architecture
-------------------------------------------------------------------------------
TaskManager.API (Presentation layer/Web API controllers and entry point)
TaskManager.Application (Business layer/Services and application logic)
TaskManager.Infrastructure (Data access and external dependencies)
TaskManager.Domain (Core domain models/Entities and interfaces)
===============================================================================
.
===============================================================================
.gitignore
-------------------------------------------------------------------------------
TaskManager.API/Properties/launchSettings.json
===============================================================================
.
===============================================================================
Terminal
-------------------------------------------------------------------------------
# If launchSettings.json was already tracked by Git before
git rm --cached TaskManager.API/Properties/launchSettings.json
===============================================================================

UserManager.API (Presentation layer/Web API controllers and entry point)
UserManager.Application (Business layer/Services and application logic)
UserManager.Infrastructure (Data access and external dependencies)
UserManager.Domain (Core domain models/Entities and interfaces)