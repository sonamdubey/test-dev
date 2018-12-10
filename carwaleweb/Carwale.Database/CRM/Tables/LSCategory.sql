CREATE TABLE [CRM].[LSCategory] (
    [CategoryId]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (50)  NOT NULL,
    [Description]      VARCHAR (100) NULL,
    [Value]            FLOAT (53)    NULL,
    [IsActive]         BIT           NOT NULL,
    [CreatedOn]        DATETIME      NOT NULL,
    [UpdatedOn]        DATETIME      NULL,
    [UpdatedBy]        INT           NULL,
    [LeadScoreVersion] INT           NULL,
    CONSTRAINT [PK_LSCategory_2] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);

