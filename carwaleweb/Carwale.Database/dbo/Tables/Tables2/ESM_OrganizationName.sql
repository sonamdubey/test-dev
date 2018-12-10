CREATE TABLE [dbo].[ESM_OrganizationName] (
    [id]             NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [OrgName]        VARCHAR (50) NOT NULL,
    [type]           SMALLINT     NULL,
    [IsActive]       BIT          CONSTRAINT [DF_ESM_OrganizationName_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedOn]      DATETIME     NOT NULL,
    [UpdatedBy]      NUMERIC (18) NOT NULL,
    [AccountManager] NUMERIC (18) NULL,
    CONSTRAINT [PK_ESM_OrganizationName] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Client 2-Agency', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESM_OrganizationName', @level2type = N'COLUMN', @level2name = N'type';

