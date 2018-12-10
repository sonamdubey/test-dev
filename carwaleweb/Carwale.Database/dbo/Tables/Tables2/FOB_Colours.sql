CREATE TABLE [dbo].[FOB_Colours] (
    [Id]       NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]     VARCHAR (100) NULL,
    [HexCode]  VARCHAR (6)   NULL,
    [IsActive] BIT           CONSTRAINT [DF_FOB_Colours_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_FOB_Colours] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

