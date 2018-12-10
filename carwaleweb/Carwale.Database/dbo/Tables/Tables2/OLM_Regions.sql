CREATE TABLE [dbo].[OLM_Regions] (
    [Id]       NUMERIC (18) NOT NULL,
    [MakeId]   NUMERIC (18) NOT NULL,
    [Region]   VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_OLM_Regions_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_OLM_Regions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

