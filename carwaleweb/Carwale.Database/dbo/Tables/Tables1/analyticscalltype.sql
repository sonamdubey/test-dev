CREATE TABLE [dbo].[analyticscalltype] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [TBCType]  SMALLINT     NOT NULL,
    [CallId]   SMALLINT     NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [Priority] SMALLINT     NOT NULL
);

