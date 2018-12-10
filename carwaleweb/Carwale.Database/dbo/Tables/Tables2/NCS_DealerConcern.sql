CREATE TABLE [dbo].[NCS_DealerConcern] (
    [Id]        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [OrgId]     NUMERIC (18)   NOT NULL,
    [Concern]   VARCHAR (2000) NOT NULL,
    [CreatedOn] DATETIME       CONSTRAINT [DF_NCS_DealerConcern_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_NCS_DealerConcern] PRIMARY KEY CLUSTERED ([Id] ASC)
);

