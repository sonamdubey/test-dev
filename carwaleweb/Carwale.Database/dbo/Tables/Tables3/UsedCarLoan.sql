CREATE TABLE [dbo].[UsedCarLoan] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarVersionId] NUMERIC (18)  NULL,
    [CustomerId]   NUMERIC (18)  NULL,
    [CustomerName] VARCHAR (200) NULL,
    [EMail]        VARCHAR (100) NULL,
    [ContactNo]    VARCHAR (100) NULL,
    [InqDate]      DATETIME      NULL,
    [ProfileId]    VARCHAR (50)  NULL,
    [CityId]       NUMERIC (18)  NULL,
    [Comments]     VARCHAR (500) NULL,
    [IsAccepted]   BIT           CONSTRAINT [DF_UsedCarLoan_IsAccepted] DEFAULT ((0)) NULL,
    [UpdatedOn]    DATETIME      NULL,
    [UpdatedBy]    NUMERIC (18)  NULL,
    CONSTRAINT [PK_UsedCarLoan] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

