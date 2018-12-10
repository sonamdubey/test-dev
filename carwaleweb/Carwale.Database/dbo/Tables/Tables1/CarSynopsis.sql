CREATE TABLE [dbo].[CarSynopsis] (
    [ID]               NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ModelId]          NUMERIC (18)   NOT NULL,
    [SmallDescription] VARCHAR (8000) NOT NULL,
    [Pros]             VARCHAR (500)  NOT NULL,
    [Cons]             VARCHAR (500)  NOT NULL,
    [Looks]            SMALLINT       NOT NULL,
    [Performance]      SMALLINT       NOT NULL,
    [FuelEfficiency]   SMALLINT       NOT NULL,
    [Comfort]          SMALLINT       NOT NULL,
    [Safety]           SMALLINT       NOT NULL,
    [Interiors]        SMALLINT       NOT NULL,
    [RideQuality]      SMALLINT       NOT NULL,
    [Handling]         SMALLINT       NOT NULL,
    [Braking]          SMALLINT       NOT NULL,
    [Overall]          SMALLINT       NOT NULL,
    [IsActive]         BIT            NOT NULL,
    [EntryDateTime]    DATETIME       NOT NULL,
    [LastUpdated]      DATETIME       NOT NULL,
    [FullDescription]  VARCHAR (MAX)  NULL,
    [CreatedBy]        INT            NULL,
    [CreatedOn]        DATETIME       CONSTRAINT [DF_CarSynopsis_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedBy]        INT            NULL,
    [UpdatedOn]        DATETIME       NULL,
    CONSTRAINT [PK_CarSynopsis] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CarSynopsis_ModelId]
    ON [dbo].[CarSynopsis]([ModelId] ASC);

