CREATE TABLE [dbo].[LA_Agencies] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [UserName]     VARCHAR (50)  NOT NULL,
    [Password]     VARCHAR (50)  NOT NULL,
    [Organization] VARCHAR (150) NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_LA_Agencies_IsActive] DEFAULT ((1)) NOT NULL,
    [IsTesting]    BIT           CONSTRAINT [DF_LA_Agencies_IsTesting] DEFAULT ((1)) NOT NULL,
    [BColor]       VARCHAR (7)   NULL,
    [FColor]       VARCHAR (7)   NULL,
    [HeadAgencyId] INT           NULL,
    CONSTRAINT [PK_LA_Agencies] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

