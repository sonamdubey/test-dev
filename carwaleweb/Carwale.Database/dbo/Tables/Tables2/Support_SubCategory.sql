CREATE TABLE [dbo].[Support_SubCategory] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CategoryId]  NUMERIC (18)  NOT NULL,
    [Name]        VARCHAR (400) NOT NULL,
    [IsActive]    BIT           CONSTRAINT [DF_Support_SubCategory_IsActive] DEFAULT ((1)) NOT NULL,
    [AssignedTo]  NUMERIC (18)  NOT NULL,
    [SubAssignee] VARCHAR (500) NULL,
    CONSTRAINT [PK_Support_SubCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

