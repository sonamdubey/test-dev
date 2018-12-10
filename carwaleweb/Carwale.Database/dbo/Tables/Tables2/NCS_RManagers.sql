CREATE TABLE [dbo].[NCS_RManagers] (
    [Id]          NUMERIC (18)        IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100)       NOT NULL,
    [Designation] VARCHAR (100)       NULL,
    [HierId]      [sys].[hierarchyid] NOT NULL,
    [lvl]         AS                  ([HierId].[GetLevel]()) PERSISTED,
    [NodeCode]    AS                  ([HierId].[ToString]()) PERSISTED,
    [ReportTo]    NUMERIC (18)        NULL,
    [MakeId]      NUMERIC (18)        NOT NULL,
    [MakeGroupId] NUMERIC (18)        NULL,
    [EMail]       VARCHAR (250)       NULL,
    [MobileNo]    VARCHAR (50)        NULL,
    [LoginId]     VARCHAR (50)        NULL,
    [Password]    VARCHAR (50)        NULL,
    [IsActive]    BIT                 NOT NULL,
    [UpdatedDate] DATETIME            NULL,
    [UpdatedBy]   NUMERIC (18)        NULL,
    [IsExecutive] BIT                 NULL,
    [CityId]      NUMERIC (18)        NULL,
    [Type]        SMALLINT            NULL,
    [OprUserId]   NUMERIC (18)        NULL,
    CONSTRAINT [PK_RManagersTemp] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'OEMHierarchy = 0,CarwaleExecutive=1,BranchHead=2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCS_RManagers', @level2type = N'COLUMN', @level2name = N'Type';

