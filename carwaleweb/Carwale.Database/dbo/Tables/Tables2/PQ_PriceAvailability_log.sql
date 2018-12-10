CREATE TABLE [dbo].[PQ_PriceAvailability_log] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [PriceAvailabilityId] INT           NOT NULL,
    [Name]                VARCHAR (200) NOT NULL,
    [Explanation]         VARCHAR (500) NOT NULL,
    [Type]                INT           NOT NULL,
    [UpdatedBy]           INT           NOT NULL,
    [UpdatedOn]           DATETIME      NOT NULL,
    [IsActive]            BIT           NOT NULL,
    [Changes]             VARCHAR (MAX) NOT NULL,
    [LogMessage]          VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

