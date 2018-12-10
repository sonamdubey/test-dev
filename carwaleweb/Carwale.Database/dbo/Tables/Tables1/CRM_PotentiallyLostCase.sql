CREATE TABLE [dbo].[CRM_PotentiallyLostCase] (
    [Id]            NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [CBDId]         NUMERIC (18)   NOT NULL,
    [Comment]       VARCHAR (1500) NOT NULL,
    [TaggedBy]      NUMERIC (18)   NOT NULL,
    [TaggedOn]      DATETIME       NOT NULL,
    [DealerId]      NUMERIC (18)   NOT NULL,
    [IsActionTaken] BIT            CONSTRAINT [DF_CRM_PotentiallyLostCase_IsActionTaken] DEFAULT ((0)) NULL,
    [UpdatedBy]     NUMERIC (18)   NULL,
    [UpdatedOn]     DATETIME       NULL,
    [Make]          VARCHAR (100)  NULL,
    [Model]         VARCHAR (100)  NULL
);

