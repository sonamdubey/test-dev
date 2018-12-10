CREATE TABLE [dbo].[OLM_SC_Users] (
    [Id]         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [FullName]   VARCHAR (100) NOT NULL,
    [Mobile]     VARCHAR (15)  NOT NULL,
    [CityId]     INT           NOT NULL,
    [Email]      VARCHAR (100) NOT NULL,
    [IsSkodaOwn] BIT           NOT NULL,
    [ModelId]    INT           NOT NULL,
    [CreatedOn]  DATETIME      CONSTRAINT [DF_OLM_SC_Users_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]  DATETIME      NULL,
    [VisitCount] INT           CONSTRAINT [DF_OLM_SC_Users_VisitCount] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_OLM_SC_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

