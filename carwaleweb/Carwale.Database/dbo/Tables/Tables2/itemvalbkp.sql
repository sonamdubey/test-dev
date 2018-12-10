CREATE TABLE [dbo].[itemvalbkp] (
    [CarVersionId]  INT           NULL,
    [ItemMasterId]  INT           NULL,
    [DataTypeId]    INT           NULL,
    [ItemValue]     FLOAT (53)    NULL,
    [UserDefinedId] INT           NULL,
    [CustomText]    VARCHAR (500) NULL,
    [CreatedOn]     DATETIME      NULL,
    [UpdatedOn]     DATETIME      NULL,
    [UpdatedBy]     INT           NULL
);

