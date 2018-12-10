CREATE TABLE [dbo].[NCS_FinOtherCharges] (
    [Id]           NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FAID]         NUMERIC (18)    NULL,
    [StartTenure]  INT             NULL,
    [EndTenure]    INT             NULL,
    [ChargesName]  VARCHAR (100)   NULL,
    [ChargesValue] NUMERIC (18, 2) NULL,
    [LastUpdated]  DATETIME        NULL,
    CONSTRAINT [PK_NCS_FinOtherCharges] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

