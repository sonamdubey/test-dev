CREATE TABLE [dbo].[DCRM_CallTypes] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [RoleId]   NUMERIC (18)  NOT NULL,
    [Name]     VARCHAR (100) NOT NULL,
    [IsActive] BIT           NOT NULL,
    CONSTRAINT [PK_DCRM_CallTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

