CREATE TABLE [CD].[ItemValuesBkp280115] (
    [ItemValueId]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [CarVersionId]  NUMERIC (18)  NULL,
    [ItemMasterId]  BIGINT        NULL,
    [DataTypeId]    SMALLINT      NULL,
    [ItemValue]     FLOAT (53)    NULL,
    [UserDefinedId] INT           NULL,
    [CustomText]    VARCHAR (200) NULL,
    [CreatedOn]     DATETIME      NULL,
    [UpdatedOn]     DATETIME      NULL,
    [UpdatedBy]     VARCHAR (50)  NULL
);

