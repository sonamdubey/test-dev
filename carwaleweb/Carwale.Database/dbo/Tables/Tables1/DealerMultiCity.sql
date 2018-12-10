CREATE TABLE [dbo].[DealerMultiCity] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Dealerid] INT            NULL,
    [Cities]   NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

