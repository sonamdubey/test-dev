CREATE TABLE [dbo].[DCRM_ADM_ActionTypes] (
    [Id]       NUMERIC (18)  NOT NULL,
    [Name]     NVARCHAR (50) NOT NULL,
    [IsActive] BIT           CONSTRAINT [DF_DCRM_ADM_ActionTypes_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_DCRM_ADM_ActionTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

