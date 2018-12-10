CREATE TABLE [dbo].[CMS_Agencies] (
    [ID]            NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]          VARCHAR (200)  NOT NULL,
    [Aliases]       VARCHAR (500)  NULL,
    [ContactPerson] VARCHAR (500)  NOT NULL,
    [ContactNumber] VARCHAR (200)  NOT NULL,
    [OtherDetails]  VARCHAR (2000) NULL,
    [ContactEmail]  VARCHAR (100)  NULL,
    [IsActive]      BIT            NULL,
    CONSTRAINT [PK_CMS_Agencies] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

