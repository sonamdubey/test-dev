CREATE TABLE [dbo].[DealerExclusionDelete_Log] (
    [ID]                SMALLINT      NOT NULL,
    [DealerId]          INT           NOT NULL,
    [ExclusionFromDate] DATE          NOT NULL,
    [ExclusionReason]   VARCHAR (200) NULL,
    [CreatedOn]         DATETIME      NULL,
    [CreatedBy]         INT           NULL,
    [DeletedBy]         INT           NOT NULL,
    [DeletedOn]         DATETIME      NOT NULL
);

