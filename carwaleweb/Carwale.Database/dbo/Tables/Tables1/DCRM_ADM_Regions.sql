CREATE TABLE [dbo].[DCRM_ADM_Regions] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (30) NOT NULL,
    [IsActive]  BIT           CONSTRAINT [DF_DCRM_Regions_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedOn] DATETIME      NOT NULL,
    [UpdatedBy] NUMERIC (18)  NOT NULL,
    CONSTRAINT [PK_DCRM_Regions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

