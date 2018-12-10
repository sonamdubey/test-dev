CREATE TABLE [dbo].[DealerExclusion] (
    [ID]                SMALLINT      IDENTITY (1, 1) NOT NULL,
    [DealerId]          INT           NOT NULL,
    [ExclusionFromDate] DATE          NOT NULL,
    [ExclusionReason]   VARCHAR (200) NULL,
    [CreatedBy]         INT           NULL,
    [CreatedOn]         DATETIME      NULL,
    [IsES]              BIT           NULL
);

