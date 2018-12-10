CREATE TABLE [dbo].[TC_AlertType] (
    [AlertType_Id]  INT           IDENTITY (1, 1) NOT NULL,
    [AlertTypeName] VARCHAR (30)  NOT NULL,
    [Descr]         VARCHAR (100) NULL,
    [CreatedDate]   DATE          CONSTRAINT [DF_TC_AlertType_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [IsActive]      BIT           CONSTRAINT [DF_TC_AlertType_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TC_AlertType] PRIMARY KEY CLUSTERED ([AlertType_Id] ASC)
);

