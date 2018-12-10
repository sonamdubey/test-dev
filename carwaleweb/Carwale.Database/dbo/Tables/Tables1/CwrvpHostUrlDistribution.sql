CREATE TABLE [dbo].[CwrvpHostUrlDistribution] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [HostUrl] VARCHAR (150) NULL
);


GO
CREATE CLUSTERED INDEX [ix_CwrvpHostUrlDistribution_Id]
    ON [dbo].[CwrvpHostUrlDistribution]([Id] ASC);

