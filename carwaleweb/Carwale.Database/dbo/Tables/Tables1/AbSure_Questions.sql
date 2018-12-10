CREATE TABLE [dbo].[AbSure_Questions] (
    [AbSure_QuestionsId]    NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [Question]              VARCHAR (1000) NULL,
    [AbSure_QCategoryId]    INT            NULL,
    [AbSure_QSubCategoryId] INT            NULL,
    [AbSure_QPositionId]    INT            NULL,
    [AbSure_QAreaId]        INT            NULL,
    [Type]                  SMALLINT       NULL,
    [Weightage]             SMALLINT       NULL,
    [IsActive]              BIT            CONSTRAINT [DF_AbSure_Questions_IsActive] DEFAULT ((1)) NULL,
    [UpdatedBy]             INT            NULL,
    [UpdatedOn]             DATETIME       NULL,
    [ABSureLogicVersion]    SMALLINT       NULL,
    [AbSure_CTQTypeId]      INT            NULL,
    CONSTRAINT [PK_AbSure_Questions] PRIMARY KEY CLUSTERED ([AbSure_QuestionsId] ASC)
);

