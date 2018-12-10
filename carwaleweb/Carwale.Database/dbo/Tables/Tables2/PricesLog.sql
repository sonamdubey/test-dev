CREATE TABLE [dbo].[PricesLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VersionId] [int] NOT NULL,
	[CityId] [int] NOT NULL,
	[IsMetallic] [int] NOT NULL,
	[PQ_CategoryItemId] [int] NOT NULL,
	[PQ_CategoryItemValue] [int] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[Status] [char](1) NOT NULL,
 CONSTRAINT [PK_PricesLog] PRIMARY KEY CLUSTERED ( [Id] ASC )
);