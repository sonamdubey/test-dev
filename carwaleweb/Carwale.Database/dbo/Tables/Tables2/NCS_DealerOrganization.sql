CREATE TABLE [dbo].[NCS_DealerOrganization] (
    [ID]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]          VARCHAR (200) NOT NULL,
    [LoginId]       VARCHAR (50)  NULL,
    [PassWord]      VARCHAR (50)  NULL,
    [IsActive]      BIT           CONSTRAINT [DF_NCS_DealerOrganization_IsActive] DEFAULT ((1)) NULL,
    [UpdatedDate]   DATETIME      CONSTRAINT [DF_NCS_DealerOrganization_UpdatedDate] DEFAULT (getdate()) NULL,
    [UpdatedBy]     INT           NULL,
    [CreateDate]    DATETIME      CONSTRAINT [DF_NCS_DealerOrganization_CreateDate] DEFAULT (getdate()) NULL,
    [IsCWExecutive] BIT           CONSTRAINT [DF_NCS_DealerOrganization_IsCWExecutive] DEFAULT ((0)) NULL,
    [MakeId]        NUMERIC (18)  NULL,
    [CityId]        NUMERIC (18)  NULL,
    [IsNCD]         BIT           NULL,
    CONSTRAINT [PK_NCS_DealerOrganization] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for active and 0 for inactive', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCS_DealerOrganization', @level2type = N'COLUMN', @level2name = N'ID';

