IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[SaveQAHead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[SaveQAHead]
GO

	CREATE PROCEDURE [CRM].[SaveQAHead]

	@QARolesId		Int,
	@HeadName		VARCHAR(250),
	@TotalWeight	FLOAT,
	@CreatedBy		NUMERIC(18,0),
	@UpdatedBy		NUMERIC(18,0),
	@UpdatedOn		DATETIME
	
	AS 
	BEGIN
		INSERT INTO CRM.QAHeads (RoleId,HeadName,TotalWeight,CreatedBy,UpdateDBy,UpdatedOn) 
		VALUES (@QARolesId,@HeadName,@TotalWeight,@CreatedBy,@UpdatedBy,@UpdatedOn)
	END