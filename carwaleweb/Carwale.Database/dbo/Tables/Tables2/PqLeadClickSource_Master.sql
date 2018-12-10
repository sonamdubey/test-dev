CREATE TABLE [dbo].[PqLeadClickSource_Master] (
    [LeadClickSource] INT            NOT NULL,
    [Description]     NVARCHAR (255) NULL,
    [Platform]        FLOAT (53)     NULL,
    CONSTRAINT [PK_PqLeadCkSrcMaster] PRIMARY KEY CLUSTERED ([LeadClickSource] ASC)
);

