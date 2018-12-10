CREATE TABLE [dbo].[CRM_TDLocationTypes] (
    [Id]       SMALLINT     NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          NOT NULL,
    CONSTRAINT [PK_CRM_TDLocationTypes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

