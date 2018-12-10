CREATE TABLE [dbo].[Con_SpareCost] (
    [Id]            NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VersionId]     NUMERIC (18)    NULL,
    [SPId]          NUMERIC (18)    NULL,
    [Quantity]      NUMERIC (18, 2) NULL,
    [UnitId]        INT             NULL,
    [IsFixed]       BIT             CONSTRAINT [DF_Con_SpareCost_IsFixed] DEFAULT ((1)) NOT NULL,
    [TotalCost]     NUMERIC (18, 2) NULL,
    [LabourCharges] NUMERIC (18, 2) NULL,
    [IsActive]      BIT             CONSTRAINT [DF_Con_SpareCost_IsActive] DEFAULT ((1)) NOT NULL,
    [LastUpdate]    DATETIME        NULL,
    CONSTRAINT [PK_Con_SpareCost] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Fixed, 2-Variable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Con_SpareCost', @level2type = N'COLUMN', @level2name = N'IsFixed';

