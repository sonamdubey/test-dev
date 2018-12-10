CREATE TABLE [CRM].[LSConstantValueLogs] (
    [LSConstantValueLogId] INT          IDENTITY (1, 1) NOT NULL,
    [ConstantValueId]      SMALLINT     NOT NULL,
    [ConstantFieldId]      SMALLINT     NOT NULL,
    [Descr]                VARCHAR (50) NULL,
    [ConstantValue]        FLOAT (53)   NOT NULL,
    [CreatedOn]            DATETIME     CONSTRAINT [DF_LSConstantValueLogs_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]            DATETIME     NULL,
    CONSTRAINT [PK_LSConstantValueLogs] PRIMARY KEY CLUSTERED ([ConstantValueId] ASC)
);

