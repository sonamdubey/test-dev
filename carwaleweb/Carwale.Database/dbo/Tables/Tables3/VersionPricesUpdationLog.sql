CREATE TABLE [dbo].[VersionPricesUpdationLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VersionId] [int] NOT NULL,
	[CityId] [int] NOT NULL,
	[IsMetallic] [bit] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdatedBy] [int] NOT NULL,
 CONSTRAINT [PK_VersionPricesUpdationLog] PRIMARY KEY CLUSTERED ( [Id] ASC )
);