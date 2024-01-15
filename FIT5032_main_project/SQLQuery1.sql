--Create table "Users"

Create TABLE [dbo].[Users](
	[Id] int IDENTITY (1,1) NOT NULL,
	[FirstName] nvarchar (max) NOT NULL,
	[LastName] nvarchar (max) NOT NULL,
		[UserId] nvarchar (max) NOT NULL,
	PRIMARY KEY (Id)
);
GO

-- Create table 'Patient'
Create TABLE [dbo].[Patients](
	[Id] int IDENTITY (1,1) NOT NULL,
	[Gender] nvarchar(max) NOT NULL,
		[UserId] int NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (UserId) REFERENCES Users (Id)
);
GO
