CREATE TABLE [dbo].[PQ_PriceAvailability] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (200) NOT NULL,
    [Explanation] VARCHAR (500) NOT NULL,
    [Type]        INT           NOT NULL,
    [IsActive]    BIT           NOT NULL,
    [UpdatedBy]   INT           NOT NULL,
    [UpdatedOn]   DATETIME      NOT NULL,
    CONSTRAINT [PK_PQ_PriceAvailabilty] PRIMARY KEY CLUSTERED ([Id] ASC)
);

