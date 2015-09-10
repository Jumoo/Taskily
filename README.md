# Taskily
Takily is a survery tool, built to make Top Tasks surverys simpler. 

you can see taskily in action at https://taskily.azurewebsites.net/

## Running Locally
By default the source is configured to use a LocalDB file (in app_Data) 
and the EntityFramework is configured to setup the database on when it first starts,

Once running, you will need to create a local user by signing up to the site,
you can then make that user admin by changing the code in the (Migration Configurations
section)[https://github.com/Jumoo/Taskily/blob/master/TaskilyWeb/Migrations/Configuration.cs#L47]

