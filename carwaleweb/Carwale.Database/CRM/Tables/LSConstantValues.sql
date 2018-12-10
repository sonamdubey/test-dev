CREATE TABLE [CRM].[LSConstantValues] (
    [ConstantValueId] SMALLINT     NOT NULL,
    [ConstantFieldId] SMALLINT     NOT NULL,
    [Descr]           VARCHAR (50) NULL,
    [ConstantValue]   FLOAT (53)   NOT NULL,
    [CreatedOn]       DATETIME     CONSTRAINT [DF_LSConstantValues_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]       DATETIME     NULL,
    CONSTRAINT [PK_LSConstantValues] PRIMARY KEY CLUSTERED ([ConstantValueId] ASC)
);


GO

CREATE TRIGGER [CRM].[TRG_BU_LSConstantValues]
on [CRM].[LSConstantValues] 
after Update
as
begin

If update(ConstantValue)
  insert into [CRM].[LSConstantValueLogs]
  ([ConstantValueId],
	[ConstantFieldId],
	[Descr] ,
	[ConstantValue],
	[UpdatedOn]
  )
 select ConstantValueId,ConstantFieldId,Descr,ConstantValue, GETDATE()
 from Deleted
end
