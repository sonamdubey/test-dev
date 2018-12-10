CREATE TABLE [dbo].[AbSure_DoubtfulReasons] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Reason]   VARCHAR (200) NULL,
    [IsActive] BIT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

