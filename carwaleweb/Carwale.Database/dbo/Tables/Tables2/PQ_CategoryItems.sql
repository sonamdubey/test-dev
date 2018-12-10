CREATE TABLE [dbo].[PQ_CategoryItems] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [CategoryId]   TINYINT        NOT NULL,
    [CategoryName] VARCHAR (200)  NOT NULL,
    [Type]         INT            NULL,
    [Scope]        INT            NULL,
    [UpdatedBy]    INT            NULL,
    [UpdatedOn]    DATETIME       NULL,
    [IsActive]     BIT            NULL,
    [Explanation]  VARCHAR (1000) NULL
);

