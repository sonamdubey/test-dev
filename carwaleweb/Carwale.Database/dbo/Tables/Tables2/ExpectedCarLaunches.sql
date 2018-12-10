CREATE TABLE [dbo].[ExpectedCarLaunches] (
    [Id]                INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarMakeId]         NUMERIC (18)   NOT NULL,
    [ModelName]         VARCHAR (250)  NULL,
    [ExpectedLaunch]    VARCHAR (250)  NULL,
    [EstimatedPrice]    VARCHAR (250)  NULL,
    [Description]       VARCHAR (4000) NULL,
    [SpecificationData] VARCHAR (3000) NULL,
    [CarwaleViews]      VARCHAR (1000) NULL,
    [IsLaunched]        BIT            CONSTRAINT [DF_ExpectedCarLaunches_IsLaunched] DEFAULT ((0)) NOT NULL,
    [PhotoName]         VARCHAR (100)  NULL,
    [DiscussionId]      NUMERIC (18)   CONSTRAINT [DF_ExpectedCarLaunches_DiscussonId] DEFAULT ((0)) NULL,
    [Sort]              INT            NULL,
    [CarModelId]        NUMERIC (18)   NULL,
    [IsReplicated]      BIT            CONSTRAINT [DF__ExpectedC__IsRep__49E981A9] DEFAULT ((1)) NULL,
    [HostURL]           VARCHAR (100)  CONSTRAINT [DF__ExpectedC__HostU__6C3E99AD] DEFAULT ('img.carwale.com') NULL,
    [LaunchDate]        DATETIME       NULL,
    [EstimatedPriceMin] NUMERIC (5, 2) NULL,
    [EstimatedPriceMax] NUMERIC (5, 2) NULL,
    [CWConfidence]      TINYINT        NULL,
    [UpdatedDate]       DATETIME       DEFAULT (getdate()) NULL,
    [IsDeleted]         BIT            CONSTRAINT [DF__ExpectedC__IsDel__229088BD] DEFAULT ((0)) NULL,
    [EntryDate]         DATETIME       DEFAULT (getdate()) NULL,
    [Priority]          INT            NULL,
    [CarVersionId]      INT            NULL,
    CONSTRAINT [PK_ExpectedCarLaunches] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_ExpectedCarLaunches_CarModelId]
    ON [dbo].[ExpectedCarLaunches]([CarModelId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Once the car is launched, update this id with research modelId.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ExpectedCarLaunches', @level2type = N'COLUMN', @level2name = N'CarModelId';

