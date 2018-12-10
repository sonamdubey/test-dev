IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[SaveQAsubHead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[SaveQAsubHead]
GO

	
	CREATE PROCEDURE [CRM].[SaveQAsubHead]

	@HeadId			Int,
	@SubHeadName	Varchar(150),
	@TotalWeight	float,
	@CreatedBy		numeric(18,0),
	@UpdatedBy		numeric(18,0),
	@UpdatedOn		Datetime,
	@Type			Varchar(10)


    AS
    BEGIN
		INSERT INTO CRM.QASubhead (HeadId,SubheadName,Weight,CreatedBy,UpdateBy,UpdatedOn,Type) 
		VALUES (@HeadId,@SubheadName,@TotalWeight,@CreatedBy,@UpdatedBy,@UpdatedOn,@Type)	
	END
	