CREATE TABLE [CRM].[LSSection] (
    [SectionId] INT           NOT NULL,
    [Name]      VARCHAR (50)  NOT NULL,
    [Descr]     VARCHAR (150) NULL,
    [IsActive]  BIT           CONSTRAINT [DF_LSSection_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedOn] DATETIME      CONSTRAINT [DF_LSSection_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_LSCategory_1] PRIMARY KEY CLUSTERED ([SectionId] ASC)
);

