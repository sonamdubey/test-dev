CREATE TABLE [CRM].[LSConstantFields] (
    [ConstantFieldId] SMALLINT     NOT NULL,
    [Name]            VARCHAR (50) NOT NULL,
    [Descr]           VARCHAR (50) NOT NULL,
    [CreateOn]        DATETIME     CONSTRAINT [DF_LSConstantFields_CreateOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_LSConstantFields] PRIMARY KEY CLUSTERED ([ConstantFieldId] ASC)
);

