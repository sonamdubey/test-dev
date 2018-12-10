CREATE TABLE [dbo].[DCRM_ADM_Roles] (
    [Id]        INT           NOT NULL,
    [Name]      NVARCHAR (30) NOT NULL,
    [IsActive]  BIT           CONSTRAINT [DF_DCRM_ADM_Roles_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedOn] DATETIME      NOT NULL,
    [UpdatedBy] NUMERIC (18)  NOT NULL,
    CONSTRAINT [PK_DCRM_ADM_Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

