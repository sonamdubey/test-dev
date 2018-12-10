CREATE TABLE [dbo].[Qpr_Outcomes] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Qpr_RatingDataId] INT            NULL,
    [CreatedOn]        DATETIME       CONSTRAINT [DF_Qpr_Outcomes_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [KRA]              VARCHAR (1000) NULL,
    [KPI]              VARCHAR (1000) NULL,
    [Weightage]        INT            NULL,
    [SelfScore]        INT            NULL,
    [LastQtrScore]     INT            NULL,
    CONSTRAINT [PK__Qpr_Outc__3214EC0714E0BC8D] PRIMARY KEY CLUSTERED ([Id] ASC)
);

