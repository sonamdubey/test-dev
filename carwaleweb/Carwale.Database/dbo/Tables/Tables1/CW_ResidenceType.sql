CREATE TABLE [dbo].[CW_ResidenceType] (
    [ID]            INT          IDENTITY (1, 1) NOT NULL,
    [ResidenceType] VARCHAR (20) NOT NULL,
    [MinStability]  INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

