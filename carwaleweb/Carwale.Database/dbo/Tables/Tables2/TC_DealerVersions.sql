CREATE TABLE [dbo].[TC_DealerVersions] (
    [ID]            INT          IDENTITY (1, 1) NOT NULL,
    [CWVersionId]   INT          NULL,
    [DWVersionName] VARCHAR (50) NULL,
    [DWModelId]     INT          NULL,
    [DealerId]      INT          NULL,
    [IsDeleted]     BIT          CONSTRAINT [DF_TC_DealerVersions_IsDeleted] DEFAULT ((0)) NULL,
    [EntryDate]     DATETIME     CONSTRAINT [DF_TC_DealerVersions_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]  DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

